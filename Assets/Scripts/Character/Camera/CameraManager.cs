using Character;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CameraStateBase CurrentStateBase;

    //ref
    [Header("Cinemachine"), Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    public GameObject CinemachineCameraTargetFollow;
    public Animator AnimatorRef;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Header("References")]
    public Rigidbody RigidbodyKayak;
    public CharacterManager CharacterManager;
    public InputManagement Input;

    [Header("Rotation Values")]
    public float BalanceRotationMultiplier = 1f;
    [Range(0, 0.1f)] public float BalanceRotationZLerp = 0.01f;

    [Header("Camera")]
    [Range(0, 10)] public float MultiplierValueRotation = 20.0f;
    [Range(0, 0.1f), Tooltip("The lerp value applied to the rotation of the camera when the player moves")]
    public float LerpLocalRotationMove = 0.005f;
    [Range(0, 10)] public float MultiplierValuePosition = 2;
    [Range(0, 0.1f), Tooltip("The lerp value applied to the position of the camera when the player moves")]
    public float LerpLocalPositionMove = .005f;
    [ReadOnly] public bool CanMoveCameraManually = true;

    [Header("Virtual Camera")]
    public CinemachineBrain Brain;
    public CinemachineVirtualCamera VirtualCamera;
    [Range(0, 5)] public float MultiplierFovCamera = 1;

    [Header("Input rotation smooth values")]
    [Range(0, 0.1f), Tooltip("The lerp value applied to the mouse/stick camera movement X input value when released")]
    public float LerpTimeX = 0.02f;
    [Range(0, 0.1f), Tooltip("The lerp value applied to the mouse/stick camera movement Y input value when released")]
    public float LerpTimeY = 0.06f;
    [Range(0, 10), Tooltip("The time it takes the camera to move back behind the boat after the last input")]
    public float TimerCameraReturnBehindBoat = 3.0f;
    [Tooltip("The curve of force in function of the joystick position X")]
    public AnimationCurve JoystickFreeRotationX;
    [Tooltip("The curve of force in function of the joystick position Y")]
    public AnimationCurve JoystickFreeRotationY;

    [Header("Lerp")]
    [Range(0, 0.1f), Tooltip("The lerp value applied to the field of view of camera depending on the speed of the player")]
    public float LerpFOV = .01f;
    [Range(0, 0.1f), Tooltip("The lerp value applied to the position of the camera when the player is not moving")]
    public float LerpLocalPositionNotMoving = 0.01f;
    [Range(0, 0.1f), Tooltip("The lerp value applied to the position of the camera when the player is not moving")]
    public float LerpLocalRotationNotMoving = 0.01f;

    [Header("Death")]
    [Tooltip("Additional degrees to override the camera. Useful for fine tuning camera position when locked")]
    public float ValueAddForTopDownWhenDeath = 0.1f;
    [Tooltip("The value to add for the camera to move backwards")]
    public float ValueAddForDistanceWhenDeath = 0.05f;
    [Tooltip("The value that the camera distance should reach")]
    public float MaxValueDistanceToStartDeath = 10f;

    [Header("Shake Camera")]
    public float AmplitudShakeWhenUnbalanced = 0.5f;
    public float AmplitudShakeWhenWaterFlow = 0.2f;
    public float AmplitudShakeWhenWaterWave = 0.2f;
    public Waves Waves;

    //camera
    [HideInInspector] public float CameraAngleOverride = 0.0f;
    [HideInInspector] public float CameraBaseFov;
    [HideInInspector] public Vector3 CameraTargetBasePos;
    [HideInInspector] public float RotationZ = 0;
    //cinemachine yaw&pitch
    [HideInInspector] public float CinemachineTargetYaw;
    [HideInInspector] public float CinemachineTargetPitch;
    //inputs
    [HideInInspector] public float LastInputX;
    [HideInInspector] public float LastInputY;
    //other
    [HideInInspector] public bool StartDeath = false;
    [HideInInspector] public bool WaterFlow = false;
    //pas tej jsp si on le garde

    //[Header("Pendulum")]
    //[Tooltip("The angle you have on the first pendulum")]
    //public float PendulumFirstAngle = 10;
    //[Tooltip("The speed you have in the first pendulum")]
    //public float PendulumFirstSpeed = 1;
    //[Tooltip("The degree of angle you remove from the pendulum at each pendulum")]
    //public float PendulumRemoveAngle = 1;
    //[Tooltip("The speed you take away from the speed with each pendulum")]
    //public float PendulumRemoveSpeed = 0.1f;
    //[Tooltip("Division of the position in X according to the angle")]
    //public float DivisionMoveForceX = 10;
    //[Tooltip("Division of the position in Y according to the angle")]
    //public float DivisionMoveForceY = 50;

    //pendulum 
    //public float PendulumValue;
    //public float SpeedPendulum;
    //public float PendulumValueMoins;
    //public float SpeedPendulumMoins;
    //public bool PlayOnce = false;
    //public bool Left;


    private void Awake()
    {
        if (Waves == null)
        {
            Debug.Log("Ta oublier de ref les waves chef");
            Waves = FindObjectOfType<Waves>();
        }

        CinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        CameraTargetBasePos = CinemachineCameraTarget.transform.localPosition;

        CameraBaseFov = VirtualCamera.m_Lens.FieldOfView;

        CameraNavigationState navigationState = new CameraNavigationState(this, this);
        CurrentStateBase = navigationState;
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
        float velocityXZ = Mathf.Abs(RigidbodyKayak.velocity.x) + Mathf.Abs(RigidbodyKayak.velocity.z);
        VirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(VirtualCamera.m_Lens.FieldOfView,
                                            CameraBaseFov + (velocityXZ * MultiplierFovCamera),
                                            LerpFOV);
    }
    #region Methods
    public void ApplyRotationCamera()
    {
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(
        CinemachineTargetPitch, //pitch
        CinemachineTargetYaw, //yaw
        RotationZ); //z rotation
    }
    public void ApplyRotationCameraWhenCharacterDeath()
    {
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(
        CinemachineTargetPitch + CameraAngleOverride, //pitch
        CinemachineTargetYaw, //yaw
        RotationZ); //z rotation
    }

    public void SmoothResetRotateZ()
    {
        RotationZ = Mathf.Lerp(RotationZ, 0, 0.01f);
    }
    public void ResetNavigationValue()
    {
        VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = 7;
        StartDeath = false;

        #region pendulum
        //_pendulumValue = PendulumFirstAngle;
        //_speedPendulum = PendulumFirstSpeed;
        //_pendulumValueMoins = PendulumRemoveAngle;
        //_speedPendulumMoins = PendulumRemoveSpeed;
        //_playOnce = false;
        //DeadState = false;
        #endregion
    }

    public void ResetCameraLocalPos()
    {
        Vector3 localPos = CinemachineCameraTarget.transform.localPosition;
        localPos.x = CameraTargetBasePos.x;
        localPos.y = CameraTargetBasePos.y;
        localPos.z = CameraTargetBasePos.z;
        CinemachineCameraTarget.transform.localPosition = localPos;
    }


    public void LastInputValue()
    {
        //last input value
        LastInputX = CurrentStateBase.ClampAngle(LastInputX, -5.0f, 5.0f);
        LastInputY = CurrentStateBase.ClampAngle(LastInputY, -1.0f, 1.0f);
        LastInputX = Mathf.Lerp(LastInputX, 0, LerpTimeX);
        LastInputY = Mathf.Lerp(LastInputY, 0, LerpTimeY);

        //apply value to camera
        CinemachineTargetYaw += LastInputX;
        CinemachineTargetPitch += LastInputY;

    }

    public void MakeTargetFollowRotationWithKayak()
    {
        Vector3 rotation = CinemachineCameraTargetFollow.transform.rotation.eulerAngles;
        Vector3 kayakRotation = RigidbodyKayak.gameObject.transform.eulerAngles;
        CinemachineCameraTargetFollow.transform.rotation = Quaternion.Euler(new Vector3(rotation.x, kayakRotation.y, rotation.z));
    }

    public void MakeSmoothCameraBehindBoat()
    {
        Quaternion localRotation = CinemachineCameraTarget.transform.localRotation;
        Vector3 cameraTargetLocalPosition = CinemachineCameraTarget.transform.localPosition;

        CinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(localRotation, Quaternion.Euler(new Vector3(0, 0, localRotation.z)), LerpLocalRotationNotMoving);
        cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x, 0, LerpLocalPositionNotMoving);
        CinemachineTargetEulerAnglesToRotation(cameraTargetLocalPosition);
    }
    public void ResetCameraBehindBoat()
    {
        Quaternion localRotation = CinemachineCameraTarget.transform.localRotation;
        Vector3 cameraTargetLocalPosition = CinemachineCameraTarget.transform.localPosition;

        CinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(localRotation, Quaternion.Euler(new Vector3(0, 0, localRotation.z)), 1f);
        cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x, 0, LerpLocalPositionNotMoving);
        CinemachineTargetEulerAnglesToRotation(cameraTargetLocalPosition);
    }
    public void CinemachineTargetEulerAnglesToRotation(Vector3 targetLocalPosition)
    {
        CinemachineCameraTarget.transform.localPosition = targetLocalPosition;
        CinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        if (CinemachineCameraTarget.transform.rotation.eulerAngles.x > 180)
        {
            CinemachineTargetPitch = CinemachineCameraTarget.transform.rotation.eulerAngles.x - 360;
        }
        else
        {
            CinemachineTargetPitch = CinemachineCameraTarget.transform.rotation.eulerAngles.x;
        }
    }
    public void ShakeCamera(float intensity)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    }
    #endregion
}
