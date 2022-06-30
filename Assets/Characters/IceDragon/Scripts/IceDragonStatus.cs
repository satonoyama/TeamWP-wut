using System;
using UnityEngine;

public class IceDragonStatus : EnemyStatus
{
    [SerializeField] private AudioSource flyingSE = null;
    [SerializeField] private AudioSource landingSE = null;

    private bool isLanding = false;
    private bool isDistCnecked = false;

    private readonly string[] getHitAnimeTriggerNameList = { "ShootingDown", "GetHit"};

    private string GetHitAnimeTriggerName(int id) => getHitAnimeTriggerNameList[id];

    protected override void Update()
    {
        base.Update();
    }

    public void OnPlayFlyingSE(float vol = 1.0f)
    {
        if (!flyingSE) { return; }

        flyingSE.volume = vol;
        flyingSE.Play();
    }

    public void OnPlayLandingSE()
    {
        if (!landingSE) { return; }

        landingSE.Play();
    }

    public override void OnScream()
    {
        getHitAnimationName = GetHitAnimeTriggerName(Convert.ToInt32(isLanding));

        isLanding = false;

        base.OnScream();
        GoToNormalStateIfPossible();
    }

    public void OnTakeOff()
    {
        isLanding = false;
        getHitAnimationName = GetHitAnimeTriggerName(Convert.ToInt32(isLanding));
    }

    public void OnLanding()
    {
        isLanding = true;
    }

    public void OnCheckLongDist()
    {
        if (isDistCnecked) { return; }

        isDistCnecked = true;

        OnTracingSpeedUp();
    }

    public void OnGettingCloser()
    {
        if (!isDistCnecked) { return; }

        isDistCnecked = false;

        OnTracingSpeedDefault();

        animator.SetTrigger("GettingCloser");
    }

    public override void GoToNormalStateIfPossible()
    {
        // Å´ Ç±ÇÍÇ™Ç»Ç¢Ç∆íÖó§íÜÇ…éüÇÃçUåÇÇÇµÇƒÇµÇ‹Ç§
        if (isLanding) { return; }

        base.GoToNormalStateIfPossible();
    }
}
