using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class CharacterInBoatTransformManager : MonoBehaviour
    {
        public Transform KayakTransform;

        private Vector3 _playerPosition;

        private void Start()
        {
            _playerPosition = transform.localPosition;
        }

        private void Update()
        {
            MatchCharacterWithBoat();
        }

        private void MatchCharacterWithBoat()
        {
            transform.position = KayakTransform.position + _playerPosition;
        
            Vector3 rotation = transform.rotation.eulerAngles;
            Vector3 boatRotation = KayakTransform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(rotation.x, boatRotation.y + 180, -boatRotation.z + 180);
        }
    }
}
