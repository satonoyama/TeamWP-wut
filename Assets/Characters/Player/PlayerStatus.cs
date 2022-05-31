using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MobStatus
{
    protected override void Start()
    {
        base.Start();

        // TODO : Ç»ÇÒÇ©í«â¡Ç∑ÇÈÇ©Ç‡ÅH
    }

    protected override void OnDie()
    {
        if(state == StateEnum.eDie) { return; }

        //base.OnDie();
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);

        Debug.Log("HP : " + Hp);
    }
}
