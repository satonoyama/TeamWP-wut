
public class BreathStatus : EffectStatus
{
    protected override void Update()
    {
        base.Update();

        var rot = generatePosObject.transform.rotation;
        particle.transform.rotation = rot;
    }

    public void OnPlayBreathEffect()
    {
        OnPlayParticle();
    }

    public void OnStopBreathEffect()
    {
        OnStopParticle();
    }
}
