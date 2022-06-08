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
    protected float defaultAgentSpeed = 1.0f;
    [SerializeField] protected float fastAgentSpeed = 1.0f;

    [SerializeField] protected MovementController target;
    protected bool isNearDist = false;
    protected bool isMiddleDist = false;

    [SerializeField] protected float animationWaitTime = 0.0f;
    [SerializeField] protected float slowlyAnimationSpeed = 0.0f;
    protected float defaultAnimationSpeed = 0.0f;

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

    public bool IsNearDist => isNearDist;

    public bool IsMiddleDist => isMiddleDist;

    public bool IsLongDist => !isNearDist && !isMiddleDist;

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

        defaultAgentSpeed = agent.speed;
        defaultAnimationSpeed = animator.speed;

        // 戦闘開始時のアニメーションに遷移
        OnScream();
    }

    protected override void Update()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
    }

    public void OnNearDistColliderStay()
    {
        isNearDist = true;
    }

    public void OnNearDistColliderExit()
    {
        isNearDist = false;
    }

    public void OnMiddleDistColliderStay()
    {
        if (isNearDist) { return; }

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
        if(actionState == ActionState.eGetHit) { return; }

        actionState = ActionState.eGetHit;
        animator.SetTrigger("GetHit");
    }

    protected override void OnDie()
    {
        base.OnDie();

        OnMoveFinished();

        GetWeakPoint.OnWeakPointFinished();

        WeakPointContainer.Instance.AllRemove();

        // TODO : Tansition to Clear Scene
    }

    public void OnPauseAnimation()
    {
        animator.speed = slowlyAnimationSpeed;
        StartCoroutine(AnimationWaitCoroutine());
    }

    private IEnumerator AnimationWaitCoroutine()
    {
        yield return new WaitForSeconds(animationWaitTime);

        animator.speed = defaultAnimationSpeed;
        GetWeakPoint.OnWeakPointFinished();
    }
}
