using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyStatus))]
public abstract class EnemyAttack : MobAttack
{
    [SerializeField] protected EnemyStatus status;
    [SerializeField] protected float cooldownCounter = 0.0f; // ( 確認用 )後でinspectorからいじれなくする

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

        // 次に使う攻撃の侵入判定器を有効に
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
        // これから使う攻撃と同じ名前が入っている攻撃用コライダーを有効に
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
