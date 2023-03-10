using UI;
using UnityEngine;

namespace GPEs.Checkpoint
{
    public class Checkpoint : PlayerTriggerManager
    {
        [SerializeField] private string _zoneName;
        public Transform TargetRespawnTransform;
    
        [Header("References"), SerializeField] private ZoneManager _zoneManager;
        [SerializeField] private ParticleSystem _activationParticles;
        [SerializeField] private AudioClip _activationClip;
    
        private bool _hasBeenUsed;

        private void Start()
        {
            OnPlayerDetected.AddListener(SetCheckPoint);
        }
        private void OnDestroy()
        {
            OnPlayerDetected.RemoveListener(SetCheckPoint);
        }

        public void SetCheckPoint()
        {
            if (_hasBeenUsed == false)
            {
                CheckpointManager.Instance.CurrentCheckpoint = this;
                _hasBeenUsed = true;
            
                _activationParticles.Play();
                SoundManager.Instance.PlaySound(_activationClip);
                _zoneManager.ShowZone(_zoneName);
            }
        }
    
#if UNITY_EDITOR

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (TargetRespawnTransform != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(TargetRespawnTransform.position,Vector3.one*0.5f);
            }
        }

#endif
    }
}
