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
        [Header("Cinemachine"),Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degrees to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        [Header("References"), SerializeField] private Rigidbody _rigidbodyKayak;
        [SerializeField] private CharacterStateManager _characterStateManager;
        
        //private values
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        }

        private void Update()
        {
            CameraRotation();
        }

        private void CameraRotation()
        {
            if (Input.GetMouseButton(0))
            {
                _cinemachineTargetYaw += Input.GetAxis("Mouse X") * 1;
                _cinemachineTargetPitch += Input.GetAxis("Mouse Y") * 1;
            }
            else if (Mathf.Abs(_rigidbodyKayak.velocity.x + _rigidbodyKayak.velocity.z) > 0.2 || Mathf.Abs(_characterStateManager.CurrentStateBase.RotationStaticForceY) > 0.01)
            {
                Quaternion rotation = CinemachineCameraTarget.transform.localRotation;

                CinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(rotation, Quaternion.Euler(new Vector3(0, 0, rotation.z)), Time.deltaTime * 2);
                _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

                if (CinemachineCameraTarget.transform.rotation.eulerAngles.x > 180)
                {
                    _cinemachineTargetPitch = CinemachineCameraTarget.transform.rotation.eulerAngles.x - 360;
                }
                else
                {
                    _cinemachineTargetPitch = CinemachineCameraTarget.transform.rotation.eulerAngles.x;
                }
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

 
    }
}
