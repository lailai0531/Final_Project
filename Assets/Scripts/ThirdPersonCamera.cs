using UnityEngine;
using UnityEngine.InputSystem; 
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; 

    [Header("¸òÀH³]©w")]
    public float distance = 8.0f;       
    public float height = 5.0f;         
    public float heightDamping = 2.0f;  
    public float rotationDamping = 3.0f;

    [Header("·Æ¹«±±¨î")]
    public bool useMouseRotation = true; 
    public float rotationSpeed = 2.0f;   

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (useMouseRotation)
        {
            float mouseX = Mouse.current.delta.x.ReadValue();
            float mouseY = Mouse.current.delta.y.ReadValue();

            currentX += mouseX * rotationSpeed;
            currentY -= mouseY * rotationSpeed;

            currentY = Mathf.Clamp(currentY, -10, 60);
        }

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * rotationDamping);

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}