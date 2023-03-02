using Kayak;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameraMod : MonoBehaviour
{

    public BoxPosition[] BoxPositions;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private string _nameOfCamera;
    private bool _isTrigger = false;

    private void Update()
    {
        foreach (var box in BoxPositions)
        {
            RaycastHit[] hits = Physics.BoxCastAll(transform.position + box.TriggerPosition, box.TriggerSize / 2, Vector3.forward, Quaternion.identity, 0f, box.PlayerLayerMask);

            foreach (var hit in hits)
            {
                KayakController kayakController = hit.collider.GetComponent<KayakController>();
                if (kayakController != null)
                {
                    Debug.Log("cc");
                  //  _isTrigger = true;
                    //CameraTrackState cameraTrackState = new CameraTrackState(_cameraManager, this,_nameOfCamera);
                    //_cameraManager.SwitchState(cameraTrackState);
                }
                else
                {
                    Debug.Log("orv");
                    /*_isTrigger = false;
                    //CameraNavigationState cameraNavigationState = new CameraNavigationState(_cameraManager, this);
                    //_cameraManager.SwitchState(cameraNavigationState);
                */}
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
