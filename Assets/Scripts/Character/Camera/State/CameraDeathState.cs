using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeathState : CameraStateBase
{

    private float _rotationZ;

    public CameraDeathState(CameraManager cameraManagerRef, MonoBehaviour monoBehaviour) :
      base(cameraManagerRef, monoBehaviour)
    {
    }

    public override void EnterState(CameraManager camera)
    {
        Debug.Log("cam death");
        CameraManagerRef.CameraAngleOverride = 0;
        _rotationZ = CameraManagerRef.RotationZ;
    }
    public override void UpdateState(CameraManager camera)
    {
        CameraManagerRef.SmoothResetRotateZ();
        if (Mathf.Abs(CameraManagerRef.RotationZ) <= 5)
        {
            Isdead();
        }
        CameraManagerRef.ApplyRotationCameraWhenCharacterDeath();
    }
    public override void FixedUpdate(CameraManager camera)
    {

    }
    public override void SwitchState(CameraManager camera)
    {

    }


    private void Isdead()
    {

        CameraManagerRef.VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance += CameraManagerRef.ValueAddForDistanceWhenDeath;

        if (CameraManagerRef.VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance > CameraManagerRef.MaxValueDistanceToStartDeath)
        {
            CameraManagerRef.StartDeath = true;
        }
        CameraManagerRef.CameraAngleOverride += CameraManagerRef.ValueAddForTopDownWhenDeath;
    }

}
