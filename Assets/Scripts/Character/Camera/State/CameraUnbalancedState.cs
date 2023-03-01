using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        RotateCameraInZ();
        CameraManagerRef.ApplyRotationCamera();

    }
    public override void FixedUpdate(CameraManager camera)
    {

    }
    public override void SwitchState(CameraManager camera)
    {
       
    }


    protected void RotateCameraInZ()
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
}
