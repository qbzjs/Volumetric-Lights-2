using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kayak
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody))]
    public class KayakController : MonoBehaviour
    {
        [Header("Drag")]
        [SerializeField, Range(1, 1.005f)] private float _dragReducingMultiplier = 1.0025f;
        [ReadOnly] public float DragReducingTimer;
        
        public KayakParameters KayakValues;

        [Header("References"), SerializeField] 
        private List<ParticleSystem> _frontParticles;
        public Rigidbody Rigidbody;
        
        [Header("Audio")] 
        public AudioClip PaddlingAudioClip;

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();

        }
        private void Update()
        {
            ParticleManagement();
            DragReducing();
            ClampVelocity();
        }

        private void ClampVelocity()
        {
            Vector3 velocity = Rigidbody.velocity;
            
            float velocityX = velocity.x;
            velocityX = Mathf.Clamp(velocityX, -KayakValues.MaximumFrontVelocity, KayakValues.MaximumFrontVelocity);
            
            float velocityZ = velocity.z;
            velocityZ = Mathf.Clamp(velocityZ, -KayakValues.MaximumFrontVelocity, KayakValues.MaximumFrontVelocity);
            
            Rigidbody.velocity = new Vector3(velocityX, velocity.y, velocityZ);
        }

        private void ParticleManagement()
        {
            if (Rigidbody.velocity.magnitude > 1)
            {
                _frontParticles.ForEach(x => x.Play());
            }
            else
            {
                _frontParticles.ForEach(x => x.Stop());
            }
        }

        private void DragReducing()
        {
            if (DragReducingTimer > 0)
            {
                DragReducingTimer -= Time.deltaTime;
                return;
            }
            
            Vector3 velocity = Rigidbody.velocity;
            float absX = Mathf.Abs(velocity.x);
            float absZ = Mathf.Abs(velocity.z);

            if (absX + absZ > 1)
            {
                Rigidbody.velocity = new Vector3(velocity.x * _dragReducingMultiplier, velocity.y, velocity.z * _dragReducingMultiplier);
            }
        }
    }

    [Serializable]
    public struct KayakParameters
    {
        [Header("Values")]
        [Range(0,40)] public float MaximumFrontVelocity;

        [Header("Paddle")]
        [Range(0,300)] public float PaddleFrontForce;
        [Range(0,2)] public float PaddleSideRotationForce;
        [Range(0,3)] public float PaddleCooldown;
        [Range(0, 0.25f)] public float PaddleRotationDeceleration;

        [Header("Static Rotation")] 
        [Range(0, 0.1f)] public float StaticRotationForce;
        [Range(0, 0.25f)] public float StaticRotationDeceleration;
        [Range(0,0.1f)] public float VelocityDecelerationLerp;
        [Range(0, 0.1f)] public float VelocityDecelerationRotationForce;
    }
}