using Character.State;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Camera
{
    public class CameraRotationController : MonoBehaviour
    {
        #region Singleton
        public static CameraRotationController Instance { get; private set; }
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
        [Header("Cinemachine"),Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow"), SerializeField]
        private GameObject _cinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up"), SerializeField]
        private float _topClamp = 70.0f;
        [Tooltip("How far in degrees can you move the camera down"), SerializeField]
        private float _bottomClamp = -30.0f;
        [Tooltip("Additional degrees to override the camera. Useful for fine tuning camera position when locked"), SerializeField]
        private float _cameraAngleOverride = 0.0f;

        [Header("References"), SerializeField] 
        private Rigidbody _rigidbodyKayak;
        [FormerlySerializedAs("_characterStateManager")] [SerializeField] 
        private CharacterManager characterManager;
        [SerializeField] 
        private InputManagement _input;
        
        //private values
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

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
            if (Input.GetMouseButton(0))
            {
                _cinemachineTargetYaw += _input.Inputs.RotateCamera.x;
                _cinemachineTargetPitch += _input.Inputs.RotateCamera.y;
            }
            //manage rotate to stay behind boat
            else if (Mathf.Abs(_rigidbodyKayak.velocity.x + _rigidbodyKayak.velocity.z) > 0.2 || Mathf.Abs(characterManager.CurrentStateBase.RotationStaticForceY) > 0.01)
            {
                Quaternion rotation = _cinemachineCameraTarget.transform.localRotation;

                _cinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(rotation, Quaternion.Euler(new Vector3(0, 0, rotation.z)), Time.deltaTime * 2);
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

            //Clamp
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);

            //y camera rotation
            const float rotationMultiplier = 4f;
            float cameraRotationZ = characterManager.CurrentStateBase.Balance * rotationMultiplier;

            //apply camera rotation
            const float lerpValue = 0.01f;
            Quaternion targetRotation = Quaternion.Euler(_cinemachineTargetPitch + _cameraAngleOverride, _cinemachineTargetYaw, cameraRotationZ);
            _cinemachineCameraTarget.transform.rotation = Quaternion.Lerp(_cinemachineCameraTarget.transform.rotation, targetRotation, lerpValue);
        }
        
        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

 
    }
}
