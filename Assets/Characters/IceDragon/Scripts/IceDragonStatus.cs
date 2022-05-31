
public class IceDragonStatus : EnemyStatus
{
    private bool isLanding = false;

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

    public override void GoToNormalStateIfPossible()
    {
        if (isLanding) { return; }

        base.GoToNormalStateIfPossible();
    }
}
