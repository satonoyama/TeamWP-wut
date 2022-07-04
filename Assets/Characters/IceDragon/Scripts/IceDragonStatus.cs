using System;
using UnityEngine;

public class IceDragonStatus : EnemyStatus
{
    public enum IceDragonGetHitState
    {
        eGetHit,
        eShootingDown
    }
    private IceDragonGetHitState getHitState = IceDragonGetHitState.eGetHit;

    [SerializeField] private AudioSource[] getHitSEList;
    [SerializeField] private AudioSource flyingSE = null;
    [SerializeField] private AudioSource landingSE = null;

    private bool isLanding = true;
    private bool isDistCnecked = false;

    private readonly string[] getHitAnimeTriggerNameList = { "GetHit", "ShootingDown" };

    private string GetHitAnimeTriggerName(int id) => getHitAnimeTriggerNameList[id];

    public void OnPlayGetHitSE(IceDragonGetHitState id)
    {
        if (!getHitSEList[(int)id] ||
             getHitSEList[(int)id].isPlaying) { return; }

        getHitSEList[(int)id].Play();
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

    public void OnTakeOff()
    {
        getHitState = IceDragonGetHitState.eShootingDown;
        isLanding = false;
        getHitAnimationName = GetHitAnimeTriggerName((int)getHitState);
    }

    public void OnLanding()
    {
        getHitState = IceDragonGetHitState.eGetHit;
        isLanding = true;
        getHitAnimationName = GetHitAnimeTriggerName((int)getHitState);
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
