using Character.State;
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
        private CharacterManager characterManager;
        [SerializeField]
        private InputManagement _input;

        [Header("Rotation Values")]
        [SerializeField] private float _balanceRotationMultiplier = 1f;
        [SerializeField, Range(0, 0.1f)] private float _balanceRotationZLerp = 0.01f;

        [Header("Camera")]
        [SerializeField, Range(10, 100)] private float _multiplierValueRotation = 20.0f;
        [SerializeField, Range(0, 10)] private float _multiplierValuePosition = 2;

        [Header("Input rotation smooth values")]
        [SerializeField, Range(-10, -1f)] private float _rotationXMinClamp = -5f;
        [SerializeField, Range(1, 10f)] private float _rotationXMaxClamp = 5f;
        [SerializeField, Range(-5, -0f)] private float _rotationYMinClamp = -1f;
        [SerializeField, Range(0, 5f)] private float _rotationYMaxClamp = 1f;
        [SerializeField, Range(0, 0.1f)] private float _lerpTimeX = 0.02f;
        [SerializeField, Range(0, 0.1f)] private float _lerpTimeY = 0.06f;

        //private values
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private float _lastInputX;
        private float _lastInputY;
        private void Start()
        {
            _cinemachineTargetYaw = _cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        }

        private void Update()
        {
            CameraRotation();
        }

        private void CameraRotation()
        {
            //rotate freely with inputs
            if (_input.Inputs.RotateCameraClick)
            {
                //Cinemachine yaw/pitch
                _cinemachineTargetYaw += _input.Inputs.RotateCamera.x;
                _cinemachineTargetPitch += _input.Inputs.RotateCamera.y;

                //last inputs
                _lastInputX = _input.Inputs.RotateCamera.x != 0 ? _input.Inputs.RotateCamera.x : _lastInputX;
                _lastInputY = _input.Inputs.RotateCamera.y != 0 ? _input.Inputs.RotateCamera.y : _lastInputY;
            }
            //manage rotate to stay behind boat
            else if (Mathf.Abs(_rigidbodyKayak.velocity.x + _rigidbodyKayak.velocity.z) > 0.01f || Mathf.Abs(characterManager.CurrentStateBase.RotationStaticForceY) > 0.01f)
            {
                //avoid last input to be 0
                if (_lastInputX != 0 || _lastInputY != 0)
                {
                    _lastInputX = 0;
                    _lastInputY = 0;
                }
                
                //get target rotation
                Quaternion rotation = _cinemachineCameraTarget.transform.localRotation;
                Quaternion targetQuaternion = Quaternion.Euler(new Vector3(0,
                    -(characterManager.CurrentStateBase.RotationStaticForceY + characterManager.CurrentStateBase.RotationPaddleForceY) * _multiplierValueRotation,
                    rotation.z));

                //get camera local position
                Vector3 cameraTargetLocalPosition = _cinemachineCameraTarget.transform.localPosition;
                
                
                const float rotationThreshold = 0.15f;
                
                //calculate camera rotation & position
                if (Mathf.Abs(characterManager.CurrentStateBase.RotationStaticForceY) > rotationThreshold || // if kayak is rotating
                    Mathf.Abs(characterManager.CurrentStateBase.RotationPaddleForceY) > rotationThreshold)
                {
                    _cinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(rotation, targetQuaternion, Time.deltaTime * 2);
                    if (characterManager.CurrentStateBase.RotationPaddleForceY > rotationThreshold)
                    {
                        cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x, (characterManager.CurrentStateBase.RotationStaticForceY + characterManager.CurrentStateBase.RotationPaddleForceY) * _multiplierValuePosition, .01f);
                        cameraTargetLocalPosition.z = 0;
                    }
                }
                else //if kayak isn't rotating
                {
                    _cinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(rotation, Quaternion.Euler(new Vector3(0, 0, rotation.z)), Time.deltaTime * 2);
                    cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x, 0, .01f);
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

            //apply pitch+yaw to camera
            _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + _cameraAngleOverride, //pitch
                _cinemachineTargetYaw, //yaw
                _cinemachineCameraTarget.transform.rotation.eulerAngles.z); //stay the same

            //z balance rotation 
            Vector3 cameraTargetRotationEuler = _cinemachineCameraTarget.transform.rotation.eulerAngles;
            float cameraRotationZ = characterManager.CurrentStateBase.Balance * _balanceRotationMultiplier;
            Quaternion targetRotation = Quaternion.Euler(cameraTargetRotationEuler.x, cameraTargetRotationEuler.y, cameraRotationZ); //apply rotation
            _cinemachineCameraTarget.transform.rotation = Quaternion.Lerp(_cinemachineCameraTarget.transform.rotation, targetRotation, _balanceRotationZLerp);
        }

        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }


    }
}