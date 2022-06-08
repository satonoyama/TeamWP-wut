using System;
using UnityEngine;

[RequireComponent(typeof(IceDragonAttack))]
public class IceDragonWeakPoint : EnemyWeakPoint
{
    [SerializeField] private IceDragonWPColliderMap[] weakPoints;

    protected override void Start()
    {
        base.Start();

        IceDragonAttack iceDragonAttack = GetComponent<IceDragonAttack>();

        string attackName;

        for (int i = 0; i < weakPoints.Length; i++)
        {
            attackName = iceDragonAttack.AttackNameList(weakPoints[i].userAttackName);

            InitWeakPointColliders(attackName, weakPoints[i]);
        }
    }

    [Serializable]
    public class IceDragonWPColliderMap : WeakPointColliderMap
    {
        public IceDragonAttack.AttackNameEnum userAttackName;
    }
}
