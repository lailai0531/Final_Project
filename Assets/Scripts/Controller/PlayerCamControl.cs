using UnityEngine;

public class SoftCameraLook : MonoBehaviour
{
    [Header("視角移動範圍 (度)")]
    public float maxTiltX = 15f; 
    public float maxTiltY = 20f; 

    [Header("平滑度")]
    public float smoothTime = 5f; 

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = Input.mousePosition.x / Screen.width;
        float mouseY = Input.mousePosition.y / Screen.height;

        float inputX = (mouseX - 0.5f) * 2;
        float inputY = (mouseY - 0.5f) * 2;

        Quaternion targetRotation = initialRotation * Quaternion.Euler(-inputY * maxTiltX, inputX * maxTiltY, 0f);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smoothTime);
    }
}
