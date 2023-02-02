using UnityEngine;

namespace Character.State
{
    public abstract class CharacterStateBase 
    {
        public CharacterStateBase(CharacterManager characterManagerRef)
        {
            CharacterManagerRef = characterManagerRef;
        }
        
        public GameplayInputs GameplayInputs;
        public CharacterManager CharacterManagerRef;
        
        public bool CanCharacterMove = true;
        public bool CanCharacterMakeActions = true;
        public float RotationStaticForceY = 0f;
        public float RotationPaddleForceY = 0f;
        
        protected Transform PlayerPosition;

        public abstract void EnterState(CharacterManager character);

        public abstract void UpdateState(CharacterManager character);

        public abstract void FixedUpdate(CharacterManager character);

        public abstract void SwitchState(CharacterManager character);

    }
}