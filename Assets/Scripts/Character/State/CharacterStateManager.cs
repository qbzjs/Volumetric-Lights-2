using UnityEngine;

namespace Character.State
{
    public class CharacterStateManager : MonoBehaviour
    {
        private CharacterBaseState _currentState;
        private CharacterNavigationState _navigationState = new CharacterNavigationState();

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
        public void SwitchState(CharacterBaseState stateCharacter)
        {
            _currentState = stateCharacter;
            stateCharacter.EnterState(this);
        }
    }
}
