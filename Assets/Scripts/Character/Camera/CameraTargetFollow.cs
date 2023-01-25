using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Camera
{
    public class CameraTargetFollow : MonoBehaviour
    {
        [SerializeField] private Transform _kayakTransform;
        
        private Vector3 _localPositionBase;

        private void Start()
        {
            _localPositionBase = transform.localPosition;
        }

        private void Update()
        {
            //position
            transform.position = _kayakTransform.position + _localPositionBase;
            
            //rotation
            Vector3 rotation = transform.rotation.eulerAngles;
            Vector3 kayakRotation = _kayakTransform.eulerAngles;
            transform.rotation = Quaternion.Euler(new Vector3(rotation.x, kayakRotation.y, rotation.z));
        }
    }
}
