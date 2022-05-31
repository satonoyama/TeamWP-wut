using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
public abstract class EnemyAttack : MobAttack
{
    [SerializeField] protected EnemyStatus status;

    protected int executionIndex = 0;     // �U�����s���X�g�̃C���f�b�N�X
    protected int atkListIndex = 0;       // ���s���X�g�̃C���f�b�N�X

    protected virtual float GetAttackPower(EnemyAtkColliderMap[] atkCollisions)
    {
        var atkPow = 0.0f;

        for (int i = 0; i < atkCollisions.Length; i++)
        {
            if (!atkCollisions[i].collider.enabled) { continue; }

            atkPow = atkCollisions[i].power;
            break;
        }

        return atkPow;
    }

    protected override void Start()
    {
        base.Start();

        status = GetComponentInChildren<EnemyStatus>();
    }

    protected override void Update()
    {
        if(!status.CanAttack()) { return; }

        CooldownCount();
    }

    protected virtual void InitAttackColliders(EnemyAtkColliderMap[] atkColliders, int i, string atkName, string useAtkName = "Attack")
    {
        atkColliders[i].attackName = atkName;
        atkColliders[i].collider.enabled = false;
        atkColliders[i].atkPossibleCollider.enabled = false;

        // �ŏ��Ɏg�p����U���������Ă���Collider�𔻒�\�ɂ���
        if (!useAtkName.Equals(atkName)) { return; }

        atkColliders[i].atkPossibleCollider.enabled = true;
    }

    // �U���\�ȏ�Ԃł���΍U�����s��
    public virtual void AttackIfPossible()
    {
        if (!status.CanAttack()) { return; }

        status.OnMoveFinished();
        status.GoToAttackStateIfPossible();
    }

    // �U���Ώۂ��͈͓��ɂ���ԌĂ΂��
    public virtual void OnAttackStay(Collider collider)
    {
        if (cooldownCounter > 0.0f) { return; }

        AttackIfPossible();
    }

    // �U���Ώۂ��̂ɐG��邭�炢���ߋ����ɂ���ꍇ�ȂǂŎg�p����
    public virtual void OnForcedAttackStay(Collider collider)
    {
        cooldownCounter = 0.0f;
        AttackIfPossible();
    }

    public override void OnAttackStart()
    {
        status.GetWeakPoint.OnCollisionEnableFinished();
    }

    protected virtual void OnAttackColliderStart(EnemyAtkColliderMap[] atkColliders, string useAtkName = "Attack")
    {
        for (int i = 0; i < atkColliders.Length; i++)
        {
            if (!useAtkName.Equals(atkColliders[i].attackName)) { continue; }

            atkColliders[i].collider.enabled = true;
            cooldownCounter = atkColliders[i].cooldown;
        }
    }

    public override void OnAttackFinished()
    {
        base.OnAttackFinished();

        status.GoToNormalStateIfPossible();
    }

    // ���Ɏg�p����U���ȊO�̃v���C���[�N�������𔻒肵�Ȃ��悤�ɂ���
    protected virtual void FinishedAttackColliders(EnemyAtkColliderMap[] atkColliders, string nextAtkName)
    {
        for (int i = 0; i < atkColliders.Length; i++)
        {
            atkColliders[i].collider.enabled = false;
            atkColliders[i].atkPossibleCollider.enabled = false;

            if (!nextAtkName.Equals(atkColliders[i].attackName)) { continue; }

            atkColliders[i].atkPossibleCollider.enabled = true;
        }
    }

    public abstract class EnemyAtkColliderMap : AttackColliderMap
    {
        public Collider atkPossibleCollider;
    }
}
