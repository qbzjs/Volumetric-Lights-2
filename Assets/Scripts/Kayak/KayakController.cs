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

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();

        }
        private void Update()
        {
            ClampVelocity();
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
        [Header("Values")]
        [Range(0,20)] public float MaximumFrontVelocity;
        [Range(0, 1)] public float RotationDeceleration;

        [Header("Paddle")]
        [Range(0,200)] public float PaddleFrontForce;
        [Range(0,2)] public float PaddleSideRotationForce;
        [Range(0,3)] public float PaddleCooldown;

        [Header("Static Rotation")] 
        [Range(0, 2)] public float StaticRotationForce;
    }
}