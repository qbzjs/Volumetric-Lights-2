using UnityEngine;

namespace Character
{
    public class CharacterInBoatTransformManager : MonoBehaviour
    {
        public Transform KayakTransform;

        private Vector3 _playerLocalPosition;

        private void Start()
        {
            _playerLocalPosition = transform.localPosition;
        }

        private void Update()
        {
            MatchCharacterWithBoat();
        }

        private void MatchCharacterWithBoat()
        {
            transform.position = KayakTransform.position + _playerLocalPosition;
        
            Vector3 rotation = transform.rotation.eulerAngles;
            Vector3 boatRotation = KayakTransform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(rotation.x, boatRotation.y + 180, -boatRotation.z + 180);
        }
    }
}
