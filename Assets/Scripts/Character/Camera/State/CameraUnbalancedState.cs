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

        this.SwitchState(camera);
    }
    public override void FixedUpdate(CameraManager camera)
    {

    }
    public override void SwitchState(CameraManager camera)
    {
        //if (Input.GetKeyDown(KeyCode.K))
        if (Mathf.Abs(CameraManagerRef._characterManager.Balance) >= CameraManagerRef._characterManager.BalanceDeathLimit)
        {
            CameraDeathState cameraDeathState = new CameraDeathState(CameraManagerRef, MonoBehaviourRef);
            CameraManagerRef.SwitchState(cameraDeathState);
        }
        //if (Input.GetKeyDown(KeyCode.I))
        if (Mathf.Abs(CameraManagerRef._characterManager.Balance) < CameraManagerRef._characterManager.RebalanceAngle)
        {
            CameraNavigationState cameraNavigationState = new CameraNavigationState(CameraManagerRef, MonoBehaviourRef);
            CameraManagerRef.SwitchState(cameraNavigationState);
        }
    }


    protected void RotateCameraInZ()
    {
        if (CameraManagerRef._characterManager.Balance > 0)
        {
            CameraManagerRef.RotationZ = Mathf.Lerp(CameraManagerRef.RotationZ, CameraManagerRef._characterManager.Balance + 10, 0.01f);
        }
        else if (CameraManagerRef._characterManager.Balance < 0)
        {
            CameraManagerRef.RotationZ = Mathf.Lerp(CameraManagerRef.RotationZ, CameraManagerRef._characterManager.Balance - 10, 0.01f);
        }
    }
}
