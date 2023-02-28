using System;
using Character;
using Kayak;
using UnityEngine;
using UnityEngine.Serialization;

namespace WaterFlowGPE
{
    public class WaterFlowBlock : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] private float _speed;
        [ReadOnly] public Vector3 Direction;
        [ReadOnly] public bool IsActive = true;
        [HideInInspector] public WaterFlowManager WaterFlowManager;
        
        private void OnTriggerStay(Collider other)
        {
            KayakController kayakController = other.GetComponent<KayakController>();
            if (kayakController != null)
            {
                WaterFlowManager.SetClosestBlockToPlayer(kayakController.transform);
                kayakController.transform.position += Direction * _speed * Time.deltaTime * (IsActive ? 1 : 0);
                kayakController.Rigidbody.AddForce(Direction * _speed * Time.deltaTime * (IsActive ? 1 : 0));
                Debug.Log($"active : {IsActive}");
            }
        }
    }
}