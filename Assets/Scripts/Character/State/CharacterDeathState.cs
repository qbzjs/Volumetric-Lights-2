using Kayak;
using UnityEngine;

namespace Character.State
{
    public class CharacterDeathState : CharacterStateBase
    {
        private KayakController _kayakController;
        private InputManagement _inputManagement;

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
            CharacterManagerRef.TransitionManager.LaunchTransitionIn(SceneTransition.TransitionType.Fade);
        }

        public override void UpdateState(CharacterManager character)
        {
            Transform checkpoint = CheckpointManager.Instance.CurrentCheckpoint.TargetRespawnTransform;
            MakeBoatRotationWithBalance(_kayakController.Mesh);
            if (Input.GetKeyDown(KeyCode.K))
            {
                _kayakController.transform.position = checkpoint.position;
                _kayakController.transform.rotation = checkpoint.rotation;

                _kayakController.CanReduceDrag = true;
                CharacterManagerRef.CamController.CanMoveCameraMaunally = true;
                CharacterManagerRef.Balance = 0;
                this.SwitchState(character);
            }
        }

        public override void FixedUpdate(CharacterManager character)
        {
        }

        public override void SwitchState(CharacterManager character)
        {
            CharacterManagerRef.TransitionManager.LaunchTransitionOut(SceneTransition.TransitionType.Fade);
            CharacterNavigationState characterNavigationState = new CharacterNavigationState(_kayakController, _inputManagement, CharacterManagerRef);
            CharacterManagerRef.SwitchState(characterNavigationState);
        }
    }
}