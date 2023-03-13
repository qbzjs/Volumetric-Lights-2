using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRespawnState : CameraStateBase
{
    public CameraRespawnState(CameraManager cameraManagerRef, MonoBehaviour monoBehaviour) :
      base(cameraManagerRef, monoBehaviour)
    {
    }

    public override void EnterState(CameraManager camera)
    {
        CameraManagerRef.ShakeCamera(0);
        CameraManagerRef.VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = CameraManagerRef.CameraDistanceRespawn;
        CameraManagerRef.CameraAngleOverride = CameraManagerRef.CameraAngleTopDownRespawn;
        ResetCameraBehindBoat();
    }
    public override void UpdateState(CameraManager camera)
    {
        CameraManagerRef.SmoothResetRotateZ();
        Respawn();
        CameraManagerRef.ApplyRotationCameraWhenCharacterDeath();
    }
    public override void FixedUpdate(CameraManager camera)
    {

    }
    public override void SwitchState(CameraManager camera)
    {

    }


    private void ResetCameraBehindBoat()
    {
        //Start
        CameraManagerRef.MakeTargetFollowRotationWithKayak();

        //Middle
        Quaternion localRotation = CameraManagerRef.CinemachineCameraTarget.transform.localRotation;
        Vector3 cameraTargetLocalPosition = CameraManagerRef.CinemachineCameraTarget.transform.localPosition;

        CameraManagerRef.CinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(localRotation, Quaternion.Euler(new Vector3(0, 0, localRotation.z)), 1f);
        cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x, 0, 1f);
        CameraManagerRef.CinemachineTargetEulerAnglesToRotation(cameraTargetLocalPosition);

        //End
        CameraManagerRef.ApplyRotationCamera();
    }


    private void Respawn()
    {
        if (CameraManagerRef.VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance >= 7)
            CameraManagerRef.VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance -= Time.deltaTime * CameraManagerRef.MultiplyTimeForDistanceWhenRespawn /* CameraManagerRef.ValueRemoveForDistanceWhenRespawn*/;

        if (CameraManagerRef.VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance <= 7 && CameraManagerRef.CameraAngleOverride <= 0)
        {
            CameraNavigationState cameraNavigationState = new CameraNavigationState(CameraManagerRef, MonoBehaviourRef);
            CameraManagerRef.SwitchState(cameraNavigationState);
        }

        if (CameraManagerRef.CameraAngleOverride > 0)
            CameraManagerRef.CameraAngleOverride -= Time.deltaTime *  CameraManagerRef.MultiplyTimeForTopDownWhenRespawn;
    }
}
