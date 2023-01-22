using UnityEngine;

public abstract class CharacterBaseState 
{


    public bool CanCharacterMove = true;
    public bool CanCharacterMakeActions = true;

    public Transform PosPlayer;

    public abstract void EnterState(CharacterStateManager character);

    public abstract void UpdateState(CharacterStateManager character);

    public abstract void FixedUpdate(CharacterStateManager character);

    public abstract void SwitchState(CharacterStateManager character);

}