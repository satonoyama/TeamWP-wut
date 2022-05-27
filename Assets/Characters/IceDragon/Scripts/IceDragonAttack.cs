using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class IceDragonAttack : EnemyAttack
{
    public enum AttackNameEnum
    {
        eBite,          // ���݂�
        eWingClaw,      // ����
        eBreath,        // �u���X
        eFlyBreath,     // ��s�u���X
    }

    public string AttackNameList(AttackNameEnum attack) => attackNameList[(int)attack];
  
    // �g�����Ƃ��Ă���U�����擾
    public string GetUseAtkName() => AttackNameList(executionList[executionIndex].attackList[atkListIndex]);

    [SerializeField] private IceDragonAtkColliderMap[] iceDragonAtkColliders;
    [SerializeField] private AttackExecutionList[] executionList;

    // �U���A�j���[�V�����̃g���K�[�p�̕�����z��
    private readonly string[] attackNameList = { "Bite", "WingClaw", "Breath", "FlyBreath" };

    protected override void Start()
    {
        base.Start();

        // �ŏ��Ɏg�p����U����
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
        // �I�����Ă������X�g���Ō�܂œ��B����
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
