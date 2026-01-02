using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    [Header("Prefab")]
    public GameObject floatingTextPrefab; 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowText(string content, Vector3 position, Color color)
    {
        if (floatingTextPrefab != null)
        {
            GameObject go = Instantiate(floatingTextPrefab, position, Quaternion.identity);

            var script = go.GetComponent<FloatingText>();
            if (script != null)
            {
                script.Setup(content, color);
            }
        }
    }
}