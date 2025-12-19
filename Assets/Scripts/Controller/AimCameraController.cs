using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineCamera))]
public class AimCameraController : MonoBehaviour {
    [SerializeField] private InputActionReference lookInput;

    [SerializeField] private float sensitivity = 1.5f;

    [SerializeField] private float pitchMin = -40f;
    [SerializeField] private float pitchMax = 80f;

    private Transform aimTarget;
    private float yaw, pitch;

    private void Awake() {
        aimTarget = GetComponent<CinemachineCamera>().Follow;
    }


    private void Start() {
        Vector3 angles = aimTarget.rotation.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    private void OnEnable() {
        lookInput.action.Enable();
    }

    private void OnDisable() {
        lookInput.action.Disable();
    }

    // Update is called once per frame
    private void Update() {
        Vector2 lookInputValue = lookInput.action.ReadValue<Vector2>();

        yaw += lookInputValue.x * sensitivity * Time.deltaTime;
        pitch -= lookInputValue.y * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        aimTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    public void SetYawPitchFromCameraForward(Transform freelookCameraTransform) {
        Vector3 flateForward = freelookCameraTransform.forward;
        flateForward.y = 0;
        flateForward.Normalize();

        if (flateForward.sqrMagnitude < 0.001f)
            return;

        yaw = Mathf.Atan2(flateForward.x, flateForward.z) * Mathf.Rad2Deg;
        pitch = Mathf.Asin(flateForward.y) * Mathf.Rad2Deg;
        aimTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}