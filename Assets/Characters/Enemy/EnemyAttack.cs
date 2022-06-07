using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyStatus))]
public abstract class EnemyAttack : MobAttack
{
    [SerializeField] protected EnemyStatus status;
    [SerializeField] protected float cooldownCounter = 0.0f; // ( �m�F�p )���inspector���炢����Ȃ�����

    protected Dictionary<bool, EnemyAtkColliderMap[]> useAttackList = new();
    protected string useAtkName = "";
    protected int useAtkListIndex = 0;
    protected bool isLongDist = false;

    public string GetUseAtkName => useAtkName;

    protected virtual float GetAttackPower => useAttackList[isLongDist][useAtkListIndex].power;

    protected override void Start()
    {
        base.Start();

        status = GetComponentInChildren<EnemyStatus>();
    }

    protected override void Update()
    {
        if(!status.CanAttack()) { return; }

        CooldownCount();
    }

    protected virtual void InitAttackColliders(bool isLongDist, EnemyAtkColliderMap[] atkColliders)
    {
        useAttackList.Add(isLongDist, atkColliders);

        for(int i = 0; i < atkColliders.Length; i++)
        {
            useAttackList[isLongDist][i].attackName = atkColliders[i].attackName;
            useAttackList[isLongDist][i].collider.enabled = false;
            useAttackList[isLongDist][i].atkPossibleCollider.enabled = false;
        }

        SelectUseAttack();
    }

    protected virtual void SelectUseAttack()
    {
        isLongDist = status.IsLongDist;

        useAtkListIndex = Random.Range(0, useAttackList[isLongDist].Length);
        useAtkName = useAttackList[isLongDist][useAtkListIndex].attackName;

        // ���Ɏg���U���̐N��������L����
        useAttackList[isLongDist][useAtkListIndex].atkPossibleCollider.enabled = true;
    }

    protected virtual void CooldownCount()
    {
        if (cooldownCounter < 0.0f) { return; }

        cooldownCounter -= Time.deltaTime;
    }

    protected virtual void ColliderFinished()
    {
        for (int i = 0; i < useAttackList.Values.Count; i++)
        {
            if(!useAttackList[isLongDist][i].collider.enabled) { continue; }

            useAttackList[isLongDist][i].collider.enabled = false;
        }

        useAttackList[isLongDist][useAtkListIndex].atkPossibleCollider.enabled = false;
    }

    // �U���\�ȏ�Ԃł���΍U�����s��
    public virtual void AttackIfPossible()
    {
        if (!status.CanAttack()) { return; }

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible(useAtkName);
    }

    // �U���Ώۂ��͈͓��ɂ���ԌĂ΂��
    public virtual void OnAttackStay(Collider collider)
    {
        if (cooldownCounter > 0.0f) { return; }

        AttackIfPossible();
    }

    // �U���Ώۂ��̂ɐG��邭�炢���ߋ����ɂ���ꍇ�ȂǂŎg�p����
    public virtual void OnForcedAttackStay(Collider collider)
    {
        cooldownCounter = 0.0f;
        AttackIfPossible();
    }

    public override void OnAttackStart()
    {
        // ���ꂩ��g���U���Ɠ������O�������Ă���U���p�R���C�_�[��L����
        for (int i = 0; i < useAttackList.Values.Count; i++)
        {
            bool isEquals = useAttackList[isLongDist][useAtkListIndex].
                attackName.Equals(useAttackList[isLongDist][i].attackName);

            if (!isEquals) { continue; }

            useAttackList[isLongDist][i].collider.enabled = true;
        }

        cooldownCounter = useAttackList[isLongDist][useAtkListIndex].cooldown;

        status.GetWeakPoint.OnCollisionEnableFinished();
    }

    public override void OnHitAttack(Collider collider)
    {
        base.OnHitAttack(collider);

        var targetMob = collider.GetComponent<PlayerStatus>();

        if (!targetMob) { return; }

        targetMob.Damage(GetAttackPower);
    }

    public override void OnAttackFinished()
    {
        base.OnAttackFinished();

        ColliderFinished();

        SelectUseAttack();

        status.GoToNormalStateIfPossible();
    }

    public abstract class EnemyAtkColliderMap : AttackColliderMap
    {
        public Collider atkPossibleCollider;
        public float cooldown = 1.0f;
    }
}
