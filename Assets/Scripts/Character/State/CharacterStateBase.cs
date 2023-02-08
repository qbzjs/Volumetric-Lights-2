using DG.Tweening;
using UnityEngine;

namespace Character.State
{
    public abstract class CharacterStateBase 
    {
        public CharacterStateBase(CharacterManager characterManagerRef, MonoBehaviour monoBehaviour)
        {
            CharacterManagerRef = characterManagerRef;
            MonoBehaviourRef = monoBehaviour;
        }

        protected CharacterManager CharacterManagerRef;
        protected MonoBehaviour MonoBehaviourRef;
        
        public bool CanCharacterMove = true;
        public bool CanCharacterMakeActions = true;
        public float RotationStaticForceY = 0f;
        public float RotationPaddleForceY = 0f;
        
        protected Transform PlayerPosition;

        public abstract void EnterState(CharacterManager character);

        public abstract void UpdateState(CharacterManager character);

        public abstract void FixedUpdate(CharacterManager character);

        public abstract void SwitchState(CharacterManager character);

        protected void MakeBoatRotationWithBalance(Transform kayakTransform, float multiplier)
        {
            Vector3 boatRotation = kayakTransform.localRotation.eulerAngles;
            Quaternion targetBoatRotation = Quaternion.Euler(boatRotation.x,boatRotation.y, CharacterManagerRef.Balance * 3 * multiplier);
            kayakTransform.localRotation = Quaternion.Lerp(kayakTransform.localRotation, targetBoatRotation, 0.025f);
        }
    }
}