using System;
using UnityEngine;

namespace Kayak
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody))]
    public class KayakController : MonoBehaviour
    {
        public KayakParameters KayakValues;
        public Rigidbody Rigidbody;
        [ReadOnly] public float CurrentSideRotationForce;

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();

        }
        private void Update()
        {
            SideRotationForceReset();
            ClampVelocity();
        }

        private void SideRotationForceReset()
        {
            CurrentSideRotationForce = Mathf.Lerp(CurrentSideRotationForce, 0, KayakValues.PaddleInputSideForceResetLerp);
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
        [Range(0,1)] public float Acceleration;
        [Range(0,1)] public float Deceleration;
        [Range(0,100)] public float Speed;

        [Range(0,20)] public float MaximumFrontVelocity;
        [Range(0,100)] public float PaddleInputFrontForce;
        [Range(0,1)] public float PaddleInputFrontDeceleration;
        [Range(0,20)] public float PaddleInputFrontForceAdding;
        [Range(0,200)] public float PaddleInputFrontForceMaximum;
        [Range(0,1)] public float PaddleInputSideForce;
        [Range(0,1)] public float PaddleInputSideForceResetLerp;

        [Range(0,20)] public float RotationMultiplier;
    }
}