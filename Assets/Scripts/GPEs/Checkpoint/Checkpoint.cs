using Kayak;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform TargetRespawnTransform;

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<KayakController>() != null & Input.GetKeyDown(KeyCode.M))
        {
            CheckpointManager.Instance.CurrentChekpoint = this;
        }
    }
}
