using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// 敵の状態管理スクリプト
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
public class EnemyStatus : MobStatus
{
    private NavMeshAgent agent;

    private bool canMove = false;
    private bool isFly = false;

    // 死亡実験用の為、後で消す
    private float destroyTime = 10.0f;
    private float timeCounter = 0.0f;

    public bool CanMove => canMove;
    public bool IsFly => isFly;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();

        // 戦闘開始時のアニメーションに遷移
        //isFly = true;
        //animator.SetTrigger("BattleStart");
    }

    private void Update()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);

        // 死亡実験
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

        // TODO : あとで消滅させるときに工夫を入れる
        //        今はとりあえず、５秒後に消すだけにしている
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
