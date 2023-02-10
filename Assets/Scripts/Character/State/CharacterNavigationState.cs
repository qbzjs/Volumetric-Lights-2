using System.Collections;
using Character.Camera;
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
        private float _staticInputTimer;

        //kayak
        private Vector2 _paddleForceValue;
        private float _leftPaddleCooldown, _rightPaddleCooldown;
        private float _currentInputPaddleFrontForce = 30;

        //reference
        private KayakController _kayakController;
        private KayakParameters _kayakValues;
        private Rigidbody _kayakRigidbody;

        #region Constructor

        public CharacterNavigationState(KayakController kayak, InputManagement inputManagement, CharacterManager characterManagerRef, MonoBehaviour monoBehaviour) : 
            base(characterManagerRef, monoBehaviour)
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
            Debug.Log("naviguation");

            //values
            _rightPaddleCooldown = _kayakValues.PaddleCooldown;
            _leftPaddleCooldown = _kayakValues.PaddleCooldown;
            _staticInputTimer = _kayakValues.StaticRotationCooldownAfterPaddle;
                
            //booleans
            CharacterManagerRef.LerpBalanceTo0 = true;
        }

        public override void UpdateState(CharacterManager character)
        {
            PaddleCooldownManagement();
            
            //check balance 
            if (Mathf.Abs(CharacterManagerRef.Balance) >= CharacterManagerRef.BalanceLimit)
            {
                CharacterManagerRef.CamController.CanMoveCameraMaunally = false;
                _kayakController.CanReduceDrag = false;
                
                CharacterUnbalancedState characterUnbalancedState = new CharacterUnbalancedState(_kayakController, _inputs, CharacterManagerRef, MonoBehaviourRef);
                CharacterManagerRef.SwitchState(characterUnbalancedState);
                SwitchState(character);
            }
            
            MakeBoatRotationWithBalance(_kayakController.transform, 1);
        }

        public override void FixedUpdate(CharacterManager character)
        {
            SetBrakeAnimationToFalse();
            
            if (CanCharacterMove == false)
            {
                StopCharacter();
                return;
            }
            
            if (_inputs.Inputs.PaddleLeft || _inputs.Inputs.PaddleRight)
            {
                HandlePaddleMovement();
            }
            if ((_inputs.Inputs.RotateLeft != 0 || _inputs.Inputs.RotateRight != 0) && _staticInputTimer <= 0)
            {
                HandleStaticRotation();
            }

            KayakRotationManager(RotationType.Paddle);
            KayakRotationManager(RotationType.Static);
            
            VelocityToward();
        }

        public override void SwitchState(CharacterManager character)
        {
            CameraController.Instance.NormalState = false;
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
                    rotationType == RotationType.Paddle
                        ? _kayakValues.PaddleRotationDeceleration
                        : _kayakValues.StaticRotationDeceleration);
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

        private void StopCharacter()
        {
            _kayakRigidbody.velocity = Vector3.Lerp(_kayakRigidbody.velocity, Vector3.zero, 0.01f);
        }

        private void SetBrakeAnimationToFalse()
        {
            CharacterManagerRef.PaddleAnimator.SetBool("BrakeLeft", false);
            CharacterManagerRef.PaddleAnimator.SetBool("BrakeRight", false);
        }

        #endregion

        #region Paddle Movement

        private void Paddle(Direction direction)
        {
            //timers
            _kayakController.DragReducingTimer = 0.5f;
            _staticInputTimer = _kayakValues.StaticRotationCooldownAfterPaddle;
            
            //rotation
            float rotation = _kayakValues.PaddleSideRotationForce;
            RotationPaddleForceY += direction == Direction.Right ? -rotation : rotation;

            //balance
            const float rotationToBalanceMultiplier = 10f;
            CharacterManagerRef.Balance += RotationPaddleForceY * rotationToBalanceMultiplier;

            //audio
            SoundManager.Instance.PlaySound(_kayakController.PaddlingAudioClip);
            
            //animation
            CharacterManagerRef.PaddleAnimator.SetTrigger(direction == Direction.Left ? "PaddleLeft" : "PaddleRight");
            
        }

        private void HandlePaddleMovement()
        {
            float staticInput = Mathf.Abs(_inputs.Inputs.RotateLeft) + Mathf.Abs(_inputs.Inputs.RotateRight);
            
            //input -> paddleMovement
            if (_inputs.Inputs.PaddleLeft && _rightPaddleCooldown <= 0 && _inputs.Inputs.PaddleRight == false)
            {
                _rightPaddleCooldown = _kayakValues.PaddleCooldown;
                _rightPaddleCooldown = _kayakValues.PaddleCooldown / 2;
                Paddle(Direction.Left);
                MonoBehaviourRef.StartCoroutine(PaddleForceCurve());
            }
            
            if (_inputs.Inputs.PaddleRight && _leftPaddleCooldown <= 0 && _inputs.Inputs.PaddleLeft == false)
            {
                _leftPaddleCooldown = _kayakValues.PaddleCooldown;
                _leftPaddleCooldown = _kayakValues.PaddleCooldown / 2;
                Paddle(Direction.Right);
                MonoBehaviourRef.StartCoroutine(PaddleForceCurve());
            }
        }

        private IEnumerator PaddleForceCurve()
        {
            for (int i = 0; i <= _kayakValues.NumberOfForceAppliance; i++)
            {
                float x = 1f/(float)_kayakValues.NumberOfForceAppliance * i;
                float force = _kayakValues.ForceCurve.Evaluate(x) * _kayakValues.PaddleForce;
                Vector3 forceToApply = _kayakController.transform.forward * force;
                _kayakRigidbody.AddForce(forceToApply);

                yield return new WaitForSeconds(_kayakValues.TimeBetweenEveryAppliance);
            }
        }

        private void PaddleCooldownManagement()
        {
            _leftPaddleCooldown -= Time.deltaTime;
            _rightPaddleCooldown -= Time.deltaTime;

            _staticInputTimer -= Time.deltaTime;
        }

        #endregion

        #region Rotate Movement

        private void HandleStaticRotation()
        {
            bool isFast = Mathf.Abs(_kayakRigidbody.velocity.x + _kayakRigidbody.velocity.z) >= 0.1f;

            //left
            if (_inputs.Inputs.RotateLeft > _inputs.Inputs.DEADZONE)
            {
                if (isFast)
                {
                    DecelerationAndRotate(Direction.Right);
                }
                RotationStaticForceY += _kayakValues.StaticRotationForce;
                
                CharacterManagerRef.PaddleAnimator.SetBool("BrakeLeft", true);
            }
            
            //right
            if (_inputs.Inputs.RotateRight > _inputs.Inputs.DEADZONE)
            {
                if (isFast)
                {
                    DecelerationAndRotate(Direction.Left);
                }
                RotationStaticForceY -= _kayakValues.StaticRotationForce;
                
                CharacterManagerRef.PaddleAnimator.SetBool("BrakeRight", true);
            }
        }

        private void DecelerationAndRotate(Direction direction)
        {
            Vector3 targetVelocity = new Vector3(0, _kayakRigidbody.velocity.y, 0);
            
            _kayakRigidbody.velocity = Vector3.Lerp(_kayakRigidbody.velocity, targetVelocity,
                _kayakValues.VelocityDecelerationLerp);
            
            float force = _kayakValues.VelocityDecelerationRotationForce;
            
            RotationStaticForceY += direction == Direction.Left ? -force : force;
        }

        #endregion
        
    }
}