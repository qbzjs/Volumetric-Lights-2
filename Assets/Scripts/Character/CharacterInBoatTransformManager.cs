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
            //position
            transform.position = KayakTransform.position + _playerPosition;
            
            //rotation
            Vector3 boatRotation = KayakTransform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(boatRotation.x,boatRotation.y,boatRotation.z);
        }
    }
}
