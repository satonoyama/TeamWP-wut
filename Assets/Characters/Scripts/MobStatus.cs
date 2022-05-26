using UnityEngine;

// Mob( �����I�u�W�F�N�g, MoveObject�̗� )�̏�ԊǗ��X�N���v�g
public abstract class MobStatus : MonoBehaviour
{
    protected enum StateEnum
    {
        eNormal,
        eAttack,
        eDie
    }

    // �ړ��\���ǂ���
    public bool IsMovabe => state == StateEnum.eNormal;

    // �U���\���ǂ���
    public bool IsAttackable => state == StateEnum.eNormal;
    
    // HP�֌W
    public float MaxHp => maxHp;
    public float Hp => hp;

    [SerializeField] private float maxHp = 100;
    protected Animator animator;
    protected StateEnum state = StateEnum.eNormal;
    protected float hp;

    protected virtual void Start()
    {
        hp = maxHp;
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void OnDie()
    {
        if (state == StateEnum.eDie) { return; }

        state = StateEnum.eDie;
        animator.SetTrigger("Die");

        // TODO : ���C�t�Q�[�W�̕\���I���Ȃǂ�ǉ�����
    }

    public virtual void Damage(float damage)
    {
        if(state == StateEnum.eDie) { return; }

        hp -= damage;
        if(hp > 0) { return; }

        OnDie();
    }

    public virtual void GoToAttackStateIfPossible(string name = "Attack")
    {
        if(!IsAttackable) { return; }

        state = StateEnum.eAttack;
        animator.SetTrigger(name);
    }

    public virtual void GoToNormalStateIfPossible()
    {
        if(state == StateEnum.eDie) { return; }
        state = StateEnum.eNormal;
    }
}