using UnityEngine;

namespace Character.State
{
    public abstract class CharacterStateBase 
    {
        public GameplayInputs GameplayInputs;
        public bool CanCharacterMove = true;
        public bool CanCharacterMakeActions = true;

        protected Transform PlayerPosition;

        public abstract void EnterState(CharacterStateManager character);

        public abstract void UpdateState(CharacterStateManager character);

        public abstract void FixedUpdate(CharacterStateManager character);

        public abstract void SwitchState(CharacterStateManager character);

    }
}