using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeathState : CameraStateBase
{



    public CameraDeathState(CameraManager cameraManagerRef, MonoBehaviour monoBehaviour) :
      base(cameraManagerRef, monoBehaviour)
    {
    }

    public override void EnterState(CameraManager camera)
    {
    }
    public override void UpdateState(CameraManager camera)
    {
        Isdead();
    }
    public override void FixedUpdate(CameraManager camera)
    {

    }
    public override void SwitchState(CameraManager camera)
    {

    }


    private void Isdead()
    {
        CameraManagerRef.SmoothResetRotateZ();
        CameraManagerRef._virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance += 0.05f;

        if (CameraManagerRef._virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance > 10)
        {
            CameraManagerRef.StartTimerDeath = true;
        }
    }
}
