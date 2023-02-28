using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNavigationState : CameraStateBase
{
    public CameraNavigationState(CameraManager cameraManagerRef, MonoBehaviour monoBehaviour) :
      base(cameraManagerRef, monoBehaviour)
    {
    }

    public override void EnterState(CameraManager camera)
    {
        CameraManagerRef.ResetNavigationValue();

        CameraManagerRef.CinemachineTargetYaw = CameraManagerRef._cinemachineCameraTarget.transform.rotation.eulerAngles.y;
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

        this.SwitchState(camera);
    }
    public override void FixedUpdate(CameraManager camera)
    {

    }
    public override void SwitchState(CameraManager camera)
    {
        if (Mathf.Abs(CameraManagerRef._characterManager.Balance) <= CameraManagerRef._characterManager.BalanceDeathLimit)
        {
            if (Mathf.Abs(CameraManagerRef._characterManager.Balance) > CameraManagerRef._characterManager.BalanceLimit)
            {

                CameraUnbalancedState cameraUnbalancedState = new CameraUnbalancedState(CameraManagerRef, MonoBehaviourRef);
                CameraManagerRef.SwitchState(cameraUnbalancedState);
            }
        }
    }


    protected void MoveCamera()
    {
        //rotate freely with inputs
        bool rotateInput = Mathf.Abs(CameraManagerRef._input.Inputs.RotateCamera.x) + Mathf.Abs(CameraManagerRef._input.Inputs.RotateCamera.y) >= 0.5f;
        const float minimumVelocityToReplaceCamera = 1f;

        if (rotateInput && CameraManagerRef.CanMoveCameraMaunally)
        {
            //Cinemachine yaw/pitch
            CameraManagerRef.CinemachineTargetYaw += CameraManagerRef._input.Inputs.RotateCamera.x;
            CameraManagerRef.CinemachineTargetPitch += CameraManagerRef._input.Inputs.RotateCamera.y;

            //last inputs
            CameraManagerRef.LastInputX = CameraManagerRef._input.Inputs.RotateCamera.x != 0 ? CameraManagerRef._input.Inputs.RotateCamera.x : CameraManagerRef.LastInputX;
            CameraManagerRef.LastInputY = CameraManagerRef._input.Inputs.RotateCamera.y != 0 ? CameraManagerRef._input.Inputs.RotateCamera.y : CameraManagerRef.LastInputY;
        }
        //manage rotate to stay behind boat
        else if (Mathf.Abs(CameraManagerRef._rigidbodyKayak.velocity.x + CameraManagerRef._rigidbodyKayak.velocity.z) > minimumVelocityToReplaceCamera ||
                 Mathf.Abs(CameraManagerRef._characterManager.CurrentStateBase.RotationStaticForceY) > minimumVelocityToReplaceCamera / 20)
        {
            //avoid last input to be 0
            if (CameraManagerRef.LastInputX != 0 || CameraManagerRef.LastInputY != 0)
            {
                CameraManagerRef.LastInputX = 0;
                CameraManagerRef.LastInputY = 0;
            }
            //get target rotation
            Quaternion localRotation = CameraManagerRef._cinemachineCameraTarget.transform.localRotation;
            Quaternion targetQuaternion = Quaternion.Euler(new Vector3(0,
                -(CameraManagerRef._characterManager.CurrentStateBase.RotationStaticForceY + CameraManagerRef._characterManager.CurrentStateBase.RotationPaddleForceY) * CameraManagerRef._multiplierValueRotation,
                localRotation.z));

            //get camera local position
            Vector3 cameraTargetLocalPosition = CameraManagerRef._cinemachineCameraTarget.transform.localPosition;


            const float rotationThreshold = 0.15f;
            float rotationStaticY = CameraManagerRef._characterManager.CurrentStateBase.RotationStaticForceY;
            float rotationPaddleY = CameraManagerRef._characterManager.CurrentStateBase.RotationPaddleForceY;

            //calculate camera rotation & position
            if (Mathf.Abs(rotationStaticY) > rotationThreshold / 2 || // if kayak is rotating
                Mathf.Abs(rotationPaddleY) > rotationThreshold)
            {
                CameraManagerRef._cinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(localRotation, targetQuaternion, CameraManagerRef._lerpLocalRotationMove);

                if (/*_input.Inputs.RotateLeft >= _input.Inputs.DEADZONE ||
                            _input.Inputs.RotateRight >= _input.Inputs.DEADZONE*/
                    Mathf.Abs(rotationStaticY) > rotationThreshold / 2)// if kayak is rotating
                {
                    cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x, 0, CameraManagerRef._lerpLocalPositionNotMoving);
                }
                else if (Mathf.Abs(rotationPaddleY) > rotationThreshold)// if kayak is moving
                {
                    cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x,
                        (rotationStaticY + rotationPaddleY) * CameraManagerRef._multiplierValuePosition, //value
                        CameraManagerRef._lerpLocalPositionMove); //time lerp
                    cameraTargetLocalPosition.z = 0;
                }
            }
            else
            {
                /*if (_input.Inputs.PaddleLeft == false && _input.Inputs.PaddleRight == false &&
                    _input.Inputs.RotateLeft < _input.Inputs.DEADZONE  && _input.Inputs.RotateRight < _input.Inputs.DEADZONE)
                {*/

                CameraManagerRef._cinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(localRotation, Quaternion.Euler(new Vector3(0, 0, localRotation.z)), CameraManagerRef._lerpLocalRotationNotMoving);
                cameraTargetLocalPosition.x = Mathf.Lerp(cameraTargetLocalPosition.x, 0, CameraManagerRef._lerpLocalPositionNotMoving);
                //}
            }
            //apply camera rotation & position
            CameraManagerRef._cinemachineCameraTarget.transform.localPosition = cameraTargetLocalPosition;
            CameraManagerRef.CinemachineTargetYaw = CameraManagerRef._cinemachineCameraTarget.transform.rotation.eulerAngles.y;

            if (CameraManagerRef._cinemachineCameraTarget.transform.rotation.eulerAngles.x > 180)
            {
                CameraManagerRef.CinemachineTargetPitch = CameraManagerRef._cinemachineCameraTarget.transform.rotation.eulerAngles.x - 360;
            }
            else
            {
                CameraManagerRef.CinemachineTargetPitch = CameraManagerRef._cinemachineCameraTarget.transform.rotation.eulerAngles.x;
            }
        }
        else
        {
            //last input value
            CameraManagerRef.LastInputX = ClampAngle(CameraManagerRef.LastInputX, -5.0f, 5.0f);
            CameraManagerRef.LastInputY = ClampAngle(CameraManagerRef.LastInputY, -1.0f, 1.0f);
            CameraManagerRef.LastInputX = Mathf.Lerp(CameraManagerRef.LastInputX, 0, CameraManagerRef._lerpTimeX);
            CameraManagerRef.LastInputY = Mathf.Lerp(CameraManagerRef.LastInputY, 0, CameraManagerRef._lerpTimeY);

            //apply value to camera
            CameraManagerRef.CinemachineTargetYaw += CameraManagerRef.LastInputX;
            CameraManagerRef.CinemachineTargetPitch += CameraManagerRef.LastInputY;
        }
    }




}
