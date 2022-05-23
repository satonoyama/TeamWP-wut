using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
public abstract class EnemyAttack : MonoBehaviour
{
    [SerializeField] private AttackColliderMap[] attackColliders;
    [SerializeField] protected EnemyStatus status;

    protected int executionIndex = 0;     // 攻撃実行リストのインデックス
    protected int atkListIndex = 0;       // 実行リストのインデックス
    [SerializeField] protected float cooldownCounter = 0.0f;
    protected bool isHit = false;

    protected virtual void SetAttackName(AttackColliderMap[] atkColliders, int i, string name)
    {
        attackColliders[i].attackName = name;
    }

    protected virtual float GetAttackPower(AttackColliderMap[] atkCollisions)
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

    protected virtual void Start()
    {
        cooldownCounter = 0.0f;

        status = GetComponent<EnemyStatus>();
    }

    protected virtual void Update()
    {
        if(!status.CanAttack()) { return; }

        cooldownCounter -= Time.deltaTime;
    }

    protected virtual void InitAttackColliders(AttackColliderMap[] atkColliders, int i, string atkName, string useAtkName = "Attack")
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
        if (status.CanAttack()) { return; }

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible();
    }

    // 攻撃対象が範囲内にいる間呼ばれる
    public virtual void OnAttackStay(Collider collider)
    {
        if (cooldownCounter > 0.0f) { return; }

        AttackIfPossible();
    }

    public virtual void OnAttackStart()
    {
        OnAttackColliderStart(attackColliders);

        status.GetWeakPoint.OnCollisionEnableFinished();
    }

    protected virtual void OnAttackColliderStart(AttackColliderMap[] atkColliders, string useAtkName = "Attack")
    {
        for (int i = 0; i < atkColliders.Length; i++)
        {
            if (!useAtkName.Equals(atkColliders[i].attackName)) { continue; }

            atkColliders[i].collider.enabled = true;
            cooldownCounter = atkColliders[i].cooldown;
        }
    }

    public virtual void OnHitAttack(Collider collider)
    {
        if (isHit) { return; }

        isHit = true;

        //player.Damage(GetAttackPower(attackColliders));
        //Debug.Log("Hit!!");
    }

    public virtual void OnAttackFinished()
    {
        isHit = false;
        
        status.GoToNormalStateIfPossible();
        status.OnMove();
    }

    // 次に使用する攻撃以外のプレイヤー侵入検査機を判定しないようにする
    protected virtual void FinishedAttackColliders(AttackColliderMap[] atkColliders, string nextAtkName)
    {
        for (int i = 0; i < atkColliders.Length; i++)
        {
            atkColliders[i].collider.enabled = false;
            atkColliders[i].atkPossibleCollider.enabled = false;

            if (!nextAtkName.Equals(atkColliders[i].attackName)) { continue; }

            atkColliders[i].atkPossibleCollider.enabled = true;
        }
    }

    public abstract class AttackColliderMap
    {
        public Collider collider;
        public Collider atkPossibleCollider;
        public float power = 1.0f;
        public float cooldown = 1.0f;
        [HideInInspector] public string attackName;
    }
}
