using Kayak;
using UnityEngine;

namespace Character.State
{
    public class CharacterDeathState : CharacterStateBase
    {
        private KayakController _kayakController;
        
        public CharacterDeathState(CharacterManager characterManagerRef, KayakController kayakController) : base(characterManagerRef)
        {
            _kayakController = kayakController;
        }

        public override void EnterState(CharacterManager character)
        {
            Debug.Log("death");
        }

        public override void UpdateState(CharacterManager character)
        {
            MakeBoatRotationWithBalance(_kayakController.Mesh);
        }

        public override void FixedUpdate(CharacterManager character)
        {
        }

        public override void SwitchState(CharacterManager character)
        {
        }
    }
}