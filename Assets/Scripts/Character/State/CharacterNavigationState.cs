using System;
using DG.Tweening;
using Kayak;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Character.State
{
    public class CharacterNavigationState : CharacterStateBase
    {
        //enum
        private enum Direction
        {
            Left = 0,
            Right = 1
        }
        
        //inputs
        private Inputs _inputs;

        //kayak
        private Vector2 _paddleForceValue;
        private float _leftPaddleCooldown, _rightPaddleCooldown;
        private float _currentInputPaddleFrontForce = 30;
        private float _rotationForceY = 0f; // the rotation 

        //reference
        private KayakController _kayakController;
        private KayakParameters _kayakValues;
        private Rigidbody _kayakRigidbody;
        
        #region Constructor

        public CharacterNavigationState(KayakController kayak)
        {
            _kayakController = kayak;
            _kayakRigidbody = kayak.Rigidbody;
            _kayakValues = kayak.KayakValues;

            _inputs.DEADZONE = 0.3f;
        }

        #endregion

        #region CharacterBaseState overrided function

        public override void EnterState(CharacterStateManager character)
        {
            //inputs
            MonoBehaviour.print("enter navigation state");
            GameplayInputs = new GameplayInputs();
            GameplayInputs.Enable();
            
            //values
            _rightPaddleCooldown = _kayakValues.PaddleCooldown;
            _leftPaddleCooldown = _kayakValues.PaddleCooldown;
        }

        public override void UpdateState(CharacterStateManager character)
        {
            GatherInputs();
            PaddleCooldownManagement();
            HandlePaddleMovement();
            KayakRotationManager();
            HandleStaticRotation();
        }

        public override void FixedUpdate(CharacterStateManager character)
        {
        }

        public override void SwitchState(CharacterStateManager character)
        {

        }
        
        #endregion

        #region Methods

        private void GatherInputs()
        {
            _inputs.PaddleLeft = GameplayInputs.Boat.PaddleLeft.triggered;       
            _inputs.PaddleRight = GameplayInputs.Boat.PaddleRight.triggered;

            _inputs.RotateLeft = GameplayInputs.Boat.StaticRotateLeft.ReadValue<float>();       
            _inputs.RotateRight = GameplayInputs.Boat.StaticRotateRight.ReadValue<float>();       
        }

        private void KayakRotationManager()
        {
            if (Mathf.Abs(_rotationForceY) > 0.01f)
            {
                _rotationForceY = Mathf.Lerp(_rotationForceY, 0, _kayakValues.RotationDeceleration);
            }
            Transform kayakTransform = _kayakController.transform;
            kayakTransform.Rotate(Vector3.up, _rotationForceY);
        }

        #endregion
        
        #region Paddle Movement

        private void Paddle(Direction direction)
        {
            //apply force
            Vector3 forceToApply = _kayakController.transform.forward * _kayakValues.PaddleFrontForce;
            _kayakRigidbody.AddForce(forceToApply);

            //rotation
            float rotation = _kayakValues.PaddleSideRotationForce;
            _rotationForceY += direction == Direction.Right ? -rotation : rotation;
        }

        private void HandlePaddleMovement()
        {
            //input -> paddleMovement
            if (_inputs.PaddleLeft && _rightPaddleCooldown <= 0)
            {
                _rightPaddleCooldown = _kayakValues.PaddleCooldown;
                Paddle(Direction.Left);
            }
            if (_inputs.PaddleRight && _leftPaddleCooldown <= 0)
            {
                _leftPaddleCooldown = _kayakValues.PaddleCooldown;
                Paddle(Direction.Right);
            }
        }

        private void PaddleCooldownManagement()
        {
            _leftPaddleCooldown -= Time.deltaTime;
            _rightPaddleCooldown -= Time.deltaTime;
        }

        #endregion
        
        #region Rotate Movement
        
        private void HandleStaticRotation()
        {
            if (_inputs.RotateLeft > _inputs.DEADZONE)
            {
                _rotationForceY -= _kayakValues.StaticRotationForce;
            }
            if (_inputs.RotateRight > _inputs.DEADZONE)
            {
                _rotationForceY += _kayakValues.StaticRotationForce;
            }
        }
        
        #endregion
    }
}




