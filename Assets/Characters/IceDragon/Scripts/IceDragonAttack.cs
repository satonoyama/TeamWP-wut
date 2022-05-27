using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
  
    // 使おうとしている攻撃名取得
    public string GetUseAtkName() => AttackNameList(executionList[executionIndex].attackList[atkListIndex]);

    [SerializeField] private IceDragonAtkColliderMap[] iceDragonAtkColliders;
    [SerializeField] private AttackExecutionList[] executionList;

    // 攻撃アニメーションのトリガー用の文字列配列
    private readonly string[] attackNameList = { "Bite", "WingClaw", "Breath", "FlyBreath" };

    protected override void Start()
    {
        base.Start();

        // 最初に使用する攻撃名
        string useAtkName = AttackNameList(executionList[executionIndex].attackList[atkListIndex]);

        for (int i = 0; i < iceDragonAtkColliders.Length; i++)
        {
            string atkName = AttackNameList(iceDragonAtkColliders[i].useAttackName);
            InitAttackColliders(iceDragonAtkColliders, i, atkName, useAtkName);
        }
    }

    public override void AttackIfPossible()
    {
        if (!status.CanAttack()) { return; }

        string useAtkName = AttackNameList(executionList[executionIndex].attackList[atkListIndex]);

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible(useAtkName);
    }

    public override void OnAttackStart()
    {
        string useAtkName = AttackNameList(executionList[executionIndex].attackList[atkListIndex]);
        OnAttackColliderStart(iceDragonAtkColliders, useAtkName);

        status.GetWeakPoint.OnCollisionEnableFinished();
    }

    public override void OnHitAttack(Collider collider)
    {
        if (isHit) { return; }

        isHit = true;

        var targetMob = collider.GetComponent<PlayerStatus>();

        if (!targetMob) { return; }

        targetMob.Damage(GetAttackPower(iceDragonAtkColliders));
    }

    public override void OnAttackFinished()
    {
        // 選択していたリストが最後まで到達した
        if (++atkListIndex >= executionList[executionIndex].attackList.Length)
        {
            atkListIndex = 0;

            executionIndex = Random.Range(0, executionList.Length);
        }

        string useAtkName = AttackNameList(executionList[executionIndex].attackList[atkListIndex]);
        FinishedAttackColliders(iceDragonAtkColliders, useAtkName);

        base.OnAttackFinished();
    }

    [Serializable]
    public class IceDragonAtkColliderMap : AttackColliderMap
    {
        public AttackNameEnum useAttackName;
    }

    [Serializable]
    public class AttackExecutionList
    {
        public AttackNameEnum[] attackList;
    }
}
