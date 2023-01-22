using System;
using Kayak;
using UnityEngine;

namespace Character.State
{
    public class CharacterStateManager : MonoBehaviour
    {

        [Header("References"), SerializeField] private KayakController _kayakController;
        
        private CharacterState _currentState;
        private CharacterNavigationState _navigationState;

        public CharacterStateManager()
        {
            _navigationState = new CharacterNavigationState(_kayakController);
        }

        private void Start()
        {
            _currentState = _navigationState;

            _currentState.EnterState(this);
        }
        private void Update()
        {
            _currentState.UpdateState(this);
        }
        private void FixedUpdate()
        {
            _currentState.FixedUpdate(this);
        }
        public void SwitchState(CharacterState stateCharacter)
        {
            _currentState = stateCharacter;
            stateCharacter.EnterState(this);
        }
    }
}
