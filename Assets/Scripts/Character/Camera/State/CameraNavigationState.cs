using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNavigationState : CameraStateBase
{
    private float _timerCameraReturnBehindBoat = 0;
    private const float JOYSTICK_DIVISION_FREE_ROTATION = 10;

    public CameraNavigationState(CameraManager cameraManagerRef, MonoBehaviour monoBehaviour) :
      base(cameraManagerRef, monoBehaviour)
    {
    }

    public override void EnterState(CameraManager camera)
    {
        Debug.Log("cam nav");
        CameraManagerRef.AnimatorRef.Play("VCam FreeLook");

        CameraManagerRef.Brain.m_BlendUpdateMethod = Cinemachine.CinemachineBrain.BrainUpdateMethod.LateUpdate;

        CameraManagerRef.ResetNavigationValue();
    }
    public override void UpdateState(CameraManager camera)
    {
        if (Mathf.Abs(CameraManagerRef.RotationZ) > 0)
        {
            CameraManagerRef.SmoothResetRotateZ();
        }

        MoveCamera();

        ClampRotationCameraValue();

        CameraManagerRef.ApplyRotationCamera();

        if (Input.GetKeyDown(KeyCode.K))
        {
            CameraTrackState cameraTrackState = new CameraTrackState(CameraManagerRef, MonoBehaviourRef, "VCam TrackDolly");
            CameraManagerRef.SwitchState(cameraTrackState);
        }
    }
    public override void FixedUpdate(CameraManager camera)
    {

    }
    public override void SwitchState(CameraManager camera)
    {

    }

    private void MoveCamera()
    {
        //rotate freely with inputs
        bool rotateInput = Mathf.Abs(CameraManagerRef.Input.Inputs.RotateCamera.x) + Mathf.Abs(CameraManagerRef.Input.Inputs.RotateCamera.y) >= CameraManagerRef.Input.Inputs.DEADZONE; //0.5f;
        const float minimumVelocityToReplaceCamera = 1f;
        _timerCameraReturnBehindBoat += Time.deltaTime;
        if (rotateInput && CameraManagerRef.CanMoveCameraManually)
        {
            //Cinemachine yaw/pitch

            //Controller
            CameraManagerRef.CinemachineTargetYaw += (CameraManagerRef.Input.Inputs.RotateCamera.x * Mathf.Abs(CameraManagerRef.Input.Inputs.RotateCamera.x * CameraManagerRef.JoystickMultiplierFreeRotationForce)) / JOYSTICK_DIVISION_FREE_ROTATION;
            CameraManagerRef.CinemachineTargetPitch += (CameraManagerRef.Input.Inputs.RotateCamera.y * Mathf.Abs(CameraManagerRef.Input.Inputs.RotateCamera.y * CameraManagerRef.JoystickMultiplierFreeRotationForce)) / JOYSTICK_DIVISION_FREE_ROTATION;

            //KBM
            //CameraManagerRef.CinemachineTargetYaw += CameraManagerRef.Input.Inputs.RotateCamera.x;
            //CameraManagerRef.CinemachineTargetPitch += CameraManagerRef.Input.Inputs.RotateCamera.y;

            //last inputs
            CameraManagerRef.LastInputX = CameraManagerRef.Input.Inputs.RotateCamera.x != 0 ? CameraManagerRef.Input.Inputs.RotateCamera.x : CameraManagerRef.LastInputX;
            CameraManagerRef.LastInputY = CameraManagerRef.Input.Inputs.RotateCamera.y != 0 ? CameraManagerRef.Input.Inputs.RotateCamera.y : CameraManagerRef.LastInputY;
            _timerCameraReturnBehindBoat = 0;
        }
        //manage rotate to stay behind boat
        else if (Mathf.Abs(CameraManagerRef.RigidbodyKayak.velocity.x + CameraManagerRef.RigidbodyKayak.velocity.z) > minimumVelocityToReplaceCamera && _timerCameraReturnBehindBoat > CameraManagerRef.TimerCameraReturnBehindBoat ||
                 (Mathf.Abs(CameraManagerRef.CharacterManager.CurrentStateBase.RotationStaticForceY) > minimumVelocityToReplaceCamera / 20) && _timerCameraReturnBehindBoat > CameraManagerRef.TimerCameraReturnBehindBoat)
        {
            //avoid last input to be 0
            if (CameraManagerRef.LastInputX != 0 || CameraManagerRef.LastInputY != 0)
            {
                CameraManagerRef.LastInputX = 0;
                CameraManagerRef.LastInputY = 0;
            }

            //get target rotation
            Quaternion localRotation = CameraManagerRef.CinemachineCameraTarget.transform.localRotation;
            Quaternion targetQuaternion = Quaternion.Euler(new Vector3(0,
                -(CameraManagerRef.CharacterManager.CurrentStateBase.RotationStaticForceY + CameraManagerRef.CharacterManager.CurrentStateBase.RotationPaddleForceY) * CameraManagerRef.MultiplierValueRotation,
                localRotation.z));
            //get camera local position
            Vector3 cameraTargetLocalPosition = CameraManagerRef.CinemachineCameraTarget.transform.localPosition;


            const float rotationThreshold = 0.15f;
            float rotationStaticY = CameraManagerRef.CharacterManager.CurrentStateBase.RotationStaticForceY;
            float rotationPaddleY = CameraManagerRef.CharacterManager.CurrentStateBase.RotationPaddleForceY;

            //calculate camera rotation & position
            if (Mathf.Abs(rotationStaticY) > rotationThreshold / 2 || // if kayak is rotating
                Mathf.Abs(rotationPaddleY) > rotationThreshold) //if kayak moving
            {
                CameraManagerRef.CinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(localRotation, targetQuaternion, CameraManagerRef.LerpLocalRotationMove);

                if (Mathf.Abs(rotationStaticY) > rotationThreshold / 2)// if kayak is rotating
                {
                    cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x, 0, CameraManagerRef.LerpLocalPositionNotMoving);
                }
                else if (Mathf.Abs(rotationPaddleY) > rotationThreshold)// if kayak is moving
                {
                    cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x,
                        (rotationStaticY + rotationPaddleY) * CameraManagerRef.MultiplierValuePosition, //value
                        CameraManagerRef.LerpLocalPositionMove); //time lerp
                    cameraTargetLocalPosition.z = 0;
                }
            }
            else
            {
                CameraManagerRef.CinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(localRotation, Quaternion.Euler(new Vector3(0, 0, localRotation.z)), CameraManagerRef.LerpLocalRotationNotMoving);
                cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x, 0, CameraManagerRef.LerpLocalPositionNotMoving);
            }

            //apply camera rotation & position
            CameraManagerRef.CinemachineCameraTarget.transform.localPosition = cameraTargetLocalPosition;
            CameraManagerRef.CinemachineTargetYaw = CameraManagerRef.CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            if (CameraManagerRef.CinemachineCameraTarget.transform.rotation.eulerAngles.x > 180)
            {
                CameraManagerRef.CinemachineTargetPitch = CameraManagerRef.CinemachineCameraTarget.transform.rotation.eulerAngles.x - 360;
            }
            else
            {
                CameraManagerRef.CinemachineTargetPitch = CameraManagerRef.CinemachineCameraTarget.transform.rotation.eulerAngles.x;
            }

            CameraManagerRef.MakeTargetFolloRotationWithKayak();
        }
        else
        {
            CameraManagerRef.LastInputValue();
        }
    }
}
