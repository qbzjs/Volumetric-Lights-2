using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Character.State
{
    public abstract class CharacterStateBase 
    {
        public CharacterStateBase(CharacterManager characterManagerRef, MonoBehaviour monoBehaviour, CameraManager cameraManagerRef)
        {
            CharacterManagerRef = characterManagerRef;
            MonoBehaviourRef = monoBehaviour;
            CameraManagerRef = cameraManagerRef;
        }

        protected CharacterManager CharacterManagerRef;
        protected CameraManager CameraManagerRef;
        protected MonoBehaviour MonoBehaviourRef;
        
        public bool CanBeMoved = true;
        public bool CanCharacterMove = true;
        public bool CanCharacterMakeActions = true;
        
        public float RotationStaticForceY = 0f;
        public float RotationPaddleForceY = 0f;
        
        //events
        public UnityEvent OnPaddleLeft = new UnityEvent();
        public UnityEvent OnPaddleRight = new UnityEvent();
        
        protected Transform PlayerPosition;

        public abstract void EnterState(CharacterManager character);

        public abstract void UpdateState(CharacterManager character);

        public abstract void FixedUpdate(CharacterManager character);

        public abstract void SwitchState(CharacterManager character);

        /// <summary>
        /// Rotate the boat alongside the Balance value
        /// </summary>
        protected void MakeBoatRotationWithBalance(Transform kayakTransform, float multiplier)
        {
            Vector3 boatRotation = kayakTransform.localRotation.eulerAngles;
            Quaternion targetBoatRotation = Quaternion.Euler(boatRotation.x,boatRotation.y, CharacterManagerRef.Balance * 3 * multiplier);
            kayakTransform.localRotation = Quaternion.Lerp(kayakTransform.localRotation, targetBoatRotation, 0.025f);
        }

        /// <summary>
        /// Move the velocity toward the player's facing direction
        /// </summary>
        protected void VelocityToward()
        {
            Vector3 oldVelocity = CharacterManagerRef.KayakController.Rigidbody.velocity;
            float oldVelocityMagnitude = new Vector2(oldVelocity.x, oldVelocity.z).magnitude;
            Vector3 forward = CharacterManagerRef.KayakController.transform.forward;
            
            Vector2 newVelocity = oldVelocityMagnitude * new Vector2(forward.x,forward.z).normalized;

            CharacterManagerRef.KayakController.Rigidbody.velocity = new Vector3(newVelocity.x, oldVelocity.y, newVelocity.y);
        }
    }
}