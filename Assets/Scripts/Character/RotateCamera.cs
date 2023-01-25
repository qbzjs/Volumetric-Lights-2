using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    #region Singleton
    public static RotateCamera Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    #endregion
    
    [Header("Cinemachine")
     ,Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degrees to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;


    // private GameplayInputs _input;
    private GameObject _mainCamera;

    private bool _isCurrentDeviceMouse = true;

    private Vector3 _cameraStartRotation;
    private float _rotateMultiplierJoystickOldInputSystem = 250f;

    private const float _threshold = 0.01f;

    private float _timerRotateCamera = 0;
    private float _timeToUnlock = 1.5f;

    private void Start()
    {
        if (Camera.main != null)
        {
            _mainCamera = Camera.main.gameObject;
        }
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _cameraStartRotation = _mainCamera.transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            _isCurrentDeviceMouse = !_isCurrentDeviceMouse;
        if (Input.GetKeyDown(KeyCode.I))
            LockCameraPosition = !LockCameraPosition;
        if (Input.GetKeyDown(KeyCode.R))
        {
            LockCamera();
        }
        UnLockCamera();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        if (LockCameraPosition == false)
        {
            float deltaTimeMultiplier = _isCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            if (_isCurrentDeviceMouse == true)
            {
                _cinemachineTargetYaw += Input.GetAxis("Mouse X") * deltaTimeMultiplier;
                _cinemachineTargetPitch += Input.GetAxis("Mouse Y") * deltaTimeMultiplier;
            }
            else
            {
                var multiplier = _rotateMultiplierJoystickOldInputSystem;
                _cinemachineTargetYaw += (Input.GetAxis("Joystick X") * multiplier) * deltaTimeMultiplier;
                _cinemachineTargetPitch += (Input.GetAxis("Joystick Y") * multiplier) * deltaTimeMultiplier;
            }
        }
        else
        {
            var rotation = CinemachineCameraTarget.transform.localRotation;

            CinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(rotation, Quaternion.Euler(new Vector3(rotation.x, 0, rotation.z)), Time.deltaTime * 2);

            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _cinemachineTargetPitch = 0;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }


    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void LockCamera()
    {
        LockCameraPosition = true;
        _timerRotateCamera = 0;
    }
    public void UnLockCamera()
    {
        if (LockCameraPosition == true && _timerRotateCamera <= _timeToUnlock)
        {
            _timerRotateCamera += Time.deltaTime;
        }
        else if(_timerRotateCamera >= _timeToUnlock)
        {
            LockCameraPosition = false;
            _timerRotateCamera = 0;
        }
    }

}
