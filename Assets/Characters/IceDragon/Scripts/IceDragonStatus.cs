using UnityEngine;

public class IceDragonStatus : EnemyStatus
{
    private bool isLanding = false;
    private bool isDistCnecked = false;

    protected override void Update()
    {
        base.Update();
    }

    public override void OnScream()
    {
        isLanding = false;

        base.OnScream();
        GoToNormalStateIfPossible();
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
