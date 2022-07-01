using System;
using UnityEngine;

public class IceDragonStatus : EnemyStatus
{
    [SerializeField] private AudioSource flyingSE = null;
    [SerializeField] private AudioSource landingSE = null;

    private bool isLanding = true;
    private bool isDistCnecked = false;

    private readonly string[] getHitAnimeTriggerNameList = { "ShootingDown", "GetHit"};

    private string GetHitAnimeTriggerName(int id) => getHitAnimeTriggerNameList[id];

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

    public void OnTakeOff()
    {
        isLanding = false;
        getHitAnimationName = GetHitAnimeTriggerName(Convert.ToInt32(isLanding));
    }

    public void OnLanding()
    {
        isLanding = true;
        getHitAnimationName = GetHitAnimeTriggerName(Convert.ToInt32(isLanding));
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
        if (!isLanding) { return; }

        base.GoToNormalStateIfPossible();
    }
}
