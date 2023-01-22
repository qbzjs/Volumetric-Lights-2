using UnityEngine;

namespace Character
{
    public class CharacterInBoatTransformManager : MonoBehaviour
    {
        public Transform KayakPosition;

        private void Update()
        {
            MatchCharacterWithBoat();
        }

        private void MatchCharacterWithBoat()
        {
            Vector3 position = KayakPosition.position;
            transform.position = position;
        
            Vector3 rotation = transform.rotation.eulerAngles;
            Vector3 boatRotation = KayakPosition.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(rotation.x, boatRotation.y + 180, -boatRotation.z + 180);
        }
    }
}
