using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    public Checkpoint CurrentChekpoint;

    private void Awake()
    {
        Instance = this;
    }
}
