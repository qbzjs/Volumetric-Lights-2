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

        [Header("Values")] [SerializeField, Range(0, 1)]
        private float balanceLerpTo0Value = 0.01f;

        private CharacterNavigationState _navigationState;

        private void Awake()
        {
            _navigationState = new CharacterNavigationState(_kayakController, _inputManagement);
            CurrentStateBase = _navigationState;
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
            //lerp to 0
            CurrentStateBase.Balance = Mathf.Lerp(CurrentStateBase.Balance, 0, balanceLerpTo0Value);
        }

        #region GUI

        private void OnGUI()
        {
            GUI.skin.label.fontSize = 50;
            GUI.Label(new Rect(10, 10, 300, 100), CurrentStateBase.Balance.ToString());
        }

        #endregion
    }
}
