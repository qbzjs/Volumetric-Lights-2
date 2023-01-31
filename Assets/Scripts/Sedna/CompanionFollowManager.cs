using UnityEngine;

namespace Sedna
{
    public class CompanionFollowManager : MonoBehaviour
    {
        [SerializeField] Transform _sednaTargetPosition;
        [SerializeField] Transform _kayakTransform;

        private Rigidbody _rigidbody;
        private Vector3 _velocity;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            HandlePosition();
            HandleRotation();
        }
        
        private void HandlePosition()
        {
            Vector3 current = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 target = new Vector3(_sednaTargetPosition.transform.position.x, 0, _sednaTargetPosition.transform.position.z);
            Vector3 positionToRefVelocity = Vector3.SmoothDamp(current, target, ref _velocity, 0.5f);

            //apply velocity
            Vector3 velocity = new Vector3(_velocity.x, _rigidbody.velocity.y, _velocity.z);
            _rigidbody.velocity = velocity;
        }
        private void HandleRotation()
        {
            Quaternion kayakRotation = Quaternion.Euler(new Vector3(0, _kayakTransform.rotation.eulerAngles.y, 0));
            Quaternion targetRotation = Quaternion.Slerp(_kayakTransform.rotation, kayakRotation, 0.5f);

            _rigidbody.MoveRotation(targetRotation);
        }
    }
}
