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

    [SerializeField] private IceDragonAtkColliderMap[] iceDragonAtkColliders;
    [SerializeField] private AttackExecutionList[] executionList;

    // �U���A�j���[�V�����̃g���K�[�p�̕�����z��
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

        Debug.Log(useAtkName + "�U�������s�I�I");

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

        // ������g���ăv���C���[�Ƀ_���[�W��^����
        //GetAttackPower(iceDragonAtkColliders);
        Debug.Log("Hit!!");
    }

    public override void OnAttackFinished()
    {
        // �I�����Ă������X�g���Ō�܂œ��B����
        if (++atkListIndex >= executionList[executionIndex].attackList.Length)
        {
            atkListIndex = 0;

            executionIndex = Random.Range(0, executionList.Length);

            Debug.Log(executionIndex + "�Ԗڂ̃��X�g���I������܂����B");
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
