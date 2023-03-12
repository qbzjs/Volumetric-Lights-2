using Character.Camera;
using Kayak;
using UnityEngine;

namespace Character.State
{
    public class CharacterDeathState : CharacterStateBase
    {
        private KayakController _kayakController;
        private InputManagement _inputs;

        private bool _transitionIn = false;
        private bool _respawned = false;
        private bool _cameraSwitchState = false;
        private float _timerToRespawnCheckpoint = 0;
        private float _timerFadeOutStart = 0;
            
        public CharacterDeathState(CharacterManager characterManagerRef, KayakController kayakController, InputManagement inputs, MonoBehaviour monoBehaviour, CameraManager cameraManagerRef) :
            base(characterManagerRef, monoBehaviour, cameraManagerRef)
        {
            _kayakController = kayakController;
            _inputs = inputs;
        }

        public override void EnterState(CharacterManager character)
        {
            Debug.Log("death");
        }

        public override void UpdateState(CharacterManager character)
        {
            Transform checkpoint = CheckpointManager.Instance.CurrentCheckpoint.TargetRespawnTransform;

            //Rotate kayak at 180 in z with balance
            if (CharacterManagerRef.Balance > 0 && CharacterManagerRef.Balance < 60)
            {
                CharacterManagerRef.Balance += 0.5f;
            }
            else if (CharacterManagerRef.Balance < 0 && CharacterManagerRef.Balance > -60)
            {
                CharacterManagerRef.Balance -= 0.5f;
            }

            //Switch camera
            if (Mathf.Abs(CharacterManagerRef.Balance) > 60 && _cameraSwitchState == false)
            {
                _cameraSwitchState = true;
                CameraDeathState cameraDeathState = new CameraDeathState(CameraManagerRef, MonoBehaviourRef);
                CameraManagerRef.SwitchState(cameraDeathState);
            }
            MakeBoatRotationWithBalance(_kayakController.transform, 1);

            //Transition In
            if (CameraManagerRef.StartDeath == true && _transitionIn == false)
            {
                CharacterManagerRef.TransitionManager.LaunchTransitionIn(SceneTransition.TransitionType.Fade);
                _transitionIn = true;
            }

            //Timer transition In
            if (_transitionIn == true && _respawned == false)
            {
                _timerToRespawnCheckpoint += Time.deltaTime;
            }

            if (_timerToRespawnCheckpoint >= 1.5f)
            {
                RespawnCheckpoint(checkpoint);
            }

            if (_timerFadeOutStart > 1.5f && _respawned == true)
                this.SwitchState(character);

        }

        public override void FixedUpdate(CharacterManager character)
        {
        }

        public override void SwitchState(CharacterManager character)
        {
            //Transition out
            CharacterManagerRef.TransitionManager.LaunchTransitionOut(SceneTransition.TransitionType.Fade);

            //Switch state character
            CharacterNavigationState characterNavigationState = new CharacterNavigationState(_kayakController, _inputs, CharacterManagerRef, MonoBehaviourRef, CameraManagerRef);
            CharacterManagerRef.SwitchState(characterNavigationState);

        }

        private void RespawnCheckpoint(Transform checkpoint)
        {
            if (_respawned == true)
            {
                _timerFadeOutStart += Time.deltaTime;
            }
            else
            {
                //put kayak in checkpoint position & rotation
                _kayakController.transform.position = checkpoint.position;
                _kayakController.transform.rotation = checkpoint.rotation;

                //Reset value
                _kayakController.CanReduceDrag = true;
                CameraManagerRef.CanMoveCameraManually = true;
                CharacterManagerRef.SetBalanceValueToCurrentSide(0);
                _respawned = true;

                //Switch state camera
                CameraNavigationState cameraNavigationState = new CameraNavigationState(CameraManagerRef, MonoBehaviourRef);
                CameraManagerRef.SwitchState(cameraNavigationState);
            }
        }
    }


}