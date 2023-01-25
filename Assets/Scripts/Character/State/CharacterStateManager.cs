using System;
using Kayak;
using UnityEngine;

namespace Character.State
{
    public class CharacterStateManager : MonoBehaviour
    {

        [Header("References"), SerializeField] private KayakController _kayakController;
        public CharacterStateBase CurrentStateBase;
        private CharacterNavigationState _navigationState;

        private void Awake()
        {
            _navigationState = new CharacterNavigationState(_kayakController);
            CurrentStateBase = _navigationState;
        }

        private void Start()
        {
            CurrentStateBase.EnterState(this);
        }
        private void Update()
        {
            CurrentStateBase.UpdateState(this);
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
    }
}
