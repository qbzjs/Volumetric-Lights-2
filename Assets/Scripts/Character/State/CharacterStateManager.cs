using System;
using Kayak;
using UnityEngine;

namespace Character.State
{
    public class CharacterStateManager : MonoBehaviour
    {

        [Header("References"), SerializeField] private KayakController _kayakController;
        
        private CharacterStateBase _currentStateBase;
        private CharacterNavigationState _navigationState;

        private void Awake()
        {
            _navigationState = new CharacterNavigationState(_kayakController);
            _currentStateBase = _navigationState;
        }

        private void Start()
        {
            _currentStateBase.EnterState(this);
        }
        private void Update()
        {
            _currentStateBase.UpdateState(this);
        }
        private void FixedUpdate()
        {
            _currentStateBase.FixedUpdate(this);
        }
        public void SwitchState(CharacterStateBase stateBaseCharacter)
        {
            _currentStateBase = stateBaseCharacter;
            stateBaseCharacter.EnterState(this);
        }
    }
}
