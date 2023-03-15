using Kayak;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace GPEs
{
    public class PlayerTriggerManager : MonoBehaviour
    {
        public enum TriggerType
        {
            BoxTrigger = 0,
            SphereTrigger = 1
        }

        [Header("Trigger"), SerializeField] private TriggerType _triggerType;
        [SerializeField] private bool _showTriggerGizmos = true;
        [SerializeField] private Vector3 _triggerBoxSize = Vector3.one;
        [SerializeField] private float _triggerSphereSize = 1;
        [SerializeField] private Vector3 _triggerOffsetPosition = Vector3.zero;
        [SerializeField] private LayerMask _playerLayerMask;
        
        [Header("Event")] public UnityEvent OnPlayerEntered = new UnityEvent();
        public UnityEvent OnPlayerStay = new UnityEvent();
        public UnityEvent OnPlayerExited = new UnityEvent();

        protected KayakController KayakController;

        public virtual void Update()
        {
            bool kayak = false;
            
            RaycastHit[] hits = new RaycastHit[] { };
            if (_triggerType == TriggerType.BoxTrigger)
            {
                hits = Physics.BoxCastAll(transform.position + _triggerOffsetPosition, _triggerBoxSize / 2, Vector3.forward, Quaternion.identity, 0f, _playerLayerMask);
                
            }
            if (_triggerType == TriggerType.SphereTrigger)
            {
                hits = Physics.SphereCastAll(transform.position + _triggerOffsetPosition, _triggerSphereSize, Vector3.forward);
            }
            foreach (RaycastHit hit in hits)
            {
                KayakController kayakController = hit.collider.gameObject.GetComponent<KayakController>();
                if (kayakController != null)
                {
                    if (KayakController == null)
                    {
                        KayakController = kayakController;
                        OnPlayerEntered.Invoke();
                    }
                    OnPlayerStay.Invoke();
                    kayak = true;
                }
            }

            if (KayakController != null && kayak == false)
            {
                KayakController = null;
                OnPlayerExited.Invoke();
            }
        }

#if UNITY_EDITOR

        public virtual void OnDrawGizmos()
        {
            if (_showTriggerGizmos)
            {
                Gizmos.color = Color.green;
                if (_triggerType == TriggerType.BoxTrigger)
                {
                    Gizmos.DrawWireCube(transform.position + _triggerOffsetPosition, _triggerBoxSize);
                }
                if (_triggerType == TriggerType.SphereTrigger)
                {
                    Gizmos.DrawWireSphere(transform.position + _triggerOffsetPosition, _triggerSphereSize);
                }
            }
        }

#endif
    }
}
