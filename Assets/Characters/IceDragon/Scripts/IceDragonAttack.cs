using System;
using UnityEngine;

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
  
    [SerializeField] private IceDragonAtkColliderMap[] nearAtkList;
    [SerializeField] private IceDragonAtkColliderMap[] middleAtkList;
    [SerializeField] private IceDragonAtkColliderMap[] longDistAtkList;

    // �U���A�j���[�V�����̃g���K�[�p�̕�����z��
    private readonly string[] attackNameList = { "Bite", "WingClaw", "Breath", "FlyBreath" };

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < nearAtkList.Length; i++)
        {
            nearAtkList[i].attackName = AttackNameList(nearAtkList[i].useAttackName);
        }
        InitAttackColliders(DistantStateEnum.eNear, nearAtkList);

        for(int i = 0; i < middleAtkList.Length; i++)
        {
            middleAtkList[i].attackName = AttackNameList(middleAtkList[i].useAttackName);
        }
        InitAttackColliders(DistantStateEnum.eMiddle, middleAtkList);

        for (int i = 0; i < longDistAtkList.Length; i++)
        {
            longDistAtkList[i].attackName = AttackNameList(longDistAtkList[i].useAttackName);
        }
        InitAttackColliders(DistantStateEnum.eLong, longDistAtkList);

        SelectUseAttack();
    }

    [Serializable]
    public class IceDragonAtkColliderMap : EnemyAtkColliderMap
    {
        public AttackNameEnum useAttackName;
    }
}
