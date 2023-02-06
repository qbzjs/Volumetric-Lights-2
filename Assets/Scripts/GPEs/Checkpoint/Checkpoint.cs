using Kayak;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private ParticleSystem _activationParticles;
    [SerializeField] private AudioClip _activationClip;
    public Transform TargetRespawnTransform;
    
    private bool _hasBeenUsed;

    private void OnTriggerEnter(Collider other)
    {
        KayakController kayakController = other.gameObject.GetComponent<KayakController>();
        if (kayakController != null && _hasBeenUsed == false)
        {
            CheckpointManager.Instance.CurrentCheckpoint = this;
            _activationParticles.Play();
            SoundManager.Instance.PlaySound(_activationClip);
            _hasBeenUsed = true;
        }
    }
}
