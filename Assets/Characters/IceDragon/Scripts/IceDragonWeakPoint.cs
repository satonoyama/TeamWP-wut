using System;
using UnityEngine;

[RequireComponent(typeof(IceDragonAttack))]
public class IceDragonWeakPoint : EnemyWeakPoint
{
    [SerializeField] private IceDragonAttack attack;
    [SerializeField] private IceDragonWPColliderMap[] iceDragonWPColliders;

    public override bool IsColliderEnable()
    {
        return GetIsColliderEnable(iceDragonWPColliders);
    }

    public override float MaxHp()
    {
        return GetMaxHp(iceDragonWPColliders);
    }

    public override float Hp()
    {
        return GetHp(iceDragonWPColliders);
    }

    protected override void Start()
    {
        attack = GetComponent<IceDragonAttack>();

        for (int i = 0; i < iceDragonWPColliders.Length; i++)
        {
            InitWeakPointColliders(iceDragonWPColliders, i, attack.AttackNameList(iceDragonWPColliders[i].userAttackName));
        }
    }

    public override void OnCollisionEnable()
    {
        if (isExecution) { return; }

        // Žg‚¨‚¤‚Æ‚µ‚Ä‚¢‚éUŒ‚–¼Ý’è
        useAtkName = attack.GetUseAtkName();

        OnWPCollisionEnable(iceDragonWPColliders);

        isExecution = true;
    }

    public override void OnCollisionEnableFinished()
    {
        OnWPCollisionEnableFinished(iceDragonWPColliders);

        isExecution = false;
    }

    public override void Damage(float dmg)
    {
        OnDamage(iceDragonWPColliders, dmg);
    }

    [Serializable]
    public class IceDragonWPColliderMap : WeakPointColliderMap
    {
        public IceDragonAttack.AttackNameEnum userAttackName;
    }
}
