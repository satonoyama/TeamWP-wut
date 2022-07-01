using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyStatus))]
public abstract class EnemyAttack : MobAttack
{
    protected enum DistantStateEnum
    {
        eNear,
        eMiddle,
        eLong
    }
    protected DistantStateEnum distState = DistantStateEnum.eNear;

    [SerializeField] protected EnemyStatus status;
    [SerializeField] protected float cooldownCounter = 0.0f; // ( ämîFóp )å„Ç≈inspectorÇ©ÇÁÇ¢Ç∂ÇÍÇ»Ç≠Ç∑ÇÈ

    [Serializable]
    protected struct ProbabilitySelectInfo
    {
        [SerializeField] [Range(0.0f, 1.0f)] public float probability;
        [SerializeField]  public float cooldown;
        [HideInInspector] public float originalVal;
        [HideInInspector] public bool isSelect;
    }
    [SerializeField] protected ProbabilitySelectInfo probSelectInfo;

    protected Dictionary<DistantStateEnum, EnemyAtkColliderMap[]> useAttackList = new();
    protected string useAtkName = "";
    protected int useAtkListIndex = 0;

    [SerializeField] protected float atkReSelectTime = 1.0f;
    protected float reSelectCounter = 0.0f;

    protected virtual bool IsSelectedByProbability()
    {
        var probability = Random.Range(0.0f, 1.0f);

        if (probability <= probSelectInfo.probability)
        {
            probSelectInfo.isSelect = true;

            probSelectInfo.probability /= 2.0f;

            return true;
        }

        return false;
    }

    protected virtual bool IsEnableCollider()
    {
        bool result = false;

        int maxRange = useAttackList[distState][useAtkListIndex].collider.Length;
        for (int i = 0; i < maxRange; i++)
        {
            if(!useAttackList[distState][useAtkListIndex].collider[i].enabled) { continue; }

            result = true;
            break;
        }

        return result;
    }

    protected virtual bool IsDisableAfterHit =>
    useAttackList[distState][useAtkListIndex].isDisableAfterHit;

    protected virtual bool IsEndCooldown => cooldownCounter <= 0.0f;

    public string GetUseAtkName => useAtkName;

    protected virtual float GetAttackPower => useAttackList[distState][useAtkListIndex].power;

    protected override void Start()
    {
        base.Start();

        status = GetComponentInChildren<EnemyStatus>();

        reSelectCounter = atkReSelectTime;
    }

    protected override void Update()
    {
        if(!status.CanAttack()) { return; }

        CooldownCount();

        AtkReSelectCount();

        OnAtkStartOutOfRange();
    }

    protected virtual void InitAttackColliders(DistantStateEnum dist, EnemyAtkColliderMap[] atkColliders)
    {
        distState = dist;

        useAttackList.Add(distState, atkColliders);

        for(int i = 0; i < atkColliders.Length; i++)
        {
            useAttackList[distState][i].attackName = atkColliders[i].attackName;

            if(useAttackList[distState][i].atkPossibleCollider)
            {
                useAttackList[distState][i].atkPossibleCollider.enabled = false;
            }

            int maxRange = useAttackList[distState][i].collider.Length;

            if(maxRange == 0) { continue; }

            for (int num = 0; num < maxRange; num++)
            {
                useAttackList[distState][i].collider[num].enabled = false;
            }
        }
    }

    public virtual void SelectUseAttack()
    {
        ColliderFinished();

        if (status.IsNearDist) { distState = DistantStateEnum.eNear; }        
        else if(status.IsMiddleDist) { distState = DistantStateEnum.eMiddle; }
        else { distState = DistantStateEnum.eLong; }

        useAtkListIndex = Random.Range(0, useAttackList[distState].Length);
        useAtkName = useAttackList[distState][useAtkListIndex].attackName;

        // éüÇ…égÇ§çUåÇÇÃêNì¸îªíËäÌÇóLå¯Ç…
        if (!useAttackList[distState][useAtkListIndex].atkPossibleCollider) { return; }
        useAttackList[distState][useAtkListIndex].atkPossibleCollider.enabled = true;
    }

    protected virtual void ReSelectUseAttack()
    {
        status.GetWeakPoint.OnWeakPointFinished();

        SelectUseAttack();
        
        status.GetWeakPoint.OnWeakPointStart();
    }

