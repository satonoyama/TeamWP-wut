using UnityEngine;

// Mob( 動くオブジェクト, MoveObjectの略 )の状態管理スクリプト
public abstract class MobStatus : MonoBehaviour
{
    protected enum StateEnum
    {
        eNormal,
        eAttack,
        eDie
    }

    // 移動可能かどうか
    public bool IsMovabe => state == StateEnum.eNormal;

    // 攻撃可能かどうか
    public bool IsAttackable => state == StateEnum.eNormal;

    // HP関係
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

        // TODO : ライフゲージの表示終了などを追加する
    }

    public virtual void Damage(int damage)
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

    public void GoToNormalStateIfPossible()
    {
        if(state == StateEnum.eDie) { return; }
        state = StateEnum.eNormal;
    }
}
