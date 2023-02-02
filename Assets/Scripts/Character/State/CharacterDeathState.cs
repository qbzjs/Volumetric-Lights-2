using UnityEngine;

namespace Character.State
{
    public class CharacterDeathState : CharacterStateBase
    {
        public CharacterDeathState(CharacterManager characterManagerRef) : base(characterManagerRef)
        {
            
        }

        public override void EnterState(CharacterManager character)
        {
            Debug.Log("death");
        }

        public override void UpdateState(CharacterManager character)
        {
        }

        public override void FixedUpdate(CharacterManager character)
        {
        }

        public override void SwitchState(CharacterManager character)
        {
        }
    }
}