using Character.Camera;
using Kayak;
using UnityEngine;

namespace Character.State
{
    public class CharacterUnbalancedState : CharacterStateBase
    {
        #region Variables

        private KayakController _kayakController;
        private InputManagement _inputs;
        private GameplayInputs _gameplayInputs;
        private KayakParameters _kayakValues;

        private float _rightPaddleCooldown, _leftPaddleCooldown;

        #endregion

        #region Constructor

        public CharacterUnbalancedState(KayakController kayak, InputManagement inputManagement, CharacterManager characterManagerRef, MonoBehaviour monoBehaviour, CameraManager cameraManager) :
            base(characterManagerRef, monoBehaviour, cameraManager)
        {
            _kayakController = kayak;
            _inputs = inputManagement;
            _kayakValues = kayak.KayakValues;
        }

        #endregion

        #region Override Functions

        public override void EnterState(CharacterManager character)
        {
            Debug.Log("unbalanced");
            CharacterManagerRef.LerpBalanceTo0 = false;

            //values
            _rightPaddleCooldown = _kayakValues.UnbalancePaddleCooldown;
            _leftPaddleCooldown = _kayakValues.UnbalancePaddleCooldown;
            CanBeMoved = false;

            //balance
            if (Mathf.Abs(CharacterManagerRef.Balance) >
                CharacterManagerRef.BalanceDeathLimit - CharacterManagerRef.MinimumTimeUnbalanced)
            {
                CharacterManagerRef.Balance = (CharacterManagerRef.BalanceDeathLimit - CharacterManagerRef.MinimumTimeUnbalanced) *
                                              Mathf.Sign(CharacterManagerRef.Balance);
            }
        }

        public override void UpdateState(CharacterManager character)
        {
            TimerManagement();
            PaddleCooldownManagement();

            MakeBoatRotationWithBalance(_kayakController.transform, 2);

            Rebalance();


            if (Mathf.Abs(CharacterManagerRef.Balance) < CharacterManagerRef.RebalanceAngle)
                this.SwitchState(character);
        }

        public override void FixedUpdate(CharacterManager character)
        {
        }

        public override void SwitchState(CharacterManager character)
        {

        }

        #endregion

        #region Methods

        private void TimerManagement()
        {
            CharacterManagerRef.Balance += Time.deltaTime * Mathf.Sign(CharacterManagerRef.Balance);
            if (Mathf.Abs(CharacterManagerRef.Balance) >= CharacterManagerRef.BalanceDeathLimit)
            {
                CharacterDeathState characterDeathState = new CharacterDeathState(CharacterManagerRef, _kayakController, _inputs, MonoBehaviourRef, CameraManagerRef);
                CharacterManagerRef.SwitchState(characterDeathState);
            }
            else if (Mathf.Abs(CharacterManagerRef.Balance) < CharacterManagerRef.RebalanceAngle)
            {
                _kayakController.CanReduceDrag = true;
                CameraManagerRef.CanMoveCameraManually = true;
                CharacterManagerRef.Balance = 0;


                CharacterNavigationState characterNavigationState = new CharacterNavigationState(_kayakController, _inputs, CharacterManagerRef, MonoBehaviourRef, CameraManagerRef);
                CharacterManagerRef.SwitchState(characterNavigationState);

                CameraNavigationState cameraNavigationState = new CameraNavigationState(CameraManagerRef, MonoBehaviourRef);
                CameraManagerRef.SwitchState(cameraNavigationState);
            }
        }

        private void PaddleCooldownManagement()
        {
            _leftPaddleCooldown -= Time.deltaTime;
            _rightPaddleCooldown -= Time.deltaTime;
        }

        private void Rebalance()
        {
            if (_inputs.Inputs.PaddleLeft && _leftPaddleCooldown <= 0 && CharacterManagerRef.Balance < 0)
            {
                CharacterManagerRef.Balance += _kayakValues.UnbalancePaddleForce;
                _leftPaddleCooldown = _kayakValues.UnbalancePaddleCooldown;

                //audio
                SoundManager.Instance.PlaySound(_kayakController.PaddlingAudioClip);
            }
            if (_inputs.Inputs.PaddleRight && _rightPaddleCooldown <= 0 && CharacterManagerRef.Balance > 0)
            {
                CharacterManagerRef.Balance -= _kayakValues.UnbalancePaddleForce;
                _rightPaddleCooldown = _kayakValues.UnbalancePaddleCooldown;

                //audio
                SoundManager.Instance.PlaySound(_kayakController.PaddlingAudioClip);
            }
        }

        #endregion
    }
}