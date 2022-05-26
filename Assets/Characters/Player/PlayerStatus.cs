using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MobStatus
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDie()
    {
        if(state == StateEnum.eDie) { return; }

        //base.OnDie();

        Debug.Log("‚â‚ç‚ê‚½...");
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);

        Debug.Log("HP : " + Hp);
    }
}
