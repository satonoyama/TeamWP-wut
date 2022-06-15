using System;
using UnityEngine;

public class IceDragonStatus : EnemyStatus
{
    private bool isLanding = false;
    private bool isDistCnecked = false;

    private readonly string[] getHitAnimeTriggerNameList = { "ShootingDown", "GetHit"};

    private string GetHitAnimeTriggerName(int id) => getHitAnimeTriggerNameList[id];

    protected override void Update()
    {
        base.Update();
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
        // « ‚±‚ê‚ª‚È‚¢‚Æ’…—¤’†‚ÉŸ‚ÌUŒ‚‚ğ‚µ‚Ä‚µ‚Ü‚¤
        if (isLanding) { return; }

        base.GoToNormalStateIfPossible();
    }
}
