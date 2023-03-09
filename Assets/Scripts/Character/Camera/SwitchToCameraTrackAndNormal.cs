using Kayak;
using System;
using System.Linq;
using UnityEngine;

public class SwitchToCameraTrackAndNormal : MonoBehaviour
{

    public BoxPosition[] BoxPositions;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private string _nameOfCamera;
    [ReadOnly, SerializeField] private bool _isTrigger = false;

    private bool _cameraTrack = false;

    private void Update()
    {
        CheckRaycast();

        _isTrigger = BoxPositions.ToList().OrderByDescending(x => x.IsTrigger).FirstOrDefault().IsTrigger;


        if (_isTrigger && _cameraTrack == false)
        {
            CameraTrackState cameraTrackState = new CameraTrackState(_cameraManager, this, _nameOfCamera);
            _cameraManager.SwitchState(cameraTrackState);
            _cameraTrack = true;
        }
        else if(_isTrigger == false && _cameraTrack)
        {
            CameraNavigationState cameraNavigationState = new CameraNavigationState(_cameraManager, this);
            _cameraManager.SwitchState(cameraNavigationState);
            _cameraTrack = false;
        }
    }

    private void CheckRaycast()
    {
        for (int i = 0; i < BoxPositions.Length; i++)
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

                }
                else
                {
                    BoxPosition boxPosition = box;
                    boxPosition.IsTrigger = false;
                    BoxPositions[i] = boxPosition;

         
                }
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach (var box in BoxPositions)
        {
            if (box.IsTrigger)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.blue;
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
    [ReadOnly] public bool IsTrigger;
}
