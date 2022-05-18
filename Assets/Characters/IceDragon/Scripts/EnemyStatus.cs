using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// �G�̏�ԊǗ��X�N���v�g
[RequireComponent(typeof(EnemyWeakPoint))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
public class EnemyStatus : MobStatus
{
    private enum ActionState
    {
        eNone,
        eScream,
        eIdle,
        eMove,
        eAttack,
        eGetHit,
        eDie
    }
    private ActionState actionState = ActionState.eNone;

    private EnemyWeakPoint weakPoint;
    private NavMeshAgent agent;

    private bool isFly = false;

    public bool IsScream => actionState == ActionState.eScream;
    public bool CanMove => actionState == ActionState.eMove;
    public bool IsFly => isFly;

    protected override void Start()
    {
        base.Start();

        weakPoint = GetComponent<EnemyWeakPoint>();
        agent = GetComponent<NavMeshAgent>();

        // �퓬�J�n���̃A�j���[�V�����ɑJ��
        OnScream();
    }

    private void Update()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
    }

    public void OnScream()
    {
        actionState = ActionState.eScream;
        animator.SetTrigger("Scream");
    }

    public void OnMove()
    {
        actionState = ActionState.eMove;
        agent.isStopped = false;
    }

    public void OnMoveFinished()
    {
        actionState = ActionState.eNone;
        agent.isStopped = true;
    }

    public void OnFly()
    {
        isFly = true;
        animator.SetTrigger("TakeOff");
    }

    public void OnFlyFinished()
    {
        isFly = false;
        animator.SetTrigger("Landing");
    }

    public void OnGetHit()
    {
        if(actionState == ActionState.eGetHit) { return; }

        actionState = ActionState.eGetHit;
        animator.SetTrigger("GetHit");
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);

        if(weakPoint.IsExecution)
        {
            weakPoint.Damage(damage);

            // �_���[�W���󂯂����ʁA��_��HP�������Ȃ����ꍇ�͋���
            if(!weakPoint.IsColliderEnable())
            {
                OnGetHit();
                weakPoint.OnCollisionEnableFinished();
            }
        }
    }

    public override void GoToAttackStateIfPossible(string name = "Attack")
    {
        base.GoToAttackStateIfPossible(name);
        weakPoint.OnCollisionEnable(name);
    }

    protected override void OnDie()
    {
        base.OnDie();

        OnMoveFinished();

        // TODO : ���Ƃŏ��ł�����Ƃ��ɍH�v������
        //        ���͂Ƃ肠�����A�T�b��ɏ��������ɂ��Ă���
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
