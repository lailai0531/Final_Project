using Controller;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour {
    [SerializeField] private CinemachineCamera freelookCamera;
    [SerializeField] private CinemachineCamera aimCamera;
    [SerializeField] private CinemachineCamera projectileCamera;
    [SerializeField] private CinemachineCamera targetViewCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerController player;
    [SerializeField] private InputActionReference aim;

    private bool _isAiming;
    private Transform _yawTarget;
    private Transform _pitchTarget;

    private AimCameraController _aimCameraController;
    private CinemachineOrbitalFollow _freeLookFallow;
    private CinemachineInputAxisController _freeLookController;
    public bool IsAiming => _isAiming;

    private void Start() {
        _aimCameraController = aimCamera.GetComponent<AimCameraController>();

        _freeLookFallow = freelookCamera.GetComponent<CinemachineOrbitalFollow>();
        _freeLookController = freelookCamera.GetComponent<CinemachineInputAxisController>();

        if (!projectileCamera)
            projectileCamera = freelookCamera;
        if (!targetViewCamera)
            targetViewCamera = freelookCamera;

        SwitchToThirdPersonView();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable() {
        aim.action.Enable();
    }

    private void OnDisable() {
        aim.action.Disable();
    }

    private void Update() {
        bool aimPressed = aim.action.IsPressed();

        if (aimPressed && !_isAiming) {
            SwitchToAimView();
        } else if (!aimPressed && _isAiming) {
            SwitchToThirdPersonView();
        }
    }

    private void SwitchToAimView() {
        _isAiming = true;
        // Debug.Log("Enter Aim Mode");

        SnapFreeLookCamera();

        aimCamera.Priority = 20;
        freelookCamera.Priority = 10;
        projectileCamera.Priority = 10;
        targetViewCamera.Priority = 10;
    }

    private void SnapFreeLookCamera() {
        _freeLookController.enabled = false;
        _aimCameraController.enabled = true;

        _aimCameraController.SetYawPitchFromCameraForward(freelookCamera.transform);
    }

    public void SwitchToThirdPersonView() {
        _isAiming = false;
        // Debug.Log("Exit Aim Mode");

        SnapAimCamera();

        aimCamera.Priority = 10;
        projectileCamera.Priority = 10;
        targetViewCamera.Priority = 10;
        freelookCamera.Priority = 20;
    }

    private void SnapAimCamera() {
        _freeLookController.enabled = true;
        _aimCameraController.enabled = false;

        Vector3 playerForward = player.transform.forward;
        float angle = Mathf.Atan2(playerForward.x, playerForward.z) * Mathf.Rad2Deg;
        _freeLookFallow.HorizontalAxis.Value = angle;
    }

    public void SwitchToProjectile(Transform projectileFollow) {
        _freeLookController.enabled = false;
        _aimCameraController.enabled = false;

        aimCamera.Priority = 10;
        freelookCamera.Priority = 10;
        projectileCamera.Priority = 20;
        targetViewCamera.Priority = 10;

        projectileCamera.Follow = projectileFollow;
    }

    public void SwitchToTargetView() {
        _freeLookController.enabled = false;
        _aimCameraController.enabled = false;

        aimCamera.Priority = 10;
        freelookCamera.Priority = 10;
        projectileCamera.Priority = 10;
        targetViewCamera.Priority = 20;
    }
}