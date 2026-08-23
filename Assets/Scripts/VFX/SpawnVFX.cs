using System;
using UnityEngine;

public class SpawnVFX : MonoBehaviour
{
    public event Action<SpawnVFX> Finished;

    public void Finish()
    {
        Finished?.Invoke(this);
    }
}