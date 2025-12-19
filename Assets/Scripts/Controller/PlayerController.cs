using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour {
        // @formatter:off
        [Header("Components")]
        [SerializeField] private Animator animator;

        [Header("Movement Settings")]
        [SerializeField] private CinemachineCamera freelookCamera;
        [SerializeField] private CinemachineCamera aimCamera;
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float sprintSpeed = 5f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private bool shouldFaceMoveDirection = false;
        [SerializeField] private CameraSwitcher cameraSwitcher;

        [Header("Animation Settings")]
        [SerializeField] private string speedParameterName = "Speed";
        [SerializeField] private string sprintParameterName = "Sprint";
        [SerializeField] private string jumpTriggerName = "Jump";
        [SerializeField] private float speedMultiplier = 2.8f;
        [SerializeField] private float animationDampTime = 0f;
        [SerializeField] private float isMovingThreshold = 0.05f;
        // @formatter:on

        private InputSystem_Actions _actions;
        private CharacterController _controller;
        private Transform _aimTarget;
        private Vector2 _moveInput;
        private Vector3 _velocity;
        private int _movementSpeedParameterId, _sprintParameterId, _jumpTriggerId;

        void Awake() {
            _controller = GetComponent<CharacterController>();
            _movementSpeedParameterId = Animator.StringToHash(speedParameterName);
            _sprintParameterId = Animator.StringToHash(sprintParameterName);
            _jumpTriggerId = Animator.StringToHash(jumpTriggerName);
            _actions = new InputSystem_Actions();
            if (aimCamera)
                _aimTarget = aimCamera.Follow;
        }

        private void OnEnable() {
            _actions.Player.Move.performed += OnMove;
            _actions.Player.Move.canceled += OnMove;
            _actions.Player.Jump.performed += OnJump;
            _actions.Player.Jump.canceled += OnJump;
            _actions.Player.Enable();
        }

        private void OnDisable() {
            _actions.Player.Move.performed -= OnMove;
            _actions.Player.Move.canceled -= OnMove;
            _actions.Player.Jump.performed -= OnJump;
            _actions.Player.Jump.canceled -= OnJump;
            _actions.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context) {
            _moveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context) {
            if (context.performed && _controller.isGrounded) {
                StartCoroutine(WaitAnimationJump());
                if (animator)
                    animator.SetTrigger(_jumpTriggerId);
            }
        }

        private IEnumerator WaitAnimationJump() {
            yield return new WaitForSeconds(0.2f);
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        private void Update() {
            Vector3 forward, right;
            if (cameraSwitcher.IsAiming) {
                forward = transform.forward;
                right = transform.right;
            } else {
                forward = freelookCamera.transform.forward;
                right = freelookCamera.transform.right;
            }

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = forward * _moveInput.y + right * _moveInput.x;

            bool isSprinting = _actions.Player.Sprint.IsPressed();
            float speed = isSprinting ? sprintSpeed : walkSpeed;

            // Horizontal movement (xz)
            Vector3 oldVelocity = new Vector3(_controller.velocity.x, 0, _controller.velocity.z);
            Vector3 horizontalVelocity = Vector3.Slerp(oldVelocity, moveDirection * speed, Time.deltaTime * 10f);
            // Apply move speed to animation parameter (e.g., "movementSpeed")
            if (animator) {
                float movementSpeed = moveDirection.magnitude * speedMultiplier;
                if (movementSpeed < isMovingThreshold)
                    movementSpeed = 0;

                animator.SetFloat(_movementSpeedParameterId, movementSpeed, animationDampTime, Time.deltaTime);
                animator.SetBool(_sprintParameterId, isSprinting);
            }

            if (cameraSwitcher.IsAiming) {
                Vector3 aimDirection = _aimTarget.forward;
                aimDirection.y = 0;
                aimDirection.Normalize();
                if (aimDirection.sqrMagnitude > 0.01f) {
                    Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                }
            } else if (shouldFaceMoveDirection && moveDirection.sqrMagnitude > 0.01f) {
                Quaternion quaternion = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * 5f);
            }

            // Apply gravity continuously
            if (!_controller.isGrounded)
                _velocity.y += gravity * Time.deltaTime;

            // Combine horizontal and vertical velocities
            Vector3 finalVelocity = new Vector3(horizontalVelocity.x, _velocity.y, horizontalVelocity.z);
            _controller.Move(finalVelocity * Time.deltaTime);
        }
    }
}