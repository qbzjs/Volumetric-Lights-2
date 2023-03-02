using Kayak;
using System;
using System.Linq;
using UnityEngine;

public class SwitchCameraMod : MonoBehaviour
{

    public BoxPosition[] BoxPositions;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private string _nameOfCamera;
    [ReadOnly, SerializeField] private bool _isTrigger = false;

    private void Update()
    {
        CheckRaycast();

        _isTrigger = BoxPositions.ToList().Find(x => x.IsTrigger).IsTrigger;
    }

    private void CheckRaycast()
    {
        for (int i = 0; i <= BoxPositions.Length; i++)
        {
            BoxPosition box = BoxPositions[i];
            RaycastHit[] hits = Physics.BoxCastAll(transform.position + box.TriggerPosition, box.TriggerSize / 2,
                Vector3.forward, Quaternion.identity, 0f, box.PlayerLayerMask);

            foreach (var hit in hits)
            {
                KayakController kayakController = hit.collider.GetComponent<KayakController>();
                if (kayakController != null)
                {
                    BoxPosition boxPosition = box;
                    boxPosition.IsTrigger = true;
                    BoxPositions[i] = boxPosition;

                    //CameraTrackState cameraTrackState = new CameraTrackState(_cameraManager, this,_nameOfCamera);
                    //_cameraManager.SwitchState(cameraTrackState);
                }
                else
                {
                    BoxPosition boxPosition = box;
                    boxPosition.IsTrigger = false;
                    BoxPositions[i] = boxPosition;

                    //CameraNavigationState cameraNavigationState = new CameraNavigationState(_cameraManager, this);
                    //_cameraManager.SwitchState(cameraNavigationState);
                }
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach (var box in BoxPositions)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + box.TriggerPosition, box.TriggerSize);
        }
    }
#endif
}

[Serializable]
public struct BoxPosition
{
    public Vector3 TriggerSize;
    public Vector3 TriggerPosition;
    public LayerMask PlayerLayerMask;
    public bool IsTrigger;
}
