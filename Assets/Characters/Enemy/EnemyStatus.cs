using UnityEngine;
using UnityEngine.AI;

// 敵の状態管理スクリプト
[RequireComponent(typeof(EnemyWeakPoint))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
public class EnemyStatus : MobStatus
{
    protected enum ActionState
    {
        eNone,
        eScream,
        eIdle,
        eMove,
        eAttack,
        eGetHit,
        eDie
    }
    protected ActionState actionState = ActionState.eNone;
    protected NavMeshAgent agent;

    [SerializeField] protected MovementController target;
    [SerializeField] protected bool isLongDist = false;

    [SerializeField] protected float triggerHpRate = 0.0f;   // 特殊な行動を実行するHP割合
    protected bool isExecuteSpecialBehavior = false;

    public MovementController GetTarget => target;

    public bool CanAttack()
    {
        if(!IsAttackable ||
           actionState == ActionState.eScream ||
           actionState == ActionState.eAttack) { return false; }

        return true;
    }

    public bool CanMove => actionState == ActionState.eMove;

    public bool IsLongDist => isLongDist;

    public bool IsExecuteSpecialBehavior => isExecuteSpecialBehavior;

    // 特別な行動をするHPになっているか
    protected bool CheckHp()
    {
        if (isExecuteSpecialBehavior) { return false; }

        if (Hp <= (MaxHp * triggerHpRate))
        {
            return true;
        }

        return false;
    }

    public EnemyWeakPoint GetWeakPoint { get; private set; }

    protected override void Start()
    {
        base.Start();

        GetWeakPoint = GetComponent<EnemyWeakPoint>();
        agent = GetComponent<NavMeshAgent>();

        // 戦闘開始時のアニメーションに遷移
        OnScream();
    }

    protected override void Update()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
    }

    public virtual void OnScream()
    {
        OnMoveFinished();

        actionState = ActionState.eScream;
        animator.SetTrigger("Scream");
    }

    public void OnLongDistColliderStay(Collider collider)
    {
        isLongDist = false;
    }

    public void OnLongDistColliderExit(Collider collider)
    {
        isLongDist = true;
    }

    public void OnMove()
    {
        actionState = ActionState.eMove;
        agent.isStopped = false;

        // 移動しながら攻撃をするモンスターもいるので、
        // 攻撃可能な状態であれば弱点を有効にするようにしている
        if (!CanAttack()) { return; }

        GetWeakPoint.OnCollisionEnable();
    }

    public void OnMoveFinished()
    {
        actionState = ActionState.eNone;
        agent.isStopped = true;
    }

    public void OnGetHit()
    {
        if(actionState == ActionState.eGetHit) { return; }

        actionState = ActionState.eGetHit;
        animator.SetTrigger("GetHit");
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);

        if(GetWeakPoint.IsExecution)
        {
            GetWeakPoint.Damage(damage);

            // ダメージを受けた結果、弱点のHPが無くなった場合は怯む
            if(!GetWeakPoint.IsColliderEnable())
            {
                OnGetHit();
                GetWeakPoint.OnCollisionEnableFinished();
            }
        }
    }

    protected override void OnDie()
    {
        base.OnDie();

        OnMoveFinished();

        WeakPointContainer.Instance.AllRemove();

        // TODO : Tansition to Clear Scene
    }
}
