using UnityEngine;

namespace Character
{
    public struct Inputs
    {
        public bool PaddleLeft;
        public bool PaddleRight;
        public float RotateLeft;
        public float RotateRight;
        public Vector2 RotateCamera;
        public float DEADZONE;
    }
}