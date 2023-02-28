using Character;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CameraStateBase CurrentStateBase;

    //serialize fields
    [Header("Cinemachine"), Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow"), SerializeField]
    public GameObject _cinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up"), SerializeField]
    public float _topClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down"), SerializeField]
    public float _bottomClamp = -30.0f;
    [Tooltip("Additional degrees to override the camera. Useful for fine tuning camera position when locked"), SerializeField]
    public float _cameraAngleOverride = 0.0f;

    [Header("References"), SerializeField]
    public Rigidbody _rigidbodyKayak;
    [SerializeField]
    public CharacterManager _characterManager;
    [SerializeField]
    public InputManagement _input;

    [Header("Rotation Values")]
    [SerializeField] public float _balanceRotationMultiplier = 1f;
    [SerializeField, Range(0, 0.1f)] public float _balanceRotationZLerp = 0.01f;

    [Header("Camera")]
    [SerializeField, Range(10, 100)] public float _multiplierValueRotation = 20.0f;
    [SerializeField, Range(0, 10)] public float _multiplierValuePosition = 2;
    [ReadOnly] public bool CanMoveCameraMaunally = true;

    [Header("Virtual Camera")]
    [SerializeField] public CinemachineVirtualCamera _virtualCamera;
    [SerializeField, Range(0, 5)] public float _multiplierFovCamera = 1;

    [Header("Input rotation smooth values")]
    [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the mouse/stick camera movement X input value when released")]
    public float _lerpTimeX = 0.02f;
    [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the mouse/stick camera movement Y input value when released")]
    public float _lerpTimeY = 0.06f;


    [Header("Lerp")]
    [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the field of view of camera depending on the speed of the player")]
    public float _lerpFOV = .01f;
    [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the rotation of the camera when the player moves")]
    public float _lerpLocalRotationMove = 0.005f;
    [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the position of the camera when the player moves")]
    public float _lerpLocalPositionMove = .005f;
    [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the position of the camera when the player is not moving")]
    public float _lerpLocalPositionNotMoving = 0.01f;
    [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the position of the camera when the player is not moving")]
    public float _lerpLocalRotationNotMoving = 0.01f;


    [Header("Pendulum")]
    [Tooltip("The angle you have on the first pendulum")]
    public float PendulumFirstAngle = 10;
    [Tooltip("The speed you have in the first pendulum")]
    public float PendulumFirstSpeed = 1;
    [Tooltip("The degree of angle you remove from the pendulum at each pendulum")]
    public float PendulumRemoveAngle = 1;
    [Tooltip("The speed you take away from the speed with each pendulum")]
    public float PendulumRemoveSpeed = 0.1f;
    [Tooltip("Division of the position in X according to the angle")]
    [SerializeField] public float _divisionMoveForceX = 10;
    [Tooltip("Division of the position in Y according to the angle")]
    [SerializeField] public float _divisionMoveForceY = 50;

    //camera
    public float CameraBaseFov;
    public Vector3 CameraTargetBasePos;
    public float RotationZ = 0;
    //cinemachine yaw&pitch
    public float CinemachineTargetYaw;
    public float CinemachineTargetPitch;
    //inputs
    public float LastInputX;
    public float LastInputY;
    //other
    public bool StartTimerDeath = false;
    /* //pendulum //pas tej jsp si on le garde
     public float PendulumValue;
     public float SpeedPendulum;
     public float PendulumValueMoins;
     public float SpeedPendulumMoins;
     public bool PlayOnce = false;
     public bool Left;
    */

    private void Awake()
    {
        CameraNavigationState navigationState = new CameraNavigationState(this, this);
        CurrentStateBase = navigationState;

        CameraTargetBasePos = _cinemachineCameraTarget.transform.localPosition;
        CameraBaseFov = _virtualCamera.m_Lens.FieldOfView;
    }

    private void Start()
    {
        CurrentStateBase.EnterState(this);
    }
    private void Update()
    {
        CurrentStateBase.UpdateState(this);
        FieldOfView();

    }
    private void FixedUpdate()
    {
        CurrentStateBase.FixedUpdate(this);
    }
    public void SwitchState(CameraStateBase stateBaseCharacter)
    {
        CurrentStateBase = stateBaseCharacter;
        stateBaseCharacter.EnterState(this);
    }

    private void FieldOfView()
    {
        float velocityXZ = Mathf.Abs(_rigidbodyKayak.velocity.x) + Mathf.Abs(_rigidbodyKayak.velocity.z);
        _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(_virtualCamera.m_Lens.FieldOfView,
                                            CameraBaseFov + (velocityXZ * _multiplierFovCamera),
                                            _lerpFOV);
    }

    public void ApplyRotationCamera()
    {
        _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(
       CinemachineTargetPitch /*+ _cameraAngleOverride*/, //pitch
       CinemachineTargetYaw, //yaw
        RotationZ); //z rotation
    }
    public void ApplyRotationDeathCamera()
    {
        _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(
        CinemachineTargetPitch + _cameraAngleOverride, //pitch
        CinemachineTargetYaw, //yaw
        RotationZ); //z rotation
    }

    public void SmoothResetRotateZ()
    {
        RotationZ = Mathf.Lerp(RotationZ, 0, 0.01f);
    }
    public void ResetNavigationValue()
    {
        //SmoothResetRotateZ();
        _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = 7;
        #region pendulum
        //_pendulumValue = PendulumFirstAngle;
        //_speedPendulum = PendulumFirstSpeed;
        //_pendulumValueMoins = PendulumRemoveAngle;
        //_speedPendulumMoins = PendulumRemoveSpeed;
        //_playOnce = false;
        //DeadState = false;
        #endregion
        //StartTimerDeath = false;
    }

    public void ResetCameraLocalPos()
    {
        Vector3 localPos = _cinemachineCameraTarget.transform.localPosition;
        localPos.x = CameraTargetBasePos.x;
        localPos.y = CameraTargetBasePos.y;
        localPos.z = CameraTargetBasePos.z;
        _cinemachineCameraTarget.transform.localPosition = localPos;
    }



}
