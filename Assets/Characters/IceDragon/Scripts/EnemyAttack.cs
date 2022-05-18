using System;
using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private AttackColliderMap[] attackColliders;
    [SerializeField] private AttackExecutionOrder[] executionOrders;
    [SerializeField] private float cooldownCounter = 0.0f;

    private EnemyStatus status;
    private int executionIndex = 0;     // 攻撃実行リストのインデックス
    private int orderIndex = 0;         // 実行順リストのインデックス
    private bool isHit = false;

    void Start()
    {
        cooldownCounter = 0.0f;

        for (int i = 0; i < attackColliders.Length; i++)
        {
            attackColliders[i].collider.enabled = false;
            attackColliders[i].atkPossibleCollider.enabled = true;

            // 最初に使用する攻撃が持っているCollider以外を判定不可にする
            string name = executionOrders[executionIndex].attackExecutionOrders[orderIndex];

            if (!name.Equals(attackColliders[i].attackName))
            {
                attackColliders[i].atkPossibleCollider.enabled = false;
            }
        }

        status = GetComponent<EnemyStatus>();
    }

    private void Update()
    {
        if(!status.IsAttackable) { return; }

        cooldownCounter -= Time.deltaTime;
    }

    // 攻撃可能な状態であれば攻撃を行う
    public void AttackIfPossible()
    {
        if (!status.IsAttackable || status.IsScream) { return; }

        string name = executionOrders[executionIndex].attackExecutionOrders[orderIndex];

        Debug.Log(name + "攻撃を実行！！");

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible(name);
    }

    // 攻撃対象が範囲内にいる間呼ばれる
    public void OnAttackStay(Collider collider)
    {
        if (cooldownCounter > 0.0f) { return; }

        AttackIfPossible();
    }

    public void OnAttackStart()
    {
        for(int i = 0; i < attackColliders.Length; i++)
        {
            string name = executionOrders[executionIndex].attackExecutionOrders[orderIndex];
            if (!name.Equals(attackColliders[i].attackName)) { continue; }

            attackColliders[i].collider.enabled = true;
        }
    }

    public void OnHitAttack(Collider collider)
    {
        if (isHit) { return; }

        isHit = true;

        for (int i = 0; i < attackColliders.Length; i++)
        {
            if (!attackColliders[i].collider.enabled) { continue; }

            // player.Damage(attackColliders[i].power);
            //float atkPow = attackColliders[i].power;
            break;
        }

        // TODO : プレイヤーにダメージを与える処理を追加する
        Debug.Log("Hit!!");
    }

    public void OnAttackFinished()
    {
        // 選択していたリストが最後まで到達した
        if(++orderIndex >= executionOrders[executionIndex].attackExecutionOrders.Length)
        {
            executionIndex++;
            orderIndex = 0;
        }

        // 実行順リストが最後まで到達した
        if(executionIndex >= executionOrders.Length)
        {
            executionIndex = 0;
        }

        for (int i = 0; i < attackColliders.Length; i++)
        {
            attackColliders[i].collider.enabled = false;
            attackColliders[i].atkPossibleCollider.enabled = false;
            cooldownCounter = attackColliders[i].cooldown;

            string name = executionOrders[executionIndex].attackExecutionOrders[orderIndex];
            if (!name.Equals(attackColliders[i].attackName)) { continue; }

            attackColliders[i].atkPossibleCollider.enabled = true;
        }

        isHit = false;
        
        status.GoToNormalStateIfPossible();
        status.OnMove();
    }

    [Serializable]
    public class AttackColliderMap
    {
        public Collider collider;
        public Collider atkPossibleCollider;
        public string attackName;
        public float power = 1.0f;
        public float cooldown = 5.0f;
    }

    [Serializable]
    public class AttackExecutionOrder
    {
        public string[] attackExecutionOrders;
    }
}
