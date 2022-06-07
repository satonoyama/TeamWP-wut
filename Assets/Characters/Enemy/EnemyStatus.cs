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

    [SerializeField] protected MovementController target;
    [SerializeField] protected float distLength = 0.0f;

    [SerializeField] protected float triggerHpRate = 0.0f;   // ����ȍs�������s����HP����
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

    public bool IsExecuteSpecialBehavior => isExecuteSpecialBehavior;

    // ���ʂȍs��������HP�ɂȂ��Ă��邩
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

        // �퓬�J�n���̃A�j���[�V�����ɑJ��
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

    public void OnMove()
    {
        actionState = ActionState.eMove;
        agent.isStopped = false;

        // �ړ����Ȃ���U�������郂���X�^�[������̂ŁA
        // �U���\�ȏ�Ԃł���Ύ�_��L���ɂ���悤�ɂ��Ă���
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

            // �_���[�W���󂯂����ʁA��_��HP�������Ȃ����ꍇ�͋���
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