    protected virtual void ColliderFinished()
    {
        if (!IsEnableCollider()) { return; }

        int maxRange = useAttackList[distState][useAtkListIndex].collider.Length;
        for (int num = 0; num < maxRange; num++)
        {
            if (!useAttackList[distState][useAtkListIndex].collider[num].enabled) { continue; }

            useAttackList[distState][useAtkListIndex].collider[num].enabled = false;
        }

        if (!useAttackList[distState][useAtkListIndex].atkPossibleCollider) { return; }
        useAttackList[distState][useAtkListIndex].atkPossibleCollider.enabled = false;
    }

    protected virtual void CooldownCount()
    {
        if (IsEndCooldown) { return; }

        cooldownCounter -= Time.deltaTime;
    }

    protected virtual void AtkReSelectCount()
    {
        if (!IsEndCooldown) { return; }

        reSelectCounter -= 1.0f * Time.deltaTime;

        if (reSelectCounter <= 0.0f)
        {
            reSelectCounter = atkReSelectTime;

            ReSelectUseAttack();
        }
    }

    protected virtual void OnAtkStartOutOfRange()
    {
        if (!IsEndCooldown) { return; }

        bool canStartOutOfRange = probSelectInfo.isSelect ||
             useAttackList[distState][useAtkListIndex].canStartOutOfRange;

        if (!canStartOutOfRange) { return; }

        probSelectInfo.isSelect = false;
        cooldownCounter = probSelectInfo.cooldown;

        AttackIfPossible();
    }

    // çUåÇâ¬î\Ç»èÛë‘Ç≈Ç†ÇÍÇŒçUåÇÇçsÇ§
    protected virtual void AttackIfPossible()
    {
        if (!status.CanAttack()) { return; }

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible(useAtkName);
    }

    // çUåÇëŒè€Ç™îÕàÕì‡Ç…Ç¢ÇÈä‘åƒÇŒÇÍÇÈ
    public virtual void OnAttackStay(Collider collider)
    {
        if (cooldownCounter > 0.0f) { return; }

        AttackIfPossible();
    }

    // çUåÇëŒè€Ç™ëÃÇ…êGÇÍÇÈÇ≠ÇÁÇ¢éäãﬂãóó£Ç…Ç¢ÇÈèÍçáÇ»Ç«Ç≈égópÇ∑ÇÈ
    public virtual void OnForcedAttackStay(Collider collider)
    {
        cooldownCounter = 0.0f;
        AttackIfPossible();
    }

    public override void OnAttackStart()
    {
        int maxRange = useAttackList[distState][useAtkListIndex].collider.Length;
        for (int i = 0; i < maxRange; i++)
        {
            useAttackList[distState][useAtkListIndex].collider[i].enabled = true;
        }

        cooldownCounter = useAttackList[distState][useAtkListIndex].cooldown;
    }

    public override void OnPlayAttackSound()
    {
        int maxSeNum = useAttackList[distState][useAtkListIndex].se.Length;
        for (int i = 0; i < maxSeNum; i++)
        {
            if (!useAttackList[distState][useAtkListIndex].se[i]) { continue; }

            useAttackList[distState][useAtkListIndex].se[i].Play();
        }
    }

    public override void OnHitAttack(Collider collider)
    {
        base.OnHitAttack(collider);

        var targetMob = collider.GetComponent<PlayerStatus>();

        if (!targetMob) { return; }

        targetMob.Damage(GetAttackPower);

        if(IsDisableAfterHit)
        {
            ColliderFinished();
        }
    }

    public override void OnAttackFinished()
    {
        base.OnAttackFinished();

        status.GetWeakPoint.OnWeakPointFinished();

        SelectUseAttack();
    }

    public virtual void OnAttackFinishedByGetHit()
    {
        cooldownCounter = useAttackList[distState][useAtkListIndex].cooldown;

        OnAttackFinished();
    }

    public abstract class EnemyAtkColliderMap : AttackColliderMap
    {
        public Collider atkPossibleCollider = null;
        public float cooldown = 1.0f;
        public bool canStartOutOfRange = false;
        public bool isDisableAfterHit = false;
    }
}
