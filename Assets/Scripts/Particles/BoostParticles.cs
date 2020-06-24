using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostParticles : MonoBehaviour
{
    // Start is called before the first frame update
    public void SetBoostParticlesActive(bool activeState)
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem)
        {
            if (particleSystem.isPlaying == activeState)
            {
                return;
            }
            if (activeState)
            {
                particleSystem.Play();
            }
            else
            {
                particleSystem.Stop();
            }
        }
    }


}
