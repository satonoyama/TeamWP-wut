using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// �G�̏�ԊǗ��X�N���v�g
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
public class EnemyStatus : MobStatus
{
    private NavMeshAgent agent;

    private bool canMove = false;
    private bool isFly = false;

    // ���S�����p�ׁ̈A��ŏ���
    private float destroyTime = 10.0f;
    private float timeCounter = 0.0f;

    public bool CanMove => canMove;
    public bool IsFly => isFly;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();

        // �퓬�J�n���̃A�j���[�V�����ɑJ��
        //isFly = true;
        //animator.SetTrigger("BattleStart");
    }

    private void Update()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);

        // ���S����
        timeCounter += Time.deltaTime;
        if(timeCounter >= destroyTime) { OnDie(); }
    }

    public void OnMove()
    {
        canMove = true;
        agent.isStopped = false;
    }

    public void OnMoveFinished()
    {
        canMove = false;
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
