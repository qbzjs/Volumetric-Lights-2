using System;
using Kayak;
using UnityEngine;

namespace Character.State
{
    public class CharacterManager : MonoBehaviour
    {

        [Header("References"), SerializeField] private KayakController _kayakController;
        public CharacterStateBase CurrentStateBase;
        [SerializeField] private InputManagement _inputManagement;

        [Header("Balance"), SerializeField, Range(0, 1)] private float balanceLerpTo0Value = 0.01f;
        [ReadOnly] public bool LerpBalanceTo0 = true;
        [ReadOnly] public float Balance = 0f;
        [Range(0,10), ReadOnly] public float BalanceLimit = 10f;
        [Range(0,100), ReadOnly] public float BalanceValueToTimerMultiplier = 10f;

        
        private void Awake()
        {
            CharacterNavigationState navigationState = new CharacterNavigationState(_kayakController, _inputManagement, this);
            CurrentStateBase = navigationState;
        }

        private void Start()
        {
            CurrentStateBase.EnterState(this);
        }
        private void Update()
        {
            CurrentStateBase.UpdateState(this);
            
            BalanceManagement();
        }
        private void FixedUpdate()
        {
            CurrentStateBase.FixedUpdate(this);
        }
        public void SwitchState(CharacterStateBase stateBaseCharacter)
        {
            CurrentStateBase = stateBaseCharacter;
            stateBaseCharacter.EnterState(this);
        }

        private void BalanceManagement()
        {
            if (LerpBalanceTo0)
            {
                Balance = Mathf.Lerp(Balance, 0, balanceLerpTo0Value);
            }
            
            if (Mathf.Abs(Balance) >= BalanceLimit)
            {
                CharacterUnbalancedState characterUnbalancedState = new CharacterUnbalancedState(_kayakController, _inputManagement, this);
                SwitchState(characterUnbalancedState);
            }
        }

        #region GUI

        private void OnGUI()
        {
            GUI.skin.label.fontSize = 50;
            GUI.Label(new Rect(10, 10, 500, 100), "Balance : " + Math.Round(Balance,2));
        }

        #endregion
    }
}
