using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace IcebergFallingGPE
{
    public class IcebergFalling : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }
        [field: SerializeField] public Waves WavesManager { get; private set; }
        [SerializeField, ReadOnly] public bool Fall;
        [SerializeField, ReadOnly] public bool HasFallen;

        [Header("Parameters"), SerializeField] private float _fallDuration;
        [SerializeField] private Vector3 _endPosition;
        [SerializeField] private Vector3 _endRotation;
        [SerializeField] private AnimationCurve _fallSpeedCurve;
        [SerializeField] private float _timeToHitWater;
        [SerializeField] private CircularWave _circularWaveData;

        private float _timer;
        private Vector3 _beginPosition, _targetPosition;

        private void Start()
        {
            _targetPosition = transform.position + _endPosition;
            _beginPosition = transform.position;
        }

        private void Update()
        {
            if (Fall)
            {
                HandleFall();
            }

            if (Fall && _timeToHitWater > 0)
            {
                _timeToHitWater -= Time.deltaTime;
                if (_timeToHitWater <= 0)
                {
                    _circularWaveData.Center = new Vector2(transform.position.x,transform.position.z);
                    WavesManager.LaunchCircularWave(_circularWaveData);
                }
            }
        }

        public void SetFall()
        {
            if (Fall == false && _timer < 1)
            {
                Fall = true;
                HasFallen = true;
                _timer = 0;
            }
        }

        private void HandleFall()
        {
            _timer += Time.deltaTime;
            float fallProgress = _timer / _fallDuration;
            Vector3 targetPosition = Vector3.Lerp(_beginPosition, _targetPosition, _fallSpeedCurve.Evaluate(fallProgress));
            Quaternion targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_endRotation), _fallSpeedCurve.Evaluate(fallProgress));
            transform.position = targetPosition;
            transform.rotation = targetRotation;

            if (fallProgress >= 1)
            {
                Fall = false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Vector3 startPosition = HasFallen ? _beginPosition : transform.position;
            Vector3 endPosition = HasFallen ?  _targetPosition : transform.position + _endPosition;
            Gizmos.DrawSphere(endPosition, 0.2f);
            Handles.DrawDottedLine(startPosition, startPosition + _endPosition, 0.5f);
        }
#endif
    }
}