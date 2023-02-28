using Kayak;
using UnityEngine;

namespace WaterFlowGPE
{
    public class WaterFlowBlock : MonoBehaviour
    {
        [SerializeField] 
        private float _speed;

        [SerializeField, Range(0, 1), Tooltip("Multiplier applied to the speed when the boat isn't facing the direction")]
        private float _speedNotFacingMultiplier = 0.5f;
        
        [SerializeField, Range(0,0.1f), Tooltip("The lerp applied to the boat rotation to match the flow direction when the boat is already facing the flow direction")] 
        private float _rotationLerpWhenInDirection = 0.05f;
        
        [SerializeField, Range(0,0.05f), Tooltip("The lerp applied to the boat rotation to match the flow direction when the boat is not facing the flow direction")] 
        private float _rotationLerpWhenNotInDirection = 0.005f;
        
        [ReadOnly] public Vector3 Direction;
        [ReadOnly] public bool IsActive = true;
        [HideInInspector] public WaterFlowManager WaterFlowManager;
        
        private void OnTriggerStay(Collider other)
        {
            KayakController kayakController = other.GetComponent<KayakController>();
            if (kayakController != null && WaterFlowManager != null) 
            {
                WaterFlowManager.SetClosestBlockToPlayer(kayakController.transform);
                if (IsActive)
                {
                    //get rotation
                    Quaternion currentRotation = kayakController.transform.rotation;
                    Vector3 currentRotationEuler = currentRotation.eulerAngles;
                    
                    //get target rotation
                    float targetYAngle = Vector3.Angle(Direction, Vector3.forward);
                    Quaternion targetRotation = Quaternion.Euler(currentRotationEuler.x,targetYAngle,currentRotationEuler.z);
                    
                    //check if the boat is facing the flow direction or not
                    const float ANGLE_TO_FACE_FLOW = 20f;
                    float angleDifference = Mathf.Abs(Mathf.Abs(currentRotationEuler.y) - Mathf.Abs(targetYAngle));
                    bool isFacingFlow = angleDifference <= ANGLE_TO_FACE_FLOW;
                    
                    //apply rotation
                    kayakController.transform.rotation = Quaternion.Lerp(currentRotation,targetRotation, 
                        isFacingFlow ? _rotationLerpWhenInDirection : _rotationLerpWhenNotInDirection);
                    
                    //apply velocity by multiplying current by speed
                    Vector3 velocity = kayakController.Rigidbody.velocity;
                    float speed = _speed * (isFacingFlow ? _speed : _speed * _speedNotFacingMultiplier);
                    kayakController.Rigidbody.velocity = new Vector3(
                        velocity.x + speed * Mathf.Sign(velocity.x), 
                        velocity.y, 
                        velocity.z + speed * Mathf.Sign(velocity.z));
                }
            }
        }
    }
}