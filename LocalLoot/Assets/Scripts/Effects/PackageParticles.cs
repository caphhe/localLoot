using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageParticles : MonoBehaviour
{
    [Header("Components")]
    public ParticleSystem shineParticles = default;
    public ParticleSystem backgroundParticles = default;

    public void StartShineParticles()
    {
        shineParticles.Play();
    }

    public void EmitShineParticles(int amount)
    {
        shineParticles.Emit(amount);
    }

    public void EmitBackgroundParticles(int amount)
    {
        backgroundParticles.Emit(amount);
    }
}
