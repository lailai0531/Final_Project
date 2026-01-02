using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class EndSceneMovement : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;

    [Header("跳躍與重力")]
    public float jumpHeight = 1.5f;     
    public float gravityValue = -9.81f; 

    private CharacterController controller;
    private Transform mainCameraTransform;
    private Vector3 playerVelocity;     
    private bool groundedPlayer;       

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        Vector2 input = Vector2.zero;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input.y = 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input.y = -1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input.x = -1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input.x = 1;

        if (input != Vector2.zero && mainCameraTransform != null)
        {
            Vector3 camForward = mainCameraTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = mainCameraTransform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 direction = (camForward * input.y + camRight * input.x).normalized;

            controller.Move(direction * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    [Header("推物體設定")]
    public float pushPower = 2.0f; 
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.linearVelocity = pushDir * pushPower;
    }
}