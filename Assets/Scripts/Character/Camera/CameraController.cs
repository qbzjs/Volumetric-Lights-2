using Character.State;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Camera
{
    public class CameraController : MonoBehaviour
    {
        #region Singleton
        public static CameraController Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;
        }
        #endregion

        #region PlayerStateEnum
        public enum PlayerState
        {
            NavigationState = 0,
            UnbalanceState = 1,
            FightState = 2,
            DeadState = 3
        }
        #endregion

        //serialize fields
        [Header("Cinemachine"), Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow"), SerializeField]
        private GameObject _cinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up"), SerializeField]
        private float _topClamp = 70.0f;
        [Tooltip("How far in degrees can you move the camera down"), SerializeField]
        private float _bottomClamp = -30.0f;
        [Tooltip("Additional degrees to override the camera. Useful for fine tuning camera position when locked"), SerializeField]
        private float _cameraAngleOverride = 0.0f;

        [Header("References"), SerializeField]
        private Rigidbody _rigidbodyKayak;
        [SerializeField]
        private CharacterManager _characterManager;
        [SerializeField]
        private InputManagement _input;

        [Header("Rotation Values")]
        [SerializeField] private float _balanceRotationMultiplier = 1f;
        [SerializeField, Range(0, 0.1f)] private float _balanceRotationZLerp = 0.01f;

        [Header("Camera")]
        [SerializeField, Range(10, 100)] private float _multiplierValueRotation = 20.0f;
        [SerializeField, Range(0, 10)] private float _multiplierValuePosition = 2;
        [ReadOnly] public bool CanMoveCameraMaunally = true;

        [Header("Virtual Camera")]
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField, Range(0, 5)] private float _multiplierFovCamera = 1;

        [Header("Input rotation smooth values")]
        [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the mouse/stick camera movement X input value when released")]
        private float _lerpTimeX = 0.02f;
        [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the mouse/stick camera movement Y input value when released")]
        private float _lerpTimeY = 0.06f;


        [Header("Lerp")]
        [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the field of view of camera depending on the speed of the player")]
        private float _lerpFOV = .01f;
        [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the rotation of the camera when the player moves")]
        private float _lerpLocalRotationMove = 0.005f;
        [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the position of the camera when the player moves")]
        private float _lerpLocalPositionMove = .005f;
        [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the position of the camera when the player is not moving")]
        private float _lerpLocalPositionNotMoving = 0.01f;
        [SerializeField, Range(0, 0.1f), Tooltip("The lerp value applied to the position of the camera when the player is not moving")]
        private float _lerpLocalRotationNotMoving = 0.01f;


        [Header("Player State")]
        [ReadOnly] public PlayerState StatePlayer;
        //[ReadOnly] public bool NormalState = true;
        //[ReadOnly] public bool DeadState = false;

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
        [SerializeField] float _divisionMoveForceX = 10;
        [Tooltip("Division of the position in Y according to the angle")]
        [SerializeField] float _divisionMoveForceY = 50;



        //pendulum
        private float _pendulumValue;
        private float _speedPendulum;
        private float _pendulumValueMoins;
        private float _speedPendulumMoins;
        //cinemachine yaw&pitch
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        //inputs
        private float _lastInputX;
        private float _lastInputY;
        //camera
        private Vector3 _cameraTargetBasePos;
        private float _cameraBaseFov;
        private float _rotationZ = 0;
        private bool _left;
        private float _lastRotaZ;
        //other
        private bool _playOnce = false;
        public bool StartTimerDeath = false;

        private void Start()
        {
            _cameraTargetBasePos = _cinemachineCameraTarget.transform.localPosition;
            ResetValueDead();
            _cinemachineTargetYaw = _cinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _cameraBaseFov = _virtualCamera.m_Lens.FieldOfView;
        }

        private void Update()
        {
            CameraRotation();
            FielOfView();

            if (StatePlayer == PlayerState.NavigationState /*NormalState*/ && Mathf.Abs(_rotationZ) >= 0.1f)
            {
                SmoothResetRotateZ();
            }

            if (StatePlayer == PlayerState.DeadState /*DeadState*/)
            {
                Isdead();
            }
        }
        private void FielOfView()
        {
            float velocityXZ = Mathf.Abs(_rigidbodyKayak.velocity.x) + Mathf.Abs(_rigidbodyKayak.velocity.z);
            _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(_virtualCamera.m_Lens.FieldOfView, _cameraBaseFov + (velocityXZ * _multiplierFovCamera), _lerpFOV);
        }
        private void CameraRotation()
        {
            //rotate freely with inputs
            bool rotateInput = Mathf.Abs(_input.Inputs.RotateCamera.x) + Mathf.Abs(_input.Inputs.RotateCamera.y) >= 0.5f;
            const float minimumVelocityToReplaceCamera = 1f;

            if (rotateInput && CanMoveCameraMaunally)
            {
                //Cinemachine yaw/pitch
                _cinemachineTargetYaw += _input.Inputs.RotateCamera.x;
                _cinemachineTargetPitch += _input.Inputs.RotateCamera.y;

                //last inputs
                _lastInputX = _input.Inputs.RotateCamera.x != 0 ? _input.Inputs.RotateCamera.x : _lastInputX;
                _lastInputY = _input.Inputs.RotateCamera.y != 0 ? _input.Inputs.RotateCamera.y : _lastInputY;
            }
            //manage rotate to stay behind boat
            else if (Mathf.Abs(_rigidbodyKayak.velocity.x + _rigidbodyKayak.velocity.z) > minimumVelocityToReplaceCamera ||
                     Mathf.Abs(_characterManager.CurrentStateBase.RotationStaticForceY) > minimumVelocityToReplaceCamera / 20)
            {
                //avoid last input to be 0
                if (_lastInputX != 0 || _lastInputY != 0)
                {
                    _lastInputX = 0;
                    _lastInputY = 0;
                }

                //get target rotation
                Quaternion localRotation = _cinemachineCameraTarget.transform.localRotation;
                Quaternion targetQuaternion = Quaternion.Euler(new Vector3(0,
                    -(_characterManager.CurrentStateBase.RotationStaticForceY + _characterManager.CurrentStateBase.RotationPaddleForceY) * _multiplierValueRotation,
                    localRotation.z));

                //get camera local position
                Vector3 cameraTargetLocalPosition = _cinemachineCameraTarget.transform.localPosition;


                const float rotationThreshold = 0.15f;
                float rotationStaticY = _characterManager.CurrentStateBase.RotationStaticForceY;
                float rotationPaddleY = _characterManager.CurrentStateBase.RotationPaddleForceY;

                //calculate camera rotation & position
                if (Mathf.Abs(rotationStaticY) > rotationThreshold / 2 || // if kayak is rotating
                    Mathf.Abs(rotationPaddleY) > rotationThreshold)
                {
                    _cinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(localRotation, targetQuaternion, _lerpLocalRotationMove);

                    if (/*_input.Inputs.RotateLeft >= _input.Inputs.DEADZONE ||
                        _input.Inputs.RotateRight >= _input.Inputs.DEADZONE*/
                        Mathf.Abs(rotationStaticY) > rotationThreshold / 2)// if kayak is rotating
                    {
                        print("cc");
                        cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x, 0, _lerpLocalPositionNotMoving);
                    }
                    else if (Mathf.Abs(rotationPaddleY) > rotationThreshold)// if kayak is moving
                    {
                        cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x,
                            (rotationStaticY + rotationPaddleY) * _multiplierValuePosition, //value
                            _lerpLocalPositionMove); //time lerp
                        cameraTargetLocalPosition.z = 0;
                    }
                }
                else
                {
                    /*if (_input.Inputs.PaddleLeft == false && _input.Inputs.PaddleRight == false &&
                        _input.Inputs.RotateLeft < _input.Inputs.DEADZONE  && _input.Inputs.RotateRight < _input.Inputs.DEADZONE)
                    {*/

                    _cinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(localRotation, Quaternion.Euler(new Vector3(0, 0, localRotation.z)), _lerpLocalRotationNotMoving);
                    cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x, 0, _lerpLocalPositionNotMoving);
                    //}
                }

                //apply camera rotation & position
                _cinemachineCameraTarget.transform.localPosition = cameraTargetLocalPosition;
                _cinemachineTargetYaw = _cinemachineCameraTarget.transform.rotation.eulerAngles.y;

                if (_cinemachineCameraTarget.transform.rotation.eulerAngles.x > 180)
                {
                    _cinemachineTargetPitch = _cinemachineCameraTarget.transform.rotation.eulerAngles.x - 360;
                }
                else
                {
                    _cinemachineTargetPitch = _cinemachineCameraTarget.transform.rotation.eulerAngles.x;
                }

            }
            else
            {
                //last input value
                _lastInputX = ClampAngle(_lastInputX, -5.0f, 5.0f);
                _lastInputY = ClampAngle(_lastInputY, -1.0f, 1.0f);
                _lastInputX = Mathf.Lerp(_lastInputX, 0, _lerpTimeX);
                _lastInputY = Mathf.Lerp(_lastInputY, 0, _lerpTimeY);

                //apply value to camera
                _cinemachineTargetYaw += _lastInputX;
                _cinemachineTargetPitch += _lastInputY;
            }

            //Clamp yaw + pitch
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);

            RotateCamInZ();

            //apply pitch+yaw+z to camera
            _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(
                _cinemachineTargetPitch /*+ _cameraAngleOverride*/, //pitch
                _cinemachineTargetYaw, //yaw
                _rotationZ); //z rotation

        }

        public void SmoothResetRotateZ()
        {
            _rotationZ = Mathf.Lerp(_rotationZ, 0, 0.01f);
        }

        private void RotateCamInZ()
        {
            if (Mathf.Abs(_characterManager.Balance) <= _characterManager.BalanceDeathLimit)
            {
                if (Mathf.Abs(_characterManager.Balance) > _characterManager.BalanceLimit)
                {
                    if (_characterManager.Balance > 0)
                    {
                        _rotationZ = Mathf.Lerp(_rotationZ, _characterManager.Balance + 10, 0.01f);
                    }
                    else if (_characterManager.Balance < 0)
                    {
                        _rotationZ = Mathf.Lerp(_rotationZ, _characterManager.Balance - 10, 0.01f);
                    }
                }
            }
        }


        private void PlayOnce()
        {
            if (_pendulumValue > 0 && _playOnce == false)
            {
                _pendulumValue -= _pendulumValueMoins;
                _speedPendulum -= _speedPendulumMoins;
                _playOnce = true;
            }
        }

        public void ResetCameraLocalPos()
        {
            Vector3 localPos = _cinemachineCameraTarget.transform.localPosition;
            localPos.x = _cameraTargetBasePos.x;
            localPos.y = _cameraTargetBasePos.y;
            localPos.z = _cameraTargetBasePos.z;
            _cinemachineCameraTarget.transform.localPosition = localPos;
        }

        public void ResetValueDead()
        {
            SmoothResetRotateZ();
            _pendulumValue = PendulumFirstAngle;
            _speedPendulum = PendulumFirstSpeed;
            _pendulumValueMoins = PendulumRemoveAngle;
            _speedPendulumMoins = PendulumRemoveSpeed;
            _playOnce = false;
            //DeadState = false;

            StatePlayer = PlayerState.NavigationState;
            StartTimerDeath = false;
        }

        private void Isdead()
        {

            if (_rotationZ >= _pendulumValue)
            {
                _left = false;
                PlayOnce();
            }
            else if (_rotationZ <= -_pendulumValue)
            {
                _left = true;
                PlayOnce();
            }

            if (_rotationZ > -_pendulumValue && _rotationZ < _pendulumValue)
                _playOnce = false;


            if (_speedPendulum > 0 || _pendulumValue > 0)
            {
                if (_left == true)
                {
                    _rotationZ += 0.1f * _speedPendulum;
                }
                else
                {
                    _rotationZ -= 0.1f * _speedPendulum;
                }
            }

            //position camera in function of z value
            Vector3 localPos = _cinemachineCameraTarget.transform.localPosition;
            localPos.x = -(_rotationZ / _divisionMoveForceX);
            if (_rotationZ > 0)
                localPos.y = -(_rotationZ / _divisionMoveForceY) + _cameraTargetBasePos.y;
            else
                localPos.y = (_rotationZ / _divisionMoveForceY) + _cameraTargetBasePos.y;
            localPos.z = _cameraTargetBasePos.z;
            _cinemachineCameraTarget.transform.localPosition = localPos;


            if (_speedPendulum <= 0.1f || _pendulumValue <= 0.1f)
            {
                StartTimerDeath = true;
                SmoothResetRotateZ();
            }
        }
        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}
