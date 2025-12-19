using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor {
    public class WaterPlaneGenerate : ScriptableWizard {
        public string objectName = "Water";

        public Material material;
        public Vector2Int segments = new(40, 40); // Number of pieces for dividing plane
        public Vector2 scale = new(120, 120);

        private static Camera _cam;
        private static Camera _lastUsedCam;

        // Generated plane meshes are saved and loaded from Plane Meshes folder (you can change it to whatever you want)
        public static string AssetSaveLocation = "Assets/Models/LowPolyWater/";

        [MenuItem("GameObject/3D Object/LowPolyWater")]
        static void CreateWizard() {
            _cam = Camera.current;
            // Hack because camera.current doesn't return editor camera if scene view doesn't have focus
            if (!_cam) {
                _cam = _lastUsedCam;
            } else {
                _lastUsedCam = _cam;
            }

            // Check if the asset save location folder exists
            // If the folder doesn't exists, create it
            if (!Directory.Exists(AssetSaveLocation)) {
                Directory.CreateDirectory(AssetSaveLocation);
            }

            // Open Wizard
            var wizard = DisplayWizard<WaterPlaneGenerate>("Generate LowPoly Water Mesh");
        }

        void OnWizardUpdate() {
            // Max segment number is 254, because a mesh can't have more
            // than 65000 vertices (254^2 = 64516 max. number of vertices)
            segments.x = Mathf.Clamp(segments.x, 1, 254);
            segments.y = Mathf.Clamp(segments.y, 1, 254);
        }

        private void OnWizardCreate() {
            // Create an empty gamobject
            GameObject plane = new GameObject(objectName);

            // Create Mesh Filter and Mesh Renderer components
            MeshFilter meshFilter = plane.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = plane.AddComponent<MeshRenderer>();
            //WaterPositionUpdater waterPositionUpdater = plane.AddComponent<WaterPositionUpdater>();
            //waterPositionUpdater.segmentRatio = new Vector2(scale.x / segments.x, scale.y / segments.y);

            // Generate a name for the mesh that will be created
            string planeMeshAssetName = $"{plane.name}_{scale.x}x{scale.y}_seg_{segments.x}x{segments.y}.asset";

            // Load the mesh from the save location
            Mesh m = (Mesh)AssetDatabase.LoadAssetAtPath(AssetSaveLocation + planeMeshAssetName, typeof(Mesh));

            // If there isn't a mesh located under assets, create the mesh
            if (m == null) {
                m = new Mesh();
                m.name = plane.name;

                int hCount2 = segments.x + 1;
                int vCount2 = segments.y + 1;
                int numTriangles = segments.x * segments.y * 6;
                int numVertices = hCount2 * vCount2;

                Vector3[] vertices = new Vector3[numVertices];
                Vector2[] uvs = new Vector2[numVertices];
                int[] triangles = new int[numTriangles];
                Vector4[] tangents = new Vector4[numVertices];
                Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
                Vector2 anchorOffset = Vector2.zero;

                int index = 0;
                Vector2 uvFactor = Vector2.one / segments;
                Vector2 segmentScale = scale / segments;

                // Generate the vertices
                for (float y = 0.0f; y < vCount2; y++) {
                    for (float x = 0.0f; x < hCount2; x++) {
                        vertices[index] = new Vector3(x * segmentScale.x - scale.x / 2f - anchorOffset.x, 0.0f,
                            y * segmentScale.y - scale.y / 2f - anchorOffset.y);

                        tangents[index] = tangent;
                        uvs[index++] = new Vector2(x * uvFactor.x, y * uvFactor.y);
                    }
                }

                // Reset the index and generate triangles
                index = 0;
                for (int y = 0; y < segments.y; y++) {
                    for (int x = 0; x < segments.x; x++) {
                        triangles[index] = (y * hCount2) + x;
                        triangles[index + 1] = ((y + 1) * hCount2) + x;
                        triangles[index + 2] = (y * hCount2) + x + 1;

                        triangles[index + 3] = ((y + 1) * hCount2) + x;
                        triangles[index + 4] = ((y + 1) * hCount2) + x + 1;
                        triangles[index + 5] = (y * hCount2) + x + 1;
                        index += 6;
                    }
                }

                // Update the mesh properties (vertices, UVs, triangles, normals etc.)
                m.vertices = vertices;
                m.uv = uvs;
                m.triangles = triangles;
                m.tangents = tangents;
                m.RecalculateNormals();

                // Save the newly created mesh under save location to reload later
                AssetDatabase.CreateAsset(m, AssetSaveLocation + planeMeshAssetName);
                AssetDatabase.SaveAssets();
            }

            // Update mesh
            meshRenderer.sharedMaterial = new Material(material);
            meshFilter.sharedMesh = m;
            m.RecalculateBounds();

            // Set parent to currently selected GameObject
            if (Selection.activeGameObject != null) {
                plane.transform.SetParent(Selection.activeGameObject.transform);
            }


            Selection.activeObject = plane;
        }
    }
}