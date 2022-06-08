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
    [SerializeField] protected float cooldownCounter = 0.0f; // ( 確認用 )後でinspectorからいじれなくする

    protected Dictionary<DistantStateEnum, EnemyAtkColliderMap[]> useAttackList = new();
    protected string useAtkName = "";
    protected int useAtkListIndex = 0;

    public bool IsEndCooldown => cooldownCounter <= 0.0f;

    public string GetUseAtkName => useAtkName;

    protected virtual float GetAttackPower => useAttackList[distState][useAtkListIndex].power;

    protected override void Start()
    {
        base.Start();

        status = GetComponentInChildren<EnemyStatus>();
    }

    protected override void Update()
    {
        if(!status.CanAttack()) { return; }

        CooldownCount();

        bool canStartOutOfRange = IsEndCooldown && useAttackList[distState][useAtkListIndex].canStartOutOfRange;

        if (canStartOutOfRange)
        {
            AttackIfPossible();
        }
    }

    protected virtual void InitAttackColliders(DistantStateEnum dist, EnemyAtkColliderMap[] atkColliders)
    {
        distState = dist;

        useAttackList.Add(distState, atkColliders);

        for(int i = 0; i < atkColliders.Length; i++)
        {
            useAttackList[distState][i].attackName = atkColliders[i].attackName;
            useAttackList[distState][i].atkPossibleCollider.enabled = false;

            int maxRange = useAttackList[distState][i].collider.Length;
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

        // 次に使う攻撃の侵入判定器を有効に
        useAttackList[distState][useAtkListIndex].atkPossibleCollider.enabled = true;
    }

    protected virtual void CooldownCount()
    {
        if (IsEndCooldown) { return; }

        cooldownCounter -= Time.deltaTime;
    }

    protected virtual void ColliderFinished()
    {
        for (int i = 0; i < useAttackList[distState].Length; i++)
        {
            int maxRange = useAttackList[distState][i].collider.Length;
            for (int num = 0; num < maxRange; num++)
            {
                if (!useAttackList[distState][i].collider[num].enabled) { continue; }

                useAttackList[distState][i].collider[num].enabled = false;
            }
        }

        useAttackList[distState][useAtkListIndex].atkPossibleCollider.enabled = false;
    }

    // 攻撃可能な状態であれば攻撃を行う
    public virtual void AttackIfPossible()
    {
        if (!status.CanAttack()) { return; }

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible(useAtkName);
    }

    // 攻撃対象が範囲内にいる間呼ばれる
    public virtual void OnAttackStay(Collider collider)
    {
        if (cooldownCounter > 0.0f) { return; }

        AttackIfPossible();
    }

    // 攻撃対象が体に触れるくらい至近距離にいる場合などで使用する
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

        SelectUseAttack();

        status.GetWeakPoint.OnWeakPointFinished();
        status.GoToNormalStateIfPossible();
    }

    public abstract class EnemyAtkColliderMap : AttackColliderMap
    {
        public Collider atkPossibleCollider;
        public float cooldown = 1.0f;
        public bool canStartOutOfRange = false;
    }
}
