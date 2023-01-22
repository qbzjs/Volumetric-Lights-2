using System;
using UnityEngine;


[SelectionBase]
[RequireComponent(typeof(Rigidbody))]
public class KayakManager : MonoBehaviour
{
    [SerializeField] public Transform PlayerPosition;
    public KayakParameters KayakValues;
    [HideInInspector] public Rigidbody Rigidbody;
    [HideInInspector] public float Balance;
    [ReadOnly] public float CurrentSideRotationForce;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();

    }
    void Update()
    {
        SideRotationForceReset();
        //BoatRotation();
        ClampVelocity();
    }

    private void SideRotationForceReset()
    {
        CurrentSideRotationForce = Mathf.Lerp(CurrentSideRotationForce, 0, KayakValues.PaddleInputSideForceResetLerp);
    }


    private void BoatRotation()
    {
        Vector3 rotation = transform.rotation.eulerAngles;

        //z rotation 
        float rotationValueZ = Balance * KayakValues.RotationMultiplier + 180;
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, Mathf.Lerp(rotation.z, rotationValueZ, 0.01f));

        //left/right rotation
        transform.Rotate(Vector3.up, CurrentSideRotationForce);
    }

    private void ClampVelocity()
    {
        Vector3 velocity = Rigidbody.velocity;
        float velocityZ = velocity.z;
        velocityZ = Mathf.Clamp(velocityZ, -KayakValues.MaximumFrontVelocity, KayakValues.MaximumFrontVelocity);
        Rigidbody.velocity = new Vector3(velocity.x, velocity.y, velocityZ);
    }
}

[Serializable]
public struct KayakParameters
{
    public float Acceleration;
    public float Deceleration;
    public float Speed;

    public float MaximumFrontVelocity;
    public float PaddleInputFrontForce;
    public float PaddleInputFrontDeceleration;
    public float PaddleInputFrontForceAdding;
    public float PaddleInputFrontForceMaximum;
    public float PaddleInputSideForce;
    public float PaddleInputSideForceResetLerp;

    public float RotationMultiplier;
}