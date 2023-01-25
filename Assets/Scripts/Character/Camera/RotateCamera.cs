using Character.State;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Camera
{
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

        //serialize fields
        [Header("Cinemachine"),Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow"), SerializeField]
        private GameObject _cinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up"), SerializeField]
        private float _topClamp = 70.0f;
        [Tooltip("How far in degrees can you move the camera down"), SerializeField]
        private float _bottomClamp = -30.0f;
        [Tooltip("Additional degrees to override the camera. Useful for fine tuning camera position when locked"), SerializeField]
        private float _cameraAngleOverride = 0.0f;
        [Tooltip("Locking the camera position on all axis"), SerializeField] 
        private bool _lockCameraPosition;
        
        [Header("References"), SerializeField] 
        private Rigidbody _rigidbodyKayak;
        [SerializeField] 
        private CharacterStateManager _characterStateManager;
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
            if (Input.GetMouseButton(0))
            {
                _cinemachineTargetYaw += _input.Inputs.RotateCamera.x;
                _cinemachineTargetPitch += _input.Inputs.RotateCamera.y;
            }
            else if (Mathf.Abs(_rigidbodyKayak.velocity.x + _rigidbodyKayak.velocity.z) > 0.2 || Mathf.Abs(_characterStateManager.CurrentStateBase.RotationStaticForceY) > 0.01)
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

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);

            _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + _cameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);


        }



        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

 
    }
}
