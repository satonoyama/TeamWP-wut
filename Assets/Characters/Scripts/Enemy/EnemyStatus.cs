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
    protected bool isDown = false;
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

    protected bool IsGetHit => actionState == ActionState.eGetHit;

    protected bool IsDown => isDown;

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
        if (!IsGetHit || !IsDown) { return; }

        downTimeCounter -= 1.0f * Time.deltaTime;

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
        GoToNormalStateIfPossible();

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
        agent.isStopped = true;

        if (getHitAnimationName != "GetHit")
        {
            downTimeCounter = downTime;
            isDown = true;
        }

        animator.SetTrigger(getHitAnimationName);

        GetWeakPoint.OnWeakPointFinished();
    }

    protected void OnGetUp()
    {
        if (!IsGetHit) { return; }

        isDown = false;

        animator.SetTrigger("GetUp");
        actionState = ActionState.eNone;
    }

    public virtual void OnDamage(GameObject magicObj)
    {
        var magic = magicObj.GetComponent<MagicalFX.MagicInfo>();
        if (!magic) { return; }

        float damageVal = magic.Power;

        if(GetWeakPoint.IsExecution)
        {
            GetWeakPoint.OnHitMagic(magicObj);

            if((hp - damageVal) > 0.0f &&
                GetWeakPoint.Hp() <= 0.0f)
            {
                OnGetHit();
            }
        }

        Damage(damageVal);
    }

    public override void Damage(float damage)
    {
        float damageVal = damage;

        if (GetWeakPoint.IsHitWeakPoint || IsDown)
        {
            damageVal *= 2.0f;
        }

        Debug.Log("Deal damage:" + damageVal);

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
