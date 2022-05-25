using UnityEngine;

public abstract class EnemyWeakPoint : MonoBehaviour
{
    [SerializeField] private WeakPointColliderMap[] weakPointColliders;

    protected string useAtkName = "Attack";     // �Ƃ肠���� Attack �����Ă���
    protected bool isExecution = false;

    public bool IsExecution => isExecution;

    protected bool GetIsColliderEnable(WeakPointColliderMap[] weakPoints)
    {
        bool result = false;

        for (int i = 0; i < weakPoints.Length; i++)
        {
            if (weakPoints[i].collider.enabled)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    protected float GetMaxHp(WeakPointColliderMap[] weakPoints)
    {
        float maxHp = 0.0f;

        for (int i = 0; i < weakPoints.Length; i++)
        {
            if (!weakPoints[i].collider.enabled) { continue; }

            maxHp += weakPoints[i].maxHp;
        }

        return maxHp;
    }

    protected float GetHp(WeakPointColliderMap[] weakPoints)
    {
        float hp = 0.0f;

        for (int i = 0; i < weakPoints.Length; i++)
        {
            if (!weakPoints[i].collider.enabled) { continue; }

            hp += weakPoints[i].hp;
        }

        return hp;
    }

    // �P�����ł����肪�L���ɂȂ��Ă����True��Ԃ�
    public virtual bool IsColliderEnable()
    {
        return GetIsColliderEnable(weakPointColliders);
    }

    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====
    //
    // �ȉ��AUI���Ŏg���\�����������
    //
    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====

    // ���肪�L���ɂȂ��Ă��镔�ʂ̑��ő�HP�擾
    public virtual float MaxHp()
    {
        return GetMaxHp(weakPointColliders);
    }

    // ���肪�L���ɂȂ��Ă��镔�ʂ̑�HP�擾
    public virtual float Hp()
    {
        return GetHp(weakPointColliders);
    }

    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====
    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====

    protected virtual void Start()
    {
        InitWeakPointColliders(weakPointColliders);
    }

    public void InitWeakPointColliders(WeakPointColliderMap[] weakPoints, string userName = "Body")
    {
        for (int i = 0; i < weakPoints.Length; i++)
        {
            weakPoints[i].collider.enabled = false;
            weakPoints[i].hp = weakPoints[i].maxHp;
            weakPoints[i].attackName = userName;
        }
    }

    public void InitWeakPointColliders(WeakPointColliderMap[] weakPoints, int i, string userName = "Body")
    {
        weakPoints[i].collider.enabled = false;
        weakPoints[i].hp = weakPoints[i].maxHp;
        weakPoints[i].attackName = userName;
    }

    // ����̔����L���ɂ���
    public virtual void OnCollisionEnable()
    {
        OnWPCollisionEnable(weakPointColliders);

        isExecution = true;
    }

    protected void OnWPCollisionEnable(WeakPointColliderMap[] weakPoints)
    {
        for (int i = 0; i < weakPoints.Length; i++)
        {
            if (useAtkName.Equals(weakPoints[i].attackName))
            {
                weakPoints[i].collider.enabled = true;
                weakPoints[i].hp = weakPoints[i].maxHp;
            }
        }
    }

    // �S�Ă̔���𖳌��ɂ���
    public virtual void OnCollisionEnableFinished()
    {
        if (!IsExecution) { return; }

        OnWPCollisionEnableFinished(weakPointColliders);

        isExecution = false;
    }

    protected void OnWPCollisionEnableFinished(WeakPointColliderMap[] weakPoints)
    {
        for (int i = 0; i < weakPoints.Length; i++)
        {
            weakPoints[i].collider.enabled = false;
            weakPoints[i].hp = 0.0f;
        }
    }

    public virtual void Damage(float dmg)
    {
        OnDamage(weakPointColliders, dmg);
    }

    protected void OnDamage(WeakPointColliderMap[] weakPoints, float dmg)
    {
        for (int i = 0; i < weakPoints.Length; i++)
        {
            if (!weakPoints[i].collider.enabled) { continue; }

            weakPoints[i].hp -= dmg;
            if (weakPoints[i].hp > 0.0f) { continue; }

            // HP�������Ȃ������ʂ𖳌��ɂ���
            weakPoints[i].collider.enabled = false;
            weakPoints[i].hp = 0.0f;
        }
    }

    public abstract class WeakPointColliderMap
    {
        public Collider collider;
        public float maxHp = 1.0f;
        [HideInInspector] public float hp = 1.0f;
        [HideInInspector] public string attackName;
    }
}
