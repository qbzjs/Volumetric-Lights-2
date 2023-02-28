using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCombatState : CameraStateBase
{
    public CameraCombatState(CameraManager cameraManagerRef, MonoBehaviour monoBehaviour) :
        base(cameraManagerRef, monoBehaviour)
    {
    }

    public override void EnterState(CameraManager camera)
    {
        Debug.Log("Camera Combat");
    }
    public override void UpdateState(CameraManager camera)
    {

    }
    public override void FixedUpdate(CameraManager camera)
    {

    }
    public override void SwitchState(CameraManager camera)
    {

    }
}
