using Kayak;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private string _zoneName;
    public Transform TargetRespawnTransform;
    
    [Header("References"), SerializeField] private ZoneManager _zoneManager;
    [SerializeField] private ParticleSystem _activationParticles;
    [SerializeField] private AudioClip _activationClip;
    
    private bool _hasBeenUsed;

    private void OnTriggerEnter(Collider other)
    {
        KayakController kayakController = other.gameObject.GetComponent<KayakController>();
        if (kayakController != null && _hasBeenUsed == false)
        {
            CheckpointManager.Instance.CurrentCheckpoint = this;
            _hasBeenUsed = true;
            
            _activationParticles.Play();
            SoundManager.Instance.PlaySound(_activationClip);
            _zoneManager.ShowZone(_zoneName);
        }
    }
}
