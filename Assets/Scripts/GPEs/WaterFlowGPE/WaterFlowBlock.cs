using System;
using System.Collections.Generic;
using Character;
using Kayak;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace WaterFlowGPE
{
    public class WaterFlowBlock : MonoBehaviour
    {
        [Header("Parameters"), SerializeField] 
        private float _speed;

        [SerializeField, Range(0, 1), Tooltip("Max balance value added to kayak while in water flow")] 
        private float _balanceValue;
        
        [SerializeField, Tooltip("The range in-between which the balance value will be multiplied randomly")] 
        private Vector2 _balanceValueRandomMultiplierRange;
        
        [SerializeField, Range(0, 1), Tooltip("Multiplier applied to the speed when the boat isn't facing the direction")]
        private float _speedNotFacingMultiplier = 0.5f;

        [SerializeField, Range(0, 0.1f), Tooltip("The lerp applied to the boat rotation to match the flow direction when the boat is already facing the flow direction")]
        private float _rotationLerpWhenInDirection = 0.05f;

        [SerializeField, Range(0, 0.05f),
         Tooltip("The lerp applied to the boat rotation to match the flow direction when the boat is not facing the flow direction")]
        private float _rotationLerpWhenNotInDirection = 0.005f;

        [SerializeField, Range(0, 0.05f),
         Tooltip("The lerp applied to the boat rotation to match the flow direction when the player is trying to move away")]
        private float _rotationLerpWhenMoving = 0.005f;

        [Header("Particles"), SerializeField] private List<ParticleSystem> _particlesList;

        [SerializeField, Tooltip("One of the particles will play at a random time between those two values")]
        private Vector2 _randomPlayOfParticleTime;


        [Header("Infos")]
        [ReadOnly] public Vector3 Direction;
        [ReadOnly] public bool IsActive = true;
        [HideInInspector] public WaterFlowManager WaterFlowManager;
        [ReadOnly, SerializeField] private float _playParticleTime;

        private void Start()
        {
            _playParticleTime = UnityEngine.Random.Range(_randomPlayOfParticleTime.x, _randomPlayOfParticleTime.y);
        }

        private void Update()
        {
            ManageParticles();
        }

        private void OnTriggerStay(Collider other)
        {
            CheckForKayak(other);
            CheckForCameraShake(other);
        }

        private void OnTriggerExit(Collider other)
        {
            ResetCameraShake(other);
        }

        /// <summary>
        /// This method checks if a collider contains a KayakController component, and if so, applies rotation and
        /// velocity to the kayak based on its facing direction and movement. It also sets the closest block to the
        /// player if a WaterFlowManager is present.
        /// </summary>
        /// <param name="collider"> The collider to check </param>
        private void CheckForKayak(Collider collider)
        {
            KayakController kayakController = collider.GetComponent<KayakController>();
            if (kayakController != null && WaterFlowManager != null)
            {
                if (kayakController.CharacterManager.CurrentStateBase.CanBeMoved == false)
                {
                    return;
                }

                WaterFlowManager.SetClosestBlockToPlayer(kayakController.transform);
                if (IsActive)
                {
                    //get rotation
                    Quaternion currentRotation = kayakController.transform.rotation;
                    Vector3 currentRotationEuler = currentRotation.eulerAngles;
                    //get target rotation
                    float targetYAngle = Quaternion.LookRotation(Direction).eulerAngles.y;
                    Quaternion targetRotation = Quaternion.Euler(currentRotationEuler.x, targetYAngle, currentRotationEuler.z);

                    //check if the boat is facing the flow direction or not
                    const float ANGLE_TO_FACE_FLOW = 20f;
                    float angleDifference = Mathf.Abs(Mathf.Abs(currentRotationEuler.y) - Mathf.Abs(targetYAngle));
                    bool isFacingFlow = angleDifference <= ANGLE_TO_FACE_FLOW;

                    //apply rotation
                    InputManagement inputManagement = kayakController.CharacterManager.InputManagement;
                    bool isMoving = inputManagement.Inputs.PaddleLeft || inputManagement.Inputs.PaddleRight ||
                                    Mathf.Abs(inputManagement.Inputs.RotateLeft) > 0.1f ||
                                    Mathf.Abs(inputManagement.Inputs.RotateRight) > 0.1f;
                    kayakController.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation,
                        isMoving ? _rotationLerpWhenMoving :
                        isFacingFlow ? _rotationLerpWhenInDirection : _rotationLerpWhenNotInDirection);

                    //apply velocity by multiplying current by speed
                    Vector3 velocity = kayakController.Rigidbody.velocity;
                    float speed = _speed * (isFacingFlow ? _speed : _speed * _speedNotFacingMultiplier);

                    kayakController.Rigidbody.velocity = new Vector3(
                        velocity.x + speed * Mathf.Sign(velocity.x),
                        velocity.y,
                        velocity.z + speed * Mathf.Sign(velocity.z));
                    
                    //balance
                    double value = _balanceValue * UnityEngine.Random.Range(_balanceValueRandomMultiplierRange.x, _balanceValueRandomMultiplierRange.y);
                    kayakController.CharacterManager.AddBalanceValueToCurrentSide(value);
                }
            }
        }

        /// <summary>
        /// Setup the water block and its values
        /// </summary>
        /// <param name="direction">the direction the block has to face</param>
        /// <param name="waterFlowManager">the waterFlowManager script managing the block</param>
        /// <param name="width">the width of the block</param>
        public void SetupBlock(Vector3 direction, WaterFlowManager waterFlowManager, float width, float height, float depth)
        {
            WaterFlowManager = waterFlowManager;
            Direction = direction;
            transform.localScale = new Vector3(depth, height, width);
            transform.rotation = Quaternion.LookRotation(Direction);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z));
        }

        private void ManageParticles()
        {
            _playParticleTime -= Time.deltaTime;
            if (_playParticleTime <= 0)
            {
                int particleIndex = new Random().Next(0, _particlesList.Count);
                _particlesList[particleIndex].Emit(new ParticleSystem.EmitParams(), 1);
                _playParticleTime = UnityEngine.Random.Range(_randomPlayOfParticleTime.x, _randomPlayOfParticleTime.y);
            }
        }

        #region Camera
        private void CheckForCameraShake(Collider collider)
        {
            CameraManager _tempoCameraManager = collider.GetComponentInParent<CameraManager>();
            if (_tempoCameraManager != null && WaterFlowManager != null)
            {
                _tempoCameraManager.WaterFlow = true;
            }
        }
        private void ResetCameraShake(Collider other)
        {
            CameraManager _tempoCameraManager=other.GetComponentInParent<CameraManager>();
            if (_tempoCameraManager != null)
            {
                _tempoCameraManager.WaterFlow = false;
            }
        }
        #endregion

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Direction * 2);
            Gizmos.DrawSphere(transform.position + Direction * 2, 0.1f);
        }

#endif
    }
}