using System.Collections;
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

    [SerializeField] protected float fastAgentSpeed = 1.0f;
    protected float defaultAgentSpeed = 1.0f;
    protected float defaultAnimationSpeed = 0.0f;

    [SerializeField] protected MovementController target;
    [SerializeField] protected float downTime = 1.0f;
    protected float downTimeCounter = 0.0f;
    protected string getHitAnimationName = "GetHit";
    protected bool isNearDist = false;
    protected bool isMiddleDist = false;

    public MovementController GetTarget => target;

    public bool CanAttack()
    {
        if(!IsAttackable || IsGetHit ||
           actionState == ActionState.eScream ||
           actionState == ActionState.eAttack) { return false; }

        return true;
    }

    public bool CanMove => actionState == ActionState.eMove;

    public bool IsGetHit => actionState == ActionState.eGetHit;

    public bool IsNearDist => isNearDist;

    public bool IsMiddleDist => isMiddleDist;

    public EnemyWeakPoint GetWeakPoint { get; private set; }

    protected override void Start()
    {
        base.Start();

        GetWeakPoint = GetComponent<EnemyWeakPoint>();
        agent = GetComponent<NavMeshAgent>();

        defaultAgentSpeed = agent.speed;
        defaultAnimationSpeed = animator.speed;

        // 戦闘開始時のアニメーションに遷移
        OnScream();
    }

    protected override void Update()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);

        DownTimeCount();
    }

    protected virtual void DownTimeCount()
    {
        if (!IsGetHit) { return; }

        downTimeCounter -= 1.0f * Time.deltaTime;

        Debug.Log("now Time : " + downTimeCounter);

        if(downTimeCounter <= 0.0f)
        {
            OnGetUp();
        }
    }

    public void OnNearDistColliderEnter()
    {
        isNearDist = true;
        isMiddleDist = false;
    }

    public void OnNearDistColliderExit()
    {
        isNearDist = false;
        isMiddleDist = true;
    }

    public void OnMiddleDistColliderEnter()
    {
        isNearDist = false;
        isMiddleDist = true;
    }

    public void OnMiddleDistColliderExit()
    {
        isNearDist = false;
        isMiddleDist = false;
    }

    public void OnTracingSpeedUp()
    {
        agent.speed = fastAgentSpeed;
    }

    public void OnTracingSpeedDefault()
    {
        agent.speed = defaultAgentSpeed;
    }

    public virtual void OnScream()
    {
        OnMoveFinished();

        actionState = ActionState.eScream;
        animator.SetTrigger("Scream");
    }

    public void OnMove()
    {
        actionState = ActionState.eMove;
        agent.isStopped = false;

        // 移動しながら攻撃をするモンスターもいるので、
        // 攻撃可能な状態であれば弱点を有効にするようにしている
        if (!CanAttack()) { return; }

        GetWeakPoint.OnWeakPointStart();
    }

    public void OnMoveFinished()
    {
        actionState = ActionState.eNone;
        agent.isStopped = true;
    }

    public void OnGetHit()
    {
        if(IsGetHit) { return; }

        actionState = ActionState.eGetHit;

        if(getHitAnimationName != "GetHit")
        {
            downTimeCounter = downTime;
        }

        animator.SetTrigger(getHitAnimationName);

        GetWeakPoint.OnWeakPointFinished();
    }

    protected void OnGetUp()
    {
        if (!IsGetHit) { return; }

        animator.SetTrigger("GetUp");
        actionState = ActionState.eNone;
    }

    public virtual void OnDamage(Collider collider, float damage)
    {
        if(GetWeakPoint.IsExecution)
        {
            GetWeakPoint.OnHitPlayerAttack(collider);

            if(GetWeakPoint.Hp() <= 0.0f)
            {
                OnGetHit();
            }
        }

        Damage(damage);
    }

    public override void Damage(float damage)
    {
        float damageVal = damage;

        if(GetWeakPoint.IsHitWeakPoint)
        {
            damageVal *= 2.0f;
        }

        base.Damage(damageVal);
    }

    protected override void OnDie()
    {
        base.OnDie();

        OnMoveFinished();

        GetWeakPoint.OnWeakPointFinished();

        WeakPointContainer.Instance.AllRemove();
        FallingLumpContainer.Instance.AllRemove();

        // TODO : Tansition to Clear Scene
    }

    public virtual void OnSlowlyAnimation(float speed)
    {
        animator.speed = speed;
    }

    public virtual void OnAnimSpeedDefault()
    {
        animator.speed = defaultAnimationSpeed;
    }
}
