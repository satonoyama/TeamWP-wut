using UnityEngine;

public class EffectStatus : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle = null;

    public void OnPlayParticle()
    {
        if (!particle) { return; }

        particle.Play();
    }

    public void OnStopParticle()
    {
        if (!particle) { return; }

        particle.Stop();
    }
}
