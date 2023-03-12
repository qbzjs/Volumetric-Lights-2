using Kayak;
using UnityEngine;
using UnityEngine.Events;

namespace GPEs
{
    public class PlayerTriggerManager : MonoBehaviour
    {
        [Header("TriggerBox"), SerializeField] private Vector3 _triggerSize = Vector3.one;
        [SerializeField] private Vector3 _triggerOffsetPosition = Vector3.zero;
        [SerializeField] private LayerMask _playerLayerMask;
        [Header("Event")] public UnityEvent OnPlayerDetected = new UnityEvent();

        protected KayakController KayakController;

        protected virtual void Update()
        {
            RaycastHit[] hits = Physics.BoxCastAll(transform.position + _triggerOffsetPosition, _triggerSize / 2, Vector3.forward, Quaternion.identity, 0f, _playerLayerMask);
            foreach (RaycastHit hit in hits)
            {
                KayakController kayakController = hit.collider.gameObject.GetComponent<KayakController>();
                if (kayakController != null)
                {
                    KayakController = kayakController;
                    OnPlayerDetected.Invoke();
                }
            }
        }

#if UNITY_EDITOR

        public virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + _triggerOffsetPosition, _triggerSize);
        }

#endif
    }
}
