using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// �G�̏�ԊǗ��X�N���v�g
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

    [SerializeField] protected AudioSource getUpSE = null;
    [SerializeField] protected AudioSource dieSE = null;
    [SerializeField] protected float dieSEVolumeDownVal = 1.0f;

    [SerializeField] protected float fastAgentSpeed = 1.0f;
    protected float defaultAgentSpeed = 1.0f;
    protected float defaultAnimationSpeed = 0.0f;

    [SerializeField] protected MovementController target;
    [SerializeField] protected float downTime = 1.0f;
    [SerializeField] protected float downTimeCounter = 0.0f;
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

    public bool CanMove => actionState == 
        ActionState.eMove && !IsDie && !agent.isStopped;

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

        // �퓬�J�n���̃A�j���[�V�����ɑJ��
        OnScream();
    }

    protected override void Update()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);

        DieSEVolumeDown();
    }

    private void FixedUpdate()
    {
        if (downTimeCounter <= 0.0f) { return; }

        downTimeCounter -= 1.0f * Time.deltaTime;

        if(downTimeCounter <= 0.0f)
        {
            OnGetUp();
        }
    }

    protected virtual void DieSEVolumeDown()
    {
        if (!dieSE.isPlaying) { return; }

        dieSE.volume -= dieSEVolumeDownVal * Time.deltaTime;

        if (dieSE.volume <= 0.0f)
        {
            dieSE.Stop();
        }
    }

    public void OnPlayGetUpSE(float volume = 1.0f)
    {
        if (!getUpSE) { return; }

        getUpSE.volume = volume;
        getUpSE.Play();
    }

    public void OnPlayDieSE()
    {
        if (!dieSE) { return; }

        dieSE.Play();
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
        if (IsDown) { return; }

        GoToNormalStateIfPossible();

        actionState = ActionState.eMove;
        agent.isStopped = false;

        // �ړ����Ȃ���U�������郂���X�^�[������̂ŁA
        // �U���\�ȏ�Ԃł���Ύ�_��L���ɂ���悤�ɂ��Ă���
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

        downTimeCounter = downTime;

        actionState = ActionState.eGetHit;
        agent.isStopped = true;

        animator.SetTrigger(getHitAnimationName);

        GetWeakPoint.OnWeakPointFinished();
        EffectController.Instance.OnStopAllParticle();

        if (getHitAnimationName != "GetHit")
        {
            isDown = true;
            StartCoroutine(DownTimeCount());
        }
    }

    protected void OnGetUp()
    {
        if (!IsGetHit) { return; }

        isDown = false;
        downTimeCounter = 0.0f;

        animator.SetTrigger("GetUp");
        actionState = ActionState.eNone;
    }

    public virtual void OnDamage(MagicalFX.MagicInfo magic)
    {
        if (!magic) { return; }

        float damageVal = magic.Power;

        if(GetWeakPoint.IsExecution)
        {
            GetWeakPoint.OnHitMagic(magic);

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

        base.Damage(damageVal);
    }

    protected override void OnDie()
    {
        base.OnDie();

        OnMoveFinished();

        GetWeakPoint.OnWeakPointFinished();

        WeakPointContainer.Instance.AllRemove();
        EffectController.Instance.OnStopAllParticle();
        FallingLumpContainer.Instance.AllRemove();
    }

    public virtual void OnSlowlyAnimation(float speed)
    {
        animator.speed = speed;
    }

    public virtual void OnAnimSpeedDefault()
    {
        animator.speed = defaultAnimationSpeed;
    }

    private IEnumerator DownTimeCount()
    {
        yield return new WaitForSeconds(downTime);

        OnGetUp();
    }
}
