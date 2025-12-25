using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Animator animator;
        // 現在只需要一個相機
        [SerializeField] private Transform playerCamera;

        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float sprintSpeed = 5f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -9.8f;

        [Header("Animation Settings")]
        [SerializeField] private string speedParameterName = "Speed";
        [SerializeField] private string sprintParameterName = "Sprint";
        [SerializeField] private string jumpTriggerName = "Jump";

        private InputSystem_Actions _actions;
        private CharacterController _controller;
        private Vector2 _moveInput;
        private Vector3 _velocity;
        private int _speedId, _sprintId, _jumpId;

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _speedId = Animator.StringToHash(speedParameterName);
            _sprintId = Animator.StringToHash(sprintParameterName);
            _jumpId = Animator.StringToHash(jumpTriggerName);
            _actions = new InputSystem_Actions();

            // 自動抓取主相機，省去手動拉的動作（如果場景中有 MainCamera）
            if (playerCamera == null && Camera.main != null)
            {
                playerCamera = Camera.main.transform;
            }
        }

        private void OnEnable() => _actions.Player.Enable();
        private void OnDisable() => _actions.Player.Disable();

        private void Update()
        {
            if (playerCamera == null) return;

            // 1. 取得移動方向 (相對於相機)
            _moveInput = _actions.Player.Move.ReadValue<Vector2>();

            Vector3 forward = playerCamera.forward;
            Vector3 right = playerCamera.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = forward * _moveInput.y + right * _moveInput.x;

            // 2. 處理速度與動畫
            bool isSprinting = _actions.Player.Sprint.IsPressed();
            float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;

            // 這裡直接用 moveDirection 帶入移動
            Vector3 horizontalMove = moveDirection * targetSpeed;

            if (animator)
            {
                animator.SetFloat(_speedId, moveDirection.magnitude * targetSpeed, 0.1f, Time.deltaTime);
                animator.SetBool(_sprintId, isSprinting);
            }

            // 3. 旋轉角色 (如果是第一人稱，通常角色會跟著相機轉)
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                // 如果是 FPS，你也可以讓角色始終跟隨相機水平轉向
                transform.rotation = Quaternion.LookRotation(forward);
            }

            // 4. 重力與跳躍
            if (_controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f; // 保持接地
            }

            if (_actions.Player.Jump.WasPressedThisFrame() && _controller.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                if (animator) animator.SetTrigger(_jumpId);
            }

            _velocity.y += gravity * Time.deltaTime;

            // 5. 最終移動
            _controller.Move((horizontalMove + _velocity) * Time.deltaTime);
        }
    }
}