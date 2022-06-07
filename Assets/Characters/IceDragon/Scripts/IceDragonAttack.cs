using System;
using UnityEngine;

public class IceDragonAttack : EnemyAttack
{
    public enum AttackNameEnum
    {
        eBite,          // 噛みつき
        eWingClaw,      // 翼爪
        eBreath,        // ブレス
        eFlyBreath,     // 飛行ブレス
    }

    public string AttackNameList(AttackNameEnum attack) => attackNameList[(int)attack];
  
    [SerializeField] private IceDragonAtkColliderMap[] nearAtkList;
    [SerializeField] private IceDragonAtkColliderMap[] longDistAtkList;

    // 攻撃アニメーションのトリガー用の文字列配列
    private readonly string[] attackNameList = { "Bite", "WingClaw", "Breath", "FlyBreath" };

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < nearAtkList.Length; i++)
        {
            nearAtkList[i].attackName = AttackNameList(nearAtkList[i].useAttackName);
        }
        InitAttackColliders(false, nearAtkList);

        for (int i = 0; i < longDistAtkList.Length; i++)
        {
            longDistAtkList[i].attackName = AttackNameList(longDistAtkList[i].useAttackName);
        }
        InitAttackColliders(true, longDistAtkList);
    }

    [Serializable]
    public class IceDragonAtkColliderMap : EnemyAtkColliderMap
    {
        public AttackNameEnum useAttackName;
    }
}
