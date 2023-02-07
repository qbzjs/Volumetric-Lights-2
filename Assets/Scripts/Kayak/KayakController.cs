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
        [SerializeField, Range(50,51f)] private float _dragReducingMultiplier = 50.5f;
        [ReadOnly] public float DragReducingTimer;
        
        [Header("Parameters")]
        public KayakParameters KayakValues;
        [ReadOnly] public bool CanReduceDrag = true;
        
        [Header("References"), SerializeField] 
        private List<ParticleSystem> _frontParticles;
        public Rigidbody Rigidbody;
        public Transform Mesh;
        
        [Header("Audio")] 
        public AudioClip PaddlingAudioClip;

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();

        }
        private void Update()
        {
            ParticleManagement();
            ClampVelocity();
        }

        private void FixedUpdate()
        {
            DragReducing();
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
            if (DragReducingTimer > 0 || CanReduceDrag == false)
            {
                DragReducingTimer -= Time.deltaTime;
                return;
            }
            
            Vector3 velocity = Rigidbody.velocity;
            float absX = Mathf.Abs(velocity.x);
            float absZ = Mathf.Abs(velocity.z);

            if (absX + absZ > 1)
            {
                Rigidbody.velocity = new Vector3(
                    velocity.x * _dragReducingMultiplier * Time.deltaTime, 
                      velocity.y, 
                    velocity.z * _dragReducingMultiplier * Time.deltaTime);
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
        [Header("new values")] 
        public float EndForce;
        public int ForceApplyTimeInFrames;
        public AnimationCurve ForceCurve;

        [Header("Static Rotation")] 
        [Range(0, 0.1f)] public float StaticRotationForce;
        [Range(0, 0.25f)] public float StaticRotationDeceleration;
        [Range(0,0.1f)] public float VelocityDecelerationLerp;
        [Range(0, 0.1f)] public float VelocityDecelerationRotationForce;

        [Header("Unbalanced")] 
        [Range(0, 1)] public float UnbalancePaddleCooldown;
        [Range(0, 10)] public float UnbalancePaddleForce;
    }
}