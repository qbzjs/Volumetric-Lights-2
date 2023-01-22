using UnityEngine;

namespace Character.State
{
    public class CharacterNavigationState : CharacterBaseState
    {

        //kayak
        private KayakManager _kayakManager;
        private KayakParameters _kayakValues;

        private Vector2 _paddleForceValue;
        private float _leftPaddleMovement, _rightPaddleMovement;
        private bool _leftPaddleEngaged, _rightPaddleEngaged;
        private float _currentInputPaddleFrontForce = 30;

        //reference
        private Rigidbody _rigidbody;
        private MonoBehaviour _monoBehaviour;
        
        #region CharacterBaseState overrided function

        public override void EnterState(CharacterStateManager character)
        {
            //setup values //TODO tuple referencing
            _kayakManager = Object.FindObjectOfType<KayakManager>();
            _kayakValues = _kayakManager.KayakValues;
            PlayerPosition = Object.FindObjectOfType<CharacterInBoatTransformManager>().transform;
            _rigidbody = GameObject.Find("KayakFloatingObject").GetComponent<Rigidbody>();
        }

        public override void UpdateState(CharacterStateManager character)
        {
            HandlePaddleMovement();
            RotateMovement();
            MaxSpeedClamp();
        }

        public override void FixedUpdate(CharacterStateManager character)
        {
        }

        public override void SwitchState(CharacterStateManager character)
        {

        }
        
        #endregion

        #region Methods

        private void RotateKayak(int value)
        {
            Transform kayakTransform = _kayakManager.transform;
            const float paddleRotateForce = 10;

            Quaternion rotation = kayakTransform.rotation;
            Quaternion newRotation = Quaternion.Euler(new Vector3(rotation.x,value * paddleRotateForce,rotation.z));
            
            kayakTransform.rotation = newRotation; //TODO lerp the rotation
        }

        #endregion
        
        #region Paddle Movement
        
        private void MaxSpeedClamp()
        {
            _kayakManager.Rigidbody.velocity = Vector3.ClampMagnitude(_kayakManager.Rigidbody.velocity, _kayakValues.MaximumFrontVelocity);
        }
        
        private void Paddle()
        {
            _currentInputPaddleFrontForce += _kayakValues.PaddleInputFrontForceAdding; //TODO value never reset to 0 
            _currentInputPaddleFrontForce = Mathf.Clamp(_currentInputPaddleFrontForce, 0, _kayakValues.PaddleInputFrontForceMaximum);
            
            Vector3 forceToApply = _kayakManager.transform.forward * _currentInputPaddleFrontForce;
            _kayakManager.Rigidbody.AddForce(forceToApply);
        }

        private void HandlePaddleMovement()
        {
            const float paddleMovementInputValue = 1;

            //input -> paddleMovement

            if (Input.GetKeyDown(KeyCode.Q) && _leftPaddleEngaged == false)
            {
                _rightPaddleMovement -= paddleMovementInputValue;

                _leftPaddleEngaged = true;

                _rightPaddleEngaged = false;
                Paddle();

            }
            if (Input.GetKeyDown(KeyCode.D) && _rightPaddleEngaged == false)
            {
                _leftPaddleMovement -= paddleMovementInputValue;
                _rightPaddleEngaged = true;
                _leftPaddleEngaged = false;
                Paddle();
            }

            //clamp values
            _leftPaddleMovement = Mathf.Clamp(_leftPaddleMovement, 0, 1);
            _rightPaddleMovement = Mathf.Clamp(_rightPaddleMovement, 0, 1);
        }

        #endregion
        
        #region Rotate Movement
        
        private void RotateMovement()
        {
            const float paddleMovementInputValue = 1;

            if (Input.GetKeyDown(KeyCode.A))
            {
                _rightPaddleMovement -= paddleMovementInputValue;

                _leftPaddleEngaged = true;

                _rightPaddleEngaged = false;
                RotateKayak(-1);

            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _leftPaddleMovement -= paddleMovementInputValue;
                _rightPaddleEngaged = true;
                _leftPaddleEngaged = false;
                RotateKayak(1);
            }
        }
        
        #endregion
    }
}


