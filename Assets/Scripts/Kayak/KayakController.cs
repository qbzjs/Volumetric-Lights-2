using System;
using System.Collections.Generic;
using Character.State;
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
        [SerializeField] private CharacterManager _characterManager;
        public Rigidbody Rigidbody;
        public Transform Mesh;
        
        [Header("Audio")] 
        public AudioClip PaddlingAudioClip;
        [SerializeField] private AudioClip CollisionAudioClip;

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

        private void OnCollisionEnter(Collision collision)
        {
            float value = collision.relativeVelocity.magnitude / KayakValues.CollisionToBalanceMagnitudeDivider;
            Debug.Log($"collision V.M. :{Math.Round(collision.relativeVelocity.magnitude)} -> {Math.Round(value,2)}");
            _characterManager.Balance += value * Mathf.Sign(_characterManager.Balance);
            SoundManager.Instance.PlaySound(CollisionAudioClip);
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
        [Header("Velocity & Collisions")]
        [Range(0,40)] public float MaximumFrontVelocity;
        [Range(0,40)] public float CollisionToBalanceMagnitudeDivider;

        [Header("Paddle")]
        [Range(0,2)] public float PaddleSideRotationForce;
        [Range(0,3)] public float PaddleCooldown;
        [Range(0, 0.25f)] public float PaddleRotationDeceleration;
        [Range(0,200)] public float PaddleForce;
        [Range(0,30)] public int NumberOfForceAppliance;
        [Range(0,0.2f)] public float TimeBetweenEveryAppliance;
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