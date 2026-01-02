using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Animator animator;
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

            _moveInput = _actions.Player.Move.ReadValue<Vector2>();

            Vector3 forward = playerCamera.forward;
            Vector3 right = playerCamera.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = forward * _moveInput.y + right * _moveInput.x;

            bool isSprinting = _actions.Player.Sprint.IsPressed();
            float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;

            Vector3 horizontalMove = moveDirection * targetSpeed;

            if (animator)
            {
                animator.SetFloat(_speedId, moveDirection.magnitude * targetSpeed, 0.1f, Time.deltaTime);
                animator.SetBool(_sprintId, isSprinting);
            }

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(forward);
            }

            if (_controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f; 
            }

            if (_actions.Player.Jump.WasPressedThisFrame() && _controller.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                if (animator) animator.SetTrigger(_jumpId);
            }

            _velocity.y += gravity * Time.deltaTime;

            _controller.Move((horizontalMove + _velocity) * Time.deltaTime);
        }
    }
}