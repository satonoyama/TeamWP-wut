using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MobStatus
{
    [SerializeField] private float maxSp = 100.0f;
    [SerializeField] private float sp = 1.0f;
    [SerializeField] private float spHealValByTime = 1.0f;  // 1�b���Ƃ�SP�̉񕜗�

    [SerializeField] private float dmgWaitTime = 0.0f;
    private float dmgWaitCounter = 0.0f;

    // SP�֌W
    public float MaxSp => maxSp;
    public float Sp => sp;

    private bool IsDamaged => dmgWaitCounter > 0.0f;

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

    private void FixedUpdate()
    {
        if (!IsDamaged) { return; }

        dmgWaitCounter -= Time.deltaTime;

        if(dmgWaitCounter <= 0.0f) { dmgWaitCounter = 0.0f; }
    }

    // �܂�������
    private void UpdateHealSP()
    {
        if(sp >= maxSp) { return; }

        sp += Time.deltaTime * spHealValByTime;

        if(sp >= MaxSp) { sp = MaxSp; }
    }

    //! ������Ȃ̂ŉ��ǂ��邱��
    public void AddSP(float val)
    {
        sp += val;
    }

    protected override void OnDie()
    {
        if(state == StateEnum.eDie) { return; }

        //base.OnDie();
    }

    public override void Damage(float damage)
    {
        if (IsDamaged) { return; }

        base.Damage(damage);

        dmgWaitCounter = dmgWaitTime;
    }
}
