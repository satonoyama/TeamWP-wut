using UnityEngine;

public class IceDragonMove : EnemyMove
{
    [SerializeField] private float descentSpeed = 0.01f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if(status.IsFly)
        {
            moveVelocity.y -= descentSpeed;

            status.transform.position = moveVelocity;
            return;
        }

        base.Update();
    }
}
