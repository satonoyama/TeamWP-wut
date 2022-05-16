using System;
using System.Linq;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private AttackColliderMap[] attackColliders;

    private EnemyStatus status;

    void Start()
    {
        status = GetComponent<EnemyStatus>();
    }

    // 攻撃可能な状態であれば攻撃を行う
    public void AttackIfPossible()
    {
        if(!status.IsAttackable) { return; }

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible(attackColliders.First().attackName);
    }

    // 攻撃対象が攻撃範囲内に入った時に呼ばれる
    public void OnAttackRangeEnter(Collider collider)
    {
        AttackIfPossible();
    }

    public void OnAttackStart()
    {
        attackColliders.First().attackCollider.enabled = true;
    }

    public void OnHitAttack(Collider collider)
    {
        // TODO : プレイヤーにダメージを与える処理を追加する
    }

    public void OnAttackFinished()
    {
        attackColliders.First().attackCollider.enabled = false;
        StartCoroutine(CooldownCoroutine());
    }

    public IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        status.GoToNormalStateIfPossible();
        status.OnMove();
    }

    [Serializable]
    public class AttackColliderMap
    {
        public Collider attackCollider;
        public string attackName;
        public float attackPower = 1.0f;
    }
}
