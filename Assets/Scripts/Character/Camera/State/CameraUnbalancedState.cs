using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraUnbalancedState : CameraStateBase
{
    public CameraUnbalancedState(CameraManager cameraManagerRef, MonoBehaviour monoBehaviour) :
       base(cameraManagerRef, monoBehaviour)
    {
    }

    public override void EnterState(CameraManager camera)
    {
    }
    public override void UpdateState(CameraManager camera)
    {
        CameraManagerRef.MakeSmoothCameraBehindBoat();
        CameraManagerRef.MakeTargetFollowRotationWithKayak();
        if (Mathf.Abs(CameraManagerRef.CharacterManager.Balance) < CameraManagerRef.CharacterManager.BalanceDeathLimit)
            RotateCameraInZ();
        else
            CameraManagerRef.SmoothResetRotateZ();

        CameraManagerRef.ApplyRotationCamera();

    }
    public override void FixedUpdate(CameraManager camera)
    {

    }
    public override void SwitchState(CameraManager camera)
    {

    }


    private void RotateCameraInZ()
    {
        if (CameraManagerRef.CharacterManager.Balance > 0)
        {
            CameraManagerRef.RotationZ = Mathf.Lerp(CameraManagerRef.RotationZ, CameraManagerRef.CharacterManager.Balance + 10, 0.01f);
        }
        else if (CameraManagerRef.CharacterManager.Balance < 0)
        {
            CameraManagerRef.RotationZ = Mathf.Lerp(CameraManagerRef.RotationZ, CameraManagerRef.CharacterManager.Balance - 10, 0.01f);
        }
    }

    private void ShakeCamera(float intensity)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = CameraManagerRef.VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    }

}
