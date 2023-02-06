using Kayak;
using SceneTransition;
using UnityEngine;

namespace Character.State
{
    public class CharacterDeathState : CharacterStateBase
    {
        private KayakController _kayakController;
        private InputManagement _inputManagement;
        private TransitionType _transitionType;

        public CharacterDeathState(CharacterManager characterManagerRef, KayakController kayakController) : base(characterManagerRef)
        {
            _kayakController = kayakController;
        }
        public CharacterDeathState(CharacterManager characterManagerRef, InputManagement inputManagement, KayakController kayakController) : base(characterManagerRef)
        {
            _inputManagement = inputManagement;
            _kayakController = kayakController;
        }




        public override void EnterState(CharacterManager character)
        {
            Debug.Log("death");
        }

        public override void UpdateState(CharacterManager character)
        {
            Transform checkpoint = CheckpointManager.Instance.CurrentChekpoint.TargetRespawnTransform;
            MakeBoatRotationWithBalance(_kayakController.Mesh);
            if (Input.GetKeyDown(KeyCode.K))
            {
                //TransitionManager.LaunchTransitionIn(_transitionType);
                _kayakController.transform.position = checkpoint.position;
                _kayakController.transform.rotation = checkpoint.rotation;

                CharacterNavigationState characterUnbalancedState = new CharacterNavigationState(_kayakController, _inputManagement, CharacterManagerRef);
                CharacterManagerRef.SwitchState(characterUnbalancedState);
            }
        }

        public override void FixedUpdate(CharacterManager character)
        {
        }

        public override void SwitchState(CharacterManager character)
        {
        }
    }
}