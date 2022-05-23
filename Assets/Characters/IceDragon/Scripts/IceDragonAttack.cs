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

    [SerializeField] private IceDragonAtkColliderMap[] iceDragonAtkColliders;
    [SerializeField] private AttackExecutionList[] executionList;

    // 攻撃アニメーションのトリガー用の文字列配列
    private readonly string[] attackNameList = { "Bite", "WingClaw", "Breath", "FlyBreath" };

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < iceDragonAtkColliders.Length; i++)
        {
            string atkName = AttackNameList(iceDragonAtkColliders[i].useAttackName);
            string useAtkName = AttackNameList(executionList[executionIndex].attackList[atkListIndex]);
            InitAttackColliders(iceDragonAtkColliders, i, atkName, useAtkName);
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void AttackIfPossible()
    {
        if (!status.IsAttackable) { return; }

        string useAtkName = AttackNameList(executionList[executionIndex].attackList[atkListIndex]);

        Debug.Log(useAtkName + "攻撃を実行！！");

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible(useAtkName);
    }

    public override void OnAttackStay(Collider collider)
    {
        base.OnAttackStay(collider);
    }

    public override void OnAttackStart()
    {
        string useAtkName = AttackNameList(executionList[executionIndex].attackList[atkListIndex]);
        OnAttackColliderStart(iceDragonAtkColliders, useAtkName);
    }

    public override void OnHitAttack(Collider collider)
    {
        if (isHit) { return; }

        isHit = true;

        // これを使ってプレイヤーにダメージを与える
        //GetAttackPower(iceDragonAtkColliders);
        Debug.Log("Hit!!");
    }

    public override void OnAttackFinished()
    {
        // 選択していたリストが最後まで到達した
        if (++atkListIndex >= executionList[executionIndex].attackList.Length)
        {
            atkListIndex = 0;

            executionIndex = Random.Range(0, executionList.Length);

            Debug.Log(executionIndex + "番目のリストが選択されました。");
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
