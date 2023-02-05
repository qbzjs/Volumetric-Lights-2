using Kayak;
using UnityEngine;

namespace Character.State
{
    public class CharacterUnbalancedState : CharacterStateBase
    {
        #region Constructor

        public CharacterUnbalancedState(KayakController kayak, InputManagement inputManagement, CharacterManager characterManagerRef) : base(characterManagerRef)
        {
            _kayakController = kayak;
            _inputManagement = inputManagement;
        }

        #endregion
        
        #region Variables

        private KayakController _kayakController;
        private InputManagement _inputManagement;
        
        #endregion
        
        
        #region Override Functions
        
        public override void EnterState(CharacterManager character)
        {
            Debug.Log("unbalanced");
            CharacterManagerRef.LerpBalanceTo0 = false;
        }

        public override void UpdateState(CharacterManager character)
        {
            TimerManagement();
            MakeBoatRotationWithBalance(_kayakController.Mesh);
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
                CharacterDeathState characterDeathState = new CharacterDeathState(CharacterManagerRef);
                CharacterManagerRef.SwitchState(characterDeathState);
            }
            else if(Mathf.Abs(CharacterManagerRef.Balance) < CharacterManagerRef.BalanceLimit)
            {
                // CharacterNavigationState characterNavigationState = new CharacterNavigationState(_kayakController, _inputManagement, CharacterManagerRef);
                // CharacterManagerRef.SwitchState(characterNavigationState);

                CharacterManagerRef.CamController.CanMoveCameraMaunally = true;
            }
        }

        #endregion
    }
}