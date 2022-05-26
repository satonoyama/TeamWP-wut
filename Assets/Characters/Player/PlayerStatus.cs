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

        Debug.Log("���ꂽ...");
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);

        Debug.Log("HP : " + Hp);
    }
}
