using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{
    CharacterBaseState _currentState;
    CharacterNavigationState NavigationState = new CharacterNavigationState();

    private void Start()
    {
        _currentState = NavigationState;

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
