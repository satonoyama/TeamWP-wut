using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
public abstract class EnemyAttack : MobAttack
{
    [SerializeField] protected EnemyStatus status;

    protected int executionIndex = 0;     // 攻撃実行リストのインデックス
    protected int atkListIndex = 0;       // 実行リストのインデックス

    protected virtual float GetAttackPower(EnemyAtkColliderMap[] atkCollisions)
    {
        var atkPow = 0.0f;

        for (int i = 0; i < atkCollisions.Length; i++)
        {
            if (!atkCollisions[i].collider.enabled) { continue; }

            atkPow = atkCollisions[i].power;
            break;
        }

        return atkPow;
    }

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

    protected virtual void InitAttackColliders(EnemyAtkColliderMap[] atkColliders, int i, string atkName, string useAtkName = "Attack")
    {
        atkColliders[i].attackName = atkName;
        atkColliders[i].collider.enabled = false;
        atkColliders[i].atkPossibleCollider.enabled = false;

        // 最初に使用する攻撃が持っているColliderを判定可能にする
        if (!useAtkName.Equals(atkName)) { return; }

        atkColliders[i].atkPossibleCollider.enabled = true;
    }

    // 攻撃可能な状態であれば攻撃を行う
    public virtual void AttackIfPossible()
    {
        if (!status.CanAttack()) { return; }

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible();
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
        status.GetWeakPoint.OnCollisionEnableFinished();
    }

    protected virtual void OnAttackColliderStart(EnemyAtkColliderMap[] atkColliders, string useAtkName = "Attack")
    {
        for (int i = 0; i < atkColliders.Length; i++)
        {
            if (!useAtkName.Equals(atkColliders[i].attackName)) { continue; }

            atkColliders[i].collider.enabled = true;
            cooldownCounter = atkColliders[i].cooldown;
        }
    }

    public override void OnAttackFinished()
    {
        base.OnAttackFinished();

        status.GoToNormalStateIfPossible();
    }

    // 次に使用する攻撃以外のプレイヤー侵入判定器を判定しないようにする
    protected virtual void FinishedAttackColliders(EnemyAtkColliderMap[] atkColliders, string nextAtkName)
    {
        for (int i = 0; i < atkColliders.Length; i++)
        {
            atkColliders[i].collider.enabled = false;
            atkColliders[i].atkPossibleCollider.enabled = false;

            if (!nextAtkName.Equals(atkColliders[i].attackName)) { continue; }

            atkColliders[i].atkPossibleCollider.enabled = true;
        }
    }

    public abstract class EnemyAtkColliderMap : AttackColliderMap
    {
        public Collider atkPossibleCollider;
    }
}
