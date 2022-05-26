using UnityEngine;

//[RequireComponent(typeof(PlayerStatus))]
[RequireComponent(typeof(EnemyStatus))]
public abstract class EnemyAttack : MonoBehaviour
{
    [SerializeField] private AttackColliderMap[] attackColliders;
    [SerializeField] protected EnemyStatus status;

    protected int executionIndex = 0;     // �U�����s���X�g�̃C���f�b�N�X
    protected int atkListIndex = 0;       // ���s���X�g�̃C���f�b�N�X
    [SerializeField] protected float cooldownCounter = 0.0f;
    protected bool isHit = false;

    protected virtual void SetAttackName(AttackColliderMap[] atkColliders, int i, string name)
    {
        attackColliders[i].attackName = name;
    }

    protected virtual float GetAttackPower(AttackColliderMap[] atkCollisions)
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

    protected virtual void Start()
    {
        cooldownCounter = 0.0f;

        status = GetComponent<EnemyStatus>();
    }

    protected virtual void Update()
    {
        if(!status.CanAttack()) { return; }

        cooldownCounter -= Time.deltaTime;
    }

    protected virtual void InitAttackColliders(AttackColliderMap[] atkColliders, int i, string atkName, string useAtkName = "Attack")
    {
        atkColliders[i].attackName = atkName;
        atkColliders[i].collider.enabled = false;
        atkColliders[i].atkPossibleCollider.enabled = false;

        if(atkColliders[i].trail)
        {
            atkColliders[i].trail.enabled = false;
        }

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

    public virtual void OnAttackStart()
    {
        OnAttackColliderStart(attackColliders);

        status.GetWeakPoint.OnCollisionEnableFinished();
    }

    protected virtual void OnAttackColliderStart(AttackColliderMap[] atkColliders, string useAtkName = "Attack")
    {
        for (int i = 0; i < atkColliders.Length; i++)
        {
            if (!useAtkName.Equals(atkColliders[i].attackName)) { continue; }

            atkColliders[i].collider.enabled = true;
            cooldownCounter = atkColliders[i].cooldown;

            if(atkColliders[i].particle)
            {
                atkColliders[i].particle.Play();
            }

            if (atkColliders[i].trail)
            {
                atkColliders[i].trail.enabled = true;
            }
        }
    }

    public virtual void OnHitAttack(Collider collider)
    {
        if (isHit) { return; }

        isHit = true;

        var targetMob = collider.GetComponent<PlayerStatus>();

        if (!targetMob) { return; }

        targetMob.Damage(1);

        //player.Damage(GetAttackPower(attackColliders));
        //Debug.Log("Hit!!");
    }

    public virtual void OnAttackFinished()
    {
        isHit = false;
        
        status.GoToNormalStateIfPossible();
    }

    // ���Ɏg�p����U���ȊO�̃v���C���[�N�������@�𔻒肵�Ȃ��悤�ɂ���
    protected virtual void FinishedAttackColliders(AttackColliderMap[] atkColliders, string nextAtkName)
    {
        for (int i = 0; i < atkColliders.Length; i++)
        {
            atkColliders[i].collider.enabled = false;
            atkColliders[i].atkPossibleCollider.enabled = false;

            if (atkColliders[i].particle)
            {
                atkColliders[i].particle.Stop();
            }

            if (atkColliders[i].trail)
            {
                atkColliders[i].trail.enabled = false;
            }

            if (!nextAtkName.Equals(atkColliders[i].attackName)) { continue; }

            atkColliders[i].atkPossibleCollider.enabled = true;
        }
    }

    public abstract class AttackColliderMap
    {
        public Collider collider;
        public Collider atkPossibleCollider;
        public ParticleSystem particle = null;
        public TrailRenderer trail = null;
        public float power = 1.0f;
        public float cooldown = 1.0f;
        [HideInInspector] public string attackName;
    }
}
