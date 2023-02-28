using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraStateBase
{
    public CameraStateBase(CameraManager cameraManagerRef, MonoBehaviour monoBehaviour)
    {
        CameraManagerRef = cameraManagerRef;
        MonoBehaviourRef = monoBehaviour;
    }

    protected CameraManager CameraManagerRef;
    protected MonoBehaviour MonoBehaviourRef;

    public abstract void EnterState(CameraManager camera);

    public abstract void UpdateState(CameraManager camera);

    public abstract void FixedUpdate(CameraManager camera);

    public abstract void SwitchState(CameraManager camera);


    protected void ClampRotationCameraValue()
    {
        CameraManagerRef.CinemachineTargetYaw = ClampAngle(CameraManagerRef.CinemachineTargetYaw, float.MinValue, float.MaxValue);
        CameraManagerRef.CinemachineTargetPitch = ClampAngle(CameraManagerRef.CinemachineTargetPitch, CameraManagerRef._bottomClamp, CameraManagerRef._topClamp);
    }
    protected float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

}
