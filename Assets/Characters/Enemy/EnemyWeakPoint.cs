using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeakPoint : MonoBehaviour
{
    protected Dictionary<string, WeakPointColliderMap> weakPointList = new();
    
    [SerializeField] protected EnemyAttack attack;

    protected Collider hitCollider = null;
    protected string useAtkName;
    protected bool isExecution = false;

    public bool IsExecution => isExecution;

    public void SetHitCollider(Collider collider) => hitCollider = collider;

    // �P�����ł����肪�L���ɂȂ��Ă����True��Ԃ�
    public virtual bool IsColliderEnable()
    {
        bool result = false;

        int maxRange = weakPointList[useAtkName].colliders.Length;
        for(int i = 0; i < maxRange; i++)
        {
            if (!weakPointList[useAtkName].colliders[i].collider.enabled) { continue; }

            result = true;
            break;
        }

        return result;
    }

    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====
    //
    // �ȉ��AUI���Ŏg���\�����������
    //
    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====

    // ���肪�L���ɂȂ��Ă��镔�ʂ̑��ő�HP�擾
    public virtual float MaxHp()
    {
        float maxHp = 0.0f;

        int maxRange = weakPointList[useAtkName].colliders.Length;
        for (int i = 0; i < maxRange; i++)
        {
            if (!weakPointList[useAtkName].colliders[i].collider.enabled) { continue; }

            maxHp += weakPointList[useAtkName].maxHp;
        }

        return maxHp;
    }

    // ���肪�L���ɂȂ��Ă��镔�ʂ̑�HP�擾
    public virtual float Hp()
    {
        float hp = 0.0f;

        int maxRange = weakPointList[useAtkName].colliders.Length;
        for (int i = 0; i < maxRange; i++)
        {
            if (!weakPointList[useAtkName].colliders[i].collider.enabled) { continue; }

            hp += weakPointList[useAtkName].hp;
        }

        return hp;
    }

    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====
    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====

    protected virtual void Start()
    {
        attack = GetComponentInChildren<EnemyAttack>();

        hitCollider = GetComponent<Collider>();
    }

    protected virtual void InitWeakPointColliders(string attackName, WeakPointColliderMap weakPoint)
    {
        weakPointList.Add(attackName, weakPoint);

        useAtkName = attackName;
        weakPointList[attackName].hp = weakPointList[attackName].maxHp;

        OnColliderFinished();

        int maxRange = weakPointList[useAtkName].colliders.Length;
        for (int i = 0; i < maxRange; i++)
        {
            WeakPointContainer.Instance.Add(weakPointList[attackName].colliders[i].pointPosCollider);
        }
    }

    protected virtual void OnColliderFinished()
    {
        int maxRange = weakPointList[useAtkName].colliders.Length;
        for (int i = 0; i < maxRange; i++)
        {
            if (!weakPointList[useAtkName].colliders[i].collider.enabled) { continue; }

            weakPointList[useAtkName].colliders[i].collider.enabled = false;
            weakPointList[useAtkName].colliders[i].pointPosCollider.enabled = false;

            Collider pointCollider = weakPointList[useAtkName].colliders[i].pointPosCollider;

            if (!WeakPointContainer.Instance.GetWeakPoint(pointCollider)) { continue; }

            WeakPointContainer.Instance.GetWeakPoint(pointCollider).OnActiveFinished();
        }
    }

    // ����̔����L���ɂ���
    public virtual void OnWeakPointStart()
    {
        useAtkName = attack.GetUseAtkName;

        if (!weakPointList.ContainsKey(useAtkName)) { return; }

        int maxRange = weakPointList[useAtkName].colliders.Length;
        for (int i = 0; i < maxRange; i++)
        {
            weakPointList[useAtkName].colliders[i].collider.enabled = true;
            weakPointList[useAtkName].hp = weakPointList[useAtkName].maxHp;

            Collider pointCollider = weakPointList[useAtkName].colliders[i].pointPosCollider;

            if (!WeakPointContainer.Instance.GetWeakPoint(pointCollider)) { continue; }

            WeakPointContainer.Instance.GetWeakPoint(pointCollider).OnActive();
        }

        isExecution = true;
    }

    // �S�Ă̔���𖳌��ɂ���
    public virtual void OnWeakPointFinished()
    {
        if (!IsExecution) { return; }

        weakPointList[useAtkName].hp = 0.0f;

        OnColliderFinished();

        isExecution = false;
    }

    public virtual void OnHitPlayerAttack(Collider atkCollider)
    {
        var attack = atkCollider.GetComponent<PlayerAttack>();

        if (!attack) { return; }

        int maxRange = weakPointList[useAtkName].colliders.Length;
        for (int i = 0; i < maxRange; i++)
        {
            // ���Ă�ꂽ�R���C�_�[�ƈ�v���Ȃ�
            if (hitCollider != weakPointList[useAtkName].colliders[i].collider ||
                !weakPointList[useAtkName].colliders[i].collider.enabled) { continue; }

            weakPointList[useAtkName].hp -= attack.GetAtkPow;
            if (weakPointList[useAtkName].hp > 0.0f) { continue; }

            weakPointList[useAtkName].colliders[i].collider.enabled = false;

            weakPointList[useAtkName].hp = 0.0f;

            Collider pointCollider = weakPointList[useAtkName].colliders[i].pointPosCollider;

            if (!WeakPointContainer.Instance.GetWeakPoint(pointCollider)) { return; }

            WeakPointContainer.Instance.GetWeakPoint(pointCollider).OnActiveFinished();
        }
    }

    public abstract class WeakPointColliderMap
    {
        public PointCollider[] colliders;
        public float maxHp = 1.0f;
        [HideInInspector] public float hp = 1.0f;
    }

    [Serializable]
    public class PointCollider
    {
        public Collider collider;
        public Collider pointPosCollider;
    }
}
