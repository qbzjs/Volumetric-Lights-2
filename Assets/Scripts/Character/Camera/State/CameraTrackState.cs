using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackState : CameraStateBase
{
    private string _cameraName;


    public CameraTrackState(CameraManager cameraManagerRef, MonoBehaviour monoBehaviour, string cameraName) :
      base(cameraManagerRef, monoBehaviour)
    {
        _cameraName = cameraName;
    }

    public override void EnterState(CameraManager camera)
    {
        Debug.Log("track");
        CameraManagerRef.AnimatorRef.Play(_cameraName);
        CameraManagerRef.Brain.m_BlendUpdateMethod = Cinemachine.CinemachineBrain.BrainUpdateMethod.FixedUpdate;

    }
    public override void UpdateState(CameraManager camera)
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CameraNavigationState cameraNavigationState = new CameraNavigationState(CameraManagerRef, MonoBehaviourRef);
            CameraManagerRef.SwitchState(cameraNavigationState);
        }
    }
    public override void FixedUpdate(CameraManager camera)
    {

    }
    public override void SwitchState(CameraManager camera)
    {

    }
}
