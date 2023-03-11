using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
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
        [SerializeField, Range(0, 1)] private float _lookForwardLerp = 0.1f;

        [Header("Position"), SerializeField] private bool _doNotAffectY;
        [SerializeField] private Vector3 _positionOffset;

        [Header("Size"), SerializeField] private bool _changeSizeAtStartAndEnd = true;
        [SerializeField, Range(0, 5)] private float _timeToScaleStart, _timeToScaleEnd;
        [SerializeField] private Vector3 _size = Vector3.one;

        private bool _hasDecreasedSize;
        private float _progress;
        private bool _goingForward = true;

        [Header("Events")]
        public UnityEvent OnSpawn = new UnityEvent();
        public UnityEvent OnDecrease = new UnityEvent();
        public UnityEvent OnEnd = new UnityEvent();

        private void Update()
        {
            if (_changeSizeAtStartAndEnd)
            {
                ManageSize();
            }

            ManageProgress();
            ManagePosition();
        }

        private void ManageSize()
        {
            if (_progress == (_goingForward ? 0 : 1))
            {
                transform.localScale = Vector3.zero;
                transform.DOScale(_size, _timeToScaleStart);
                _hasDecreasedSize = false;
                OnSpawn.Invoke();
            }

            float startDecreaseSize = _goingForward ? (_duration - _timeToScaleEnd) / _duration : 1 - (_duration - _timeToScaleEnd) / _duration;

            if (_goingForward && _hasDecreasedSize == false) 
            {
                if (_progress >= startDecreaseSize)
                {
                    DecreaseSize();
                }
            }
            else
            {
                if (_progress <= startDecreaseSize)
                {
                    DecreaseSize();
                }
            }
            
        }

        private void DecreaseSize()
        {
            OnDecrease.Invoke();
            transform.DOScale(Vector3.zero, _timeToScaleEnd).OnComplete(InvokeEnd);
            _hasDecreasedSize = true;
        }

        private void InvokeEnd()
        {
            OnEnd.Invoke();
        }

        private void ManagePosition()
        {
            //set position
            Vector3 pointPosition = _spline.GetPoint(_progress);
            Vector3 position = new Vector3(pointPosition.x, _doNotAffectY ? transform.position.y : pointPosition.y,
                pointPosition.z);
            transform.position = position + _positionOffset;
            if (_lookForward)
            {
                Quaternion currentRotation = transform.rotation;
                transform.LookAt(position + _spline.GetDirection(_progress) * (_goingForward ? 1 : -1));
                Quaternion lerpRotation = Quaternion.Lerp(currentRotation, transform.rotation, _lookForwardLerp);
                transform.rotation = lerpRotation;
            }
        }

        private void ManageProgress()
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
                            _progress = 1f;
                            _goingForward = false;
                            break;
                        case WalkerMode.Loop:
                            _progress = 0f;
                            break;
                    }
                }
            }
            else
            {
                _progress -= Time.deltaTime / _duration;
                if (_progress < 0f)
                {
                    _progress = 0f;
                    _goingForward = true;
                }
            }
        }
    }
}