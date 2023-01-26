using System;
using UnityEngine;

namespace Character
{
    public class InputManagement : MonoBehaviour
    {
        public GameplayInputs GameplayInputs;
        public InputsEnum Inputs;

        private void Awake()
        {
            GameplayInputs = new GameplayInputs();
            GameplayInputs.Enable();
        }

        private void Update()
        {
            GatherInputs();
        }
        
        private void GatherInputs()
        {
            Inputs.PaddleLeft = GameplayInputs.Boat.PaddleLeft.triggered;
            Inputs.PaddleRight = GameplayInputs.Boat.PaddleRight.triggered;

            Inputs.RotateLeft = GameplayInputs.Boat.StaticRotateLeft.ReadValue<float>();
            Inputs.RotateRight = GameplayInputs.Boat.StaticRotateRight.ReadValue<float>();

            Inputs.RotateCameraClick = GameplayInputs.Boat.RotateCameraActivation.ReadValue<float>() > 0;
            
            Inputs.RotateCamera = GameplayInputs.Boat.RotateCamera.ReadValue<Vector2>();
        }
    }
    
    public struct InputsEnum
    {
        public bool PaddleLeft;
        public bool PaddleRight;
        public float RotateLeft;
        public float RotateRight;
        public bool RotateCameraClick;
        public Vector2 RotateCamera;
        public float DEADZONE;
    }
}