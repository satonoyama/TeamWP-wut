using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MobStatus
{
    [SerializeField] private float maxSp = 100.0f;
    [SerializeField] private float sp = 1.0f;

    [SerializeField] private float spHealValByTime = 1.0f;  // 1�b���Ƃ�SP�̉񕜗�

    // SP�֌W
    public float MaxSp => maxSp;
    public float Sp => sp;

    protected override void Start()
    {
        base.Start();

        sp = maxSp;

        // �����p
        sp -= (MaxSp / 2.0f);

        // TODO : �Ȃ񂩒ǉ����邩���H
    }

    protected override void Update()
    {
        if (!IsAttackable) { return; }

        UpdateHealSP();
    }

    // �܂�������
    private void UpdateHealSP()
    {
        if(sp >= maxSp) { return; }

        sp += Time.deltaTime * spHealValByTime;

        Mathf.Clamp(sp, 0.0f, maxSp);
    }

    protected override void OnDie()
    {
        if(state == StateEnum.eDie) { return; }

        //base.OnDie();
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);

        //Debug.Log("HP : " + Hp);
    }
}
