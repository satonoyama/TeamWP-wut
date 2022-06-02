using UnityEngine;

public class IceDragonStatus : EnemyStatus
{
    private bool isLanding = false;

    protected override void Update()
    {
        base.Update();

        if ((hp -= Time.deltaTime) <= 0.0f)
        {
            OnDie();
        }
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

    public override void GoToNormalStateIfPossible()
    {
        if (isLanding) { return; }

        base.GoToNormalStateIfPossible();
    }
}
