using IcebergFallingGPE;
using Kayak;
using UnityEngine;
using UnityEngine.Events;

public class IcebergFallingManager : MonoBehaviour
{
    [Header("TriggerBox"), SerializeField] private Vector3 _triggerSize = Vector3.one;
    [SerializeField] private Vector3 _triggerOffsetPosition = Vector3.zero;
    [SerializeField] private LayerMask _playerLayerMask;
    [Header("Event")] public UnityEvent OnPlayerDetected = new UnityEvent();

    private void Update()
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + _triggerOffsetPosition, _triggerSize / 2, Vector3.forward, Quaternion.identity, 0f, _playerLayerMask);
        foreach (RaycastHit hit in hits)
        {
            KayakController kayakController = hit.collider.gameObject.GetComponent<KayakController>();
            if (kayakController != null)
            {
                OnPlayerDetected.Invoke();
            }
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + _triggerOffsetPosition, _triggerSize);
    }

#endif
}
