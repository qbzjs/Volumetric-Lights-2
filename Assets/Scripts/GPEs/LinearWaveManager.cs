using System;
using UnityEngine;
using WaterAndFloating;

namespace GPEs
{
    public class LinearWaveManager : PlayerTriggerManager
    {
        [field:SerializeField] private Waves Waves { get; set; }
        
        [SerializeField] private LinearWave _waveData;
        [ReadOnly, SerializeField] private bool _hasBeenLaunched;

        private void Start()
        {
            OnPlayerDetected.AddListener(LaunchWave);
        }
        private void OnDestroy()
        {
            OnPlayerDetected.RemoveListener(LaunchWave);
        }

        private void LaunchWave()
        {
            if (_hasBeenLaunched == false)
            {
                _waveData.Center = new Vector2(transform.position.x, transform.position.z);
                Waves.LaunchLinearWave(_waveData);
                _hasBeenLaunched = true;
            }
        }

        #region GUI

#if UNITY_EDITOR
        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.white;

            //cube
            Vector3 cubeSize = new Vector3(_waveData.EndWidth, _waveData.Amplitude, _waveData.Distance);
            Vector3 startCubePosition = transform.position + new Vector3(0, 0, cubeSize.z / 2);
            //Gizmos.DrawWireCube(startCubePosition, cubeSize);
            
            //points
            float distanceBetweenPointsStart = _waveData.StartWidth / _waveData.NumberOfPoints;
            float distanceBetweenPointsEnd = _waveData.EndWidth / _waveData.NumberOfPoints;
            for (int i = 0; i <= _waveData.NumberOfPoints; i++)
            {
                Vector3 startPosition = startCubePosition + new Vector3(-((_waveData.NumberOfPoints/2)*distanceBetweenPointsStart) + distanceBetweenPointsStart*i, 
                    0, -cubeSize.z/2);
                Vector3 endPosition = startCubePosition + new Vector3(-((_waveData.NumberOfPoints/2)*distanceBetweenPointsEnd) + distanceBetweenPointsEnd*i, 
                    0, cubeSize.z / 2);

                Vector3 rotatedStart = MathTools.RotatePointAroundCenter(transform.position, _waveData.Angle, startPosition);
                rotatedStart = new Vector3(rotatedStart.x, transform.position.y - cubeSize.y/2, rotatedStart.z);
                Vector3 rotatedEnd = MathTools.RotatePointAroundCenter(transform.position, _waveData.Angle, endPosition);
                rotatedEnd = new Vector3(rotatedEnd.x, transform.position.y - cubeSize.y/2, rotatedEnd.z);
                
                Gizmos.DrawLine(rotatedStart,rotatedEnd);
            }
            
            //amplitudeCurve
            float distanceBetweenAmplitudePoints = _waveData.Distance / (int)_waveData.Duration/2;
            for (int i = 0; i <= (int)_waveData.Duration*2; i++)
            {
                float percent = (float)i / (float)(int)_waveData.Duration/2;
                float y = -cubeSize.y / 2 + _waveData.AmplitudeCurve.Evaluate(percent) * _waveData.Amplitude;
                float z = -cubeSize.z / 2 + distanceBetweenAmplitudePoints * i;
                Vector3 startPosition = startCubePosition + new Vector3(-cubeSize.x/2,y, z);
                startPosition = new Vector3(startPosition.x, y, startPosition.z);
                Vector3 endPosition = startCubePosition + new Vector3(cubeSize.x/2, y, z);
                endPosition = new Vector3(endPosition.x, y, endPosition.z);

                Vector3 rotatedStart = MathTools.RotatePointAroundCenter(transform.position, _waveData.Angle, startPosition);
                Vector3 rotatedEnd = MathTools.RotatePointAroundCenter(transform.position, _waveData.Angle, endPosition);
                Gizmos.DrawLine(rotatedStart,rotatedEnd);
            }
        }
#endif

        #endregion
    }

    [Serializable]
    public struct LinearWave
    {
        [ReadOnly] public Vector2 Center;
        public float Distance;
        public float StartWidth;
        public float EndWidth;
        public float Amplitude;
        [Range(0,360)] public float Angle;
        public float Duration;
        public AnimationCurve AmplitudeCurve;
        public int NumberOfPoints;
    }
}
