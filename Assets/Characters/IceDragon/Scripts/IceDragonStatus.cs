using UnityEngine;

public class IceDragonStatus : EnemyStatus
{
    private bool isLanding = false;
    private bool isDistCnecked = false;

    protected override void Update()
    {
        base.Update();

        //if ((hp -= Time.deltaTime) <= 0.0f)
        //{
        //    OnDie();
        //}
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
        if (isLanding) { return; }

        base.GoToNormalStateIfPossible();
    }
}
