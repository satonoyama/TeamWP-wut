using System;
using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private AttackColliderMap[] attackColliders;
    [SerializeField] private AttackExecutionOrder[] executionOrders;
    [SerializeField] private float cooldownCounter = 0.0f;

    private EnemyStatus status;
    private int executionIndex = 0;     // �U�����s���X�g�̃C���f�b�N�X
    private int orderIndex = 0;         // ���s�����X�g�̃C���f�b�N�X
    private bool isHit = false;

    void Start()
    {
        cooldownCounter = 0.0f;

        for (int i = 0; i < attackColliders.Length; i++)
        {
            attackColliders[i].collider.enabled = false;
            attackColliders[i].atkPossibleCollider.enabled = true;

            // �ŏ��Ɏg�p����U���������Ă���Collider�ȊO�𔻒�s�ɂ���
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

    // �U���\�ȏ�Ԃł���΍U�����s��
    public void AttackIfPossible()
    {
        if (!status.IsAttackable || status.IsScream) { return; }

        string name = executionOrders[executionIndex].attackExecutionOrders[orderIndex];

        Debug.Log(name + "�U�������s�I�I");

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible(name);
    }

    // �U���Ώۂ��͈͓��ɂ���ԌĂ΂��
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

        // TODO : �v���C���[�Ƀ_���[�W��^���鏈����ǉ�����
        Debug.Log("Hit!!");
    }

    public void OnAttackFinished()
    {
        // �I�����Ă������X�g���Ō�܂œ��B����
        if(++orderIndex >= executionOrders[executionIndex].attackExecutionOrders.Length)
        {
            executionIndex++;
            orderIndex = 0;
        }

        // ���s�����X�g���Ō�܂œ��B����
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
