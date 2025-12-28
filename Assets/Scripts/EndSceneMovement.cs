using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class EndSceneMovement : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;

    [Header("跳躍與重力")]
    public float jumpHeight = 1.5f;     // 跳躍高度
    public float gravityValue = -9.81f; // 重力

    // 內部變數
    private CharacterController controller;
    private Transform mainCameraTransform;
    private Vector3 playerVelocity;     // 垂直速度 (處理跳躍與掉落)
    private bool groundedPlayer;        // 是否在地面

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
        // 1. 檢查著地狀態
        groundedPlayer = controller.isGrounded;

        // 如果在地面且速度向下，重置垂直速度 (給一個微小的向下力確保貼地)
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        // 2. 讀取輸入
        Vector2 input = Vector2.zero;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input.y = 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input.y = -1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input.x = -1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input.x = 1;

        // 3. 計算移動 (如果有輸入)
        if (input != Vector2.zero && mainCameraTransform != null)
        {
            // 取得攝影機的水平前方 (確保移動方向是跟著鏡頭轉的)
            Vector3 camForward = mainCameraTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = mainCameraTransform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 direction = (camForward * input.y + camRight * input.x).normalized;

            // 移動角色
            controller.Move(direction * moveSpeed * Time.deltaTime);

            // 旋轉角色
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        // 4. 跳躍邏輯 (新增)
        if (Keyboard.current.spaceKey.wasPressedThisFrame && groundedPlayer)
        {
            // 物理公式：v = sqrt(h * -2 * g)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        // 5. 應用重力
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    [Header("推物體設定")]
    public float pushPower = 2.0f; // 推力大小 (如果不夠大可以調高)

    // 這是一個 Unity 內建的回呼函式，當 CharacterController 碰到 Collider 時會自動觸發
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 1. 檢查碰到的東西有沒有 Rigidbody
        Rigidbody body = hit.collider.attachedRigidbody;

        // 2. 如果沒有 Rigidbody，或是設定為 Kinematic (不受物理影響)，就不能推
        if (body == null || body.isKinematic)
        {
            return;
        }

        // 3. 避免推動地板 (如果撞擊方向是向下的，代表踩在上面，不推)
        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        // 4. 計算推的方向 (只在水平面推，不要把物體推到地底下)
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // 5. 施加力道
        // 使用 Velocity 比較穩定，適合推箱子
        body.linearVelocity = pushDir * pushPower;
    }
}