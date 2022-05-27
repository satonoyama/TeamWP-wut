using UnityEngine;

public abstract class EffectStatus : MonoBehaviour
{
    [SerializeField] protected ParticleSystem particle = null;
    [SerializeField] protected GameObject generatePosObject = null;

    protected virtual void Update()
    {
        if(!particle.isPlaying || !generatePosObject) { return; }

        var pos = generatePosObject.transform.position;
        particle.transform.position = pos;
    }

    protected void OnPlayParticle()
    {
        if (!particle) { return; }

        particle.Play();
    }

    protected void OnStopParticle()
    {
        if (!particle) { return; }

        particle.Stop();
    }
}
