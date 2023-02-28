using Kayak;
using UnityEngine;

namespace WaterFlowGPE
{
    public class WaterFlowBlock : MonoBehaviour
    {
        [SerializeField] private float _speed;
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
                    // -> rotate kayak lerp toward direction
                    Vector3 rotation = kayakController.transform.rotation.eulerAngles;
                    float angle = Vector3.Angle(Direction, Vector3.forward);
                    Quaternion targetRotation = Quaternion.Euler(rotation.x,angle,rotation.z);
                    kayakController.transform.rotation = Quaternion.Lerp(Quaternion.Euler(rotation),targetRotation, 0.1f);
                    
                    // -> apply velocity by multiplying current by speed
                    Vector3 velocity = kayakController.Rigidbody.velocity;
                    kayakController.Rigidbody.velocity = new Vector3(velocity.x + _speed, velocity.y, velocity.z + _speed); // * Time.deltaTime;
                }
            }
        }
    }
}