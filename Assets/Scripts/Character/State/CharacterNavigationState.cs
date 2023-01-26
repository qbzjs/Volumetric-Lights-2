using Kayak;
using UnityEngine;

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

        private enum RotationType
        {
            Static = 0,
            Paddle = 1
        }
        
        //inputs
        private InputManagement _inputs;

        //kayak
        private Vector2 _paddleForceValue;
        private float _leftPaddleCooldown, _rightPaddleCooldown;
        private float _currentInputPaddleFrontForce = 30;

        //reference
        private KayakController _kayakController;
        private KayakParameters _kayakValues;
        private Rigidbody _kayakRigidbody;

        #region Constructor

        public CharacterNavigationState(KayakController kayak, InputManagement inputManagement)
        {
            _kayakController = kayak;
            _kayakRigidbody = kayak.Rigidbody;
            _kayakValues = kayak.KayakValues;
            _inputs = inputManagement;
            _inputs.Inputs.DEADZONE = 0.3f;
        }

        #endregion

        #region CharacterBaseState overrided function

        public override void EnterState(CharacterManager character)
        {
            //inputs
            GameplayInputs = new GameplayInputs();
            GameplayInputs.Enable();

            //values
            _rightPaddleCooldown = _kayakValues.PaddleCooldown;
            _leftPaddleCooldown = _kayakValues.PaddleCooldown;
        }

        public override void UpdateState(CharacterManager character)
        {
            PaddleCooldownManagement();
            HandlePaddleMovement();
            HandleStaticRotation();
        }

        public override void FixedUpdate(CharacterManager character)
        {
            KayakRotationManager(RotationType.Paddle);
            KayakRotationManager(RotationType.Static);
        }

        public override void SwitchState(CharacterManager character)
        {

        }

        #endregion

        #region Methods

        private void KayakRotationManager(RotationType rotationType)
        {
            //get rotation
            float rotationForceY = rotationType == RotationType.Paddle ? RotationPaddleForceY : RotationStaticForceY;
            
            //calculate rotation
            if (Mathf.Abs(rotationForceY) > 0.001f)
            {
                rotationForceY = Mathf.Lerp(rotationForceY, 0,  
                    rotationType == RotationType.Paddle ? _kayakValues.PaddleRotationDeceleration : _kayakValues.StaticRotationDeceleration);
            }
            else
            {
                rotationForceY = 0;
            }
            
            //apply transform
            Transform kayakTransform = _kayakController.transform;
            kayakTransform.Rotate(Vector3.up, rotationForceY);

            //changes values
            switch (rotationType)
            {
                case RotationType.Paddle:
                    RotationPaddleForceY = rotationForceY;
                    break;
                case RotationType.Static:
                    RotationStaticForceY = rotationForceY;
                    break;
            }
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
            RotationPaddleForceY += direction == Direction.Right ? -rotation : rotation;
            
            //balance
            const float rotationToBalanceMultiplier = 10f;
            Balance += RotationPaddleForceY * rotationToBalanceMultiplier;
        }

        private void HandlePaddleMovement()
        {
            //input -> paddleMovement
            if (_inputs.Inputs.PaddleLeft && _rightPaddleCooldown <= 0)
            {
                _rightPaddleCooldown = _kayakValues.PaddleCooldown;
                Paddle(Direction.Left);
            }
            if (_inputs.Inputs.PaddleRight && _leftPaddleCooldown <= 0)
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
            //left
            if (_inputs.Inputs.RotateLeft > _inputs.Inputs.DEADZONE)
            {
                if (Mathf.Abs(_kayakRigidbody.velocity.x + _kayakRigidbody.velocity.z) > 0.1f)
                {
                    DecelerationAndRotate(Direction.Left);
                }
                RotationStaticForceY -= _kayakValues.StaticRotationForce;
            }
            //right
            if (_inputs.Inputs.RotateRight > _inputs.Inputs.DEADZONE)
            {
                if (Mathf.Abs(_kayakRigidbody.velocity.x + _kayakRigidbody.velocity.z) > 0.1f)
                {
                    DecelerationAndRotate(Direction.Right);
                }
                RotationStaticForceY += _kayakValues.StaticRotationForce;
            }
        }

        private void DecelerationAndRotate(Direction direction)
        {
            Vector3 targetVelocity = new Vector3(0, _kayakRigidbody.velocity.y, 0);
            _kayakRigidbody.velocity = Vector3.Lerp(_kayakRigidbody.velocity, targetVelocity, _kayakValues.VelocityDecelerationLerp);
            float force = _kayakValues.VelocityDecelerationRotationForce;
            RotationStaticForceY += direction == Direction.Left ? -force : force;
        }

        #endregion
    }
}




