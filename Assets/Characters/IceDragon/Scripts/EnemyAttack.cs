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

    // UŒ‚‰Â”\‚Èó‘Ô‚Å‚ ‚ê‚ÎUŒ‚‚ğs‚¤
    public void AttackIfPossible()
    {
        if(!status.IsAttackable) { return; }

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible(attackColliders.First().attackName);
    }

    // UŒ‚‘ÎÛ‚ªUŒ‚”ÍˆÍ“à‚É“ü‚Á‚½‚ÉŒÄ‚Î‚ê‚é
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
        // TODO : ƒvƒŒƒCƒ„[‚Éƒ_ƒ[ƒW‚ğ—^‚¦‚éˆ—‚ğ’Ç‰Á‚·‚é
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
