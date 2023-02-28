using UnityEngine;
using WaterFlowGPE.Bezier;

namespace WaterFlowGPE.SpineWalker
{
    public class SplineWalker : MonoBehaviour
    {
        private enum WalkerMode
        {
            OneShot,
            PingPong,
            Loop
        }
    
        [Header("Spline"), SerializeField] private BezierSpline _spline;
        [Header("Parameters"), SerializeField] private float _duration;
        [SerializeField] private WalkerMode _mode;
        [Header("Rotation"), SerializeField] private bool _lookForward;
        [SerializeField, Range(0,1)] private float _lookForwardLerp = 0.1f;
        [Header("Position"), SerializeField] private bool _doNotAffectY;

        private float _progress;
        private bool _goingForward = true;

        private void Update () 
        {
            if (_goingForward) 
            {
                _progress += Time.deltaTime / _duration;
            
                if (_progress > 1f) 
                {
                    switch (_mode)
                    {
                        case WalkerMode.OneShot:
                            _progress = 1f;
                            break;
                        case WalkerMode.PingPong:
                            _progress = 2f - _progress;
                            _goingForward = false;
                            break;
                        case WalkerMode.Loop:
                            _progress -= 1f;
                            break;
                    }
                }
            }
            else 
            {
                _progress -= Time.deltaTime / _duration;
                if (_progress < 0f) 
                {
                    _progress = -_progress;
                    _goingForward = true;
                }
            }

            //set position
            Vector3 pointPosition = _spline.GetPoint(_progress);
            Vector3 position = new Vector3(pointPosition.x, _doNotAffectY ? transform.localPosition.y : pointPosition.y, pointPosition.z);
            transform.localPosition = position;
            if (_lookForward)
            {
                Quaternion currentRotation = transform.rotation;
                transform.LookAt(position + _spline.GetDirection(_progress) * (_goingForward ? 1 : -1));
                Quaternion lerpRotation = Quaternion.Lerp(currentRotation, transform.rotation, _lookForwardLerp);
                transform.rotation = lerpRotation;
            }
        }
    }
}
