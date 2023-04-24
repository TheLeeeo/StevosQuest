using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoseMobileProximityHandler : MonoBehaviour
{
    private void OnTriggerEnter2D()
    {
        NoseMobileController._instance.StopSpawnPointParticles();
        NoseMobileController._instance.PlaySpawnParticles();

        NoseMobileController._instance.Activate();

        this.enabled = false;
    }
}
