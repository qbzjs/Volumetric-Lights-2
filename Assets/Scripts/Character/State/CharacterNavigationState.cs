using System;
using UnityEngine;

public class CharacterNavigationState : CharacterBaseState
{

    //kayak
    private KayakManager _kayakManager;
    private KayakParameters _kayakValues;

    private Vector2 _paddleForceValue;
    private float _leftPaddleMovement, _rightPaddleMovement;
    private bool _leftPaddleEngaged = false, _rightPaddleEngaged = false;
    private float _currentInputPaddleFrontForce = 30;

    //reference
    private Rigidbody _rigidbody;
    private MonoBehaviour _monoBehaviour;


    public override void EnterState(CharacterStateManager character)
    {
        Debug.Log("Navigation State");

        //setup values
        _kayakManager = GameObject.FindObjectOfType<KayakManager>();
        _kayakValues = _kayakManager.KayakValues;
        PosPlayer = GameObject.FindObjectOfType<CharacterPosition>().transform;
        _rigidbody = GameObject.Find("KayakFloatingObject").GetComponent<Rigidbody>();
    }

    public override void UpdateState(CharacterStateManager character)
    {
        HandlePaddleMovement();
        RotateMovement();
        MaxSpeed();
    }

    public override void FixedUpdate(CharacterStateManager character)
    {
    }

    public override void SwitchState(CharacterStateManager character)
    {

    }


    private void PaddleRotate(int value)
    {
        //c'est de la grosse merde
        Debug.Log("rotate");

        var rotate = _kayakManager.transform.rotation;
        rotate.y = _kayakManager.transform.rotation.y + value * 10;

        _kayakManager.transform.rotation = Quaternion.Lerp(_kayakManager.transform.rotation, rotate, Time.deltaTime * 1);
    }

    void MaxSpeed()
    {
        var sqrMaxVelocity = _kayakValues.MaximumFrontVelocity * _kayakValues.MaximumFrontVelocity;
        if (_kayakManager.Rigidbody.velocity.sqrMagnitude > sqrMaxVelocity)
        {
            _kayakManager.Rigidbody.velocity = _kayakManager.Rigidbody.velocity.normalized * _kayakValues.MaximumFrontVelocity;
        }
    }
    private void Paddle()
    {
        Debug.Log("move");
        _currentInputPaddleFrontForce += _kayakValues.PaddleInputFrontForceAdding;

        //force & apply to rigidbody
        _currentInputPaddleFrontForce = Mathf.Clamp(_currentInputPaddleFrontForce, 0, _kayakValues.PaddleInputFrontForceMaximum);
        _kayakManager.Rigidbody.AddForce(_kayakManager.transform.forward * _currentInputPaddleFrontForce);
        //_kayakManager.Rigidbody.AddForce(_kayakManager.transform.forward * _kayakValues.Speed);
    }

    private void HandlePaddleMovement()
    {
        float paddleMovementInputValue = 1;

        //input -> paddleMovement

        if (Input.GetKeyDown(KeyCode.Q) && !_leftPaddleEngaged)
        {
            _rightPaddleMovement -= paddleMovementInputValue;

            _leftPaddleEngaged = true;

            _rightPaddleEngaged = false;
            Paddle();

        }
        if (Input.GetKeyDown(KeyCode.D) && !_rightPaddleEngaged)
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

    private void RotateMovement()
    {
        float paddleMovementInputValue = 1;

        if (Input.GetKeyDown(KeyCode.A))
        {
            _rightPaddleMovement -= paddleMovementInputValue;

            _leftPaddleEngaged = true;

            _rightPaddleEngaged = false;
            PaddleRotate(-1);

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _leftPaddleMovement -= paddleMovementInputValue;
            _rightPaddleEngaged = true;
            _leftPaddleEngaged = false;
            PaddleRotate(1);
        }
    }
}


