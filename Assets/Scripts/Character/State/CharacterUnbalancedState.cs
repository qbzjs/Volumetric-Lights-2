using Kayak;
using UnityEngine;

namespace Character.State
{
    public class CharacterUnbalancedState : CharacterStateBase
    {
        #region Constructor

        public CharacterUnbalancedState(KayakController kayak, InputManagement inputManagement, CharacterManager characterManagerRef) : base(characterManagerRef)
        {
            
        }

        #endregion
        
        #region Variables

        [SerializeField, ReadOnly, Tooltip("The amplitude of the unbalance, the higher it is, the less time player has to react")]
        private float _unbalanceAmount;
        [SerializeField, ReadOnly, Tooltip("The amplitude of the unbalance, the higher it is, the less time player has to react")]
        private float _deathTimer;

        private KayakController _kayakController;
        private InputManagement _inputManagement;
        
        #endregion
        
        
        #region Override Functions
        
        public override void EnterState(CharacterManager character)
        {
            //booleans
            CharacterManagerRef.LerpBalanceTo0 = false;
            
            //methods
            CalculateUnbalance();
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
        
        #endregion

        #region Methods

        private void CalculateUnbalance()
        {
            _unbalanceAmount = CharacterManagerRef.Balance - CharacterManagerRef.BalanceLimit;
            _deathTimer = _unbalanceAmount * CharacterManagerRef.BalanceValueToTimerMultiplier;
            Debug.Log($"{_unbalanceAmount} -> {_deathTimer}");
        }

        #endregion
    }
}