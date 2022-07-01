using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeakPoint : MonoBehaviour
{
    protected Dictionary<string, WeakPointColliderMap> weakPointList = new();
    
    [SerializeField] protected EnemyAttack attack;
    [SerializeField] protected CancelGaugeController gaugeController;

    protected Collider hitCollider = null;
    protected string useAtkName;
    protected bool isExecution = false;
    protected bool enableWeakPoint = false;
    protected bool isHitWeakPoint = false;

    public bool IsExecution => isExecution;

    public bool IsHitWeakPoint => isHitWeakPoint;
    
    public void SetHitCollider(Collider collider) => hitCollider = collider;

    // １か所でも判定が有効になっていればTrueを返す
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
    // 以下、UI等で使う可能性があるもの
    //
    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====

    // 判定が有効になっている部位の総HP取得
    public virtual float Hp()
    {
        float hp = 0.0f;

        int maxRange = weakPointList[useAtkName].colliders.Length;
        for (int i = 0; i < maxRange; i++)
        {
            if (!weakPointList[useAtkName].colliders[i].collider.enabled) { continue; }

            hp += weakPointList[useAtkName].colliders[i].hp;
        }

        return hp;
    }

    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====
    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====

    protected virtual void Start()
    {
        attack = GetComponentInChildren<EnemyAttack>();
    }

    protected virtual void InitWeakPointColliders(string attackName, WeakPointColliderMap weakPoint)
    {
        weakPointList.Add(attackName, weakPoint);

        useAtkName = attackName;

        OnColliderFinished();

        int maxRange = weakPointList[useAtkName].colliders.Length;
        for (int i = 0; i < maxRange; i++)
        {
            weakPointList[useAtkName].colliders[i].pointPosCollider.enabled = false;
            weakPointList[attackName].colliders[i].hp = weakPointList[useAtkName].maxHp;
            WeakPointContainer.Instance.Add(weakPointList[attackName].colliders[i].pointPosCollider);
        }
    }

    protected virtual void OnColliderFinished()
    {
        int maxRange = weakPointList[useAtkName].colliders.Length;
        for (int i = 0; i < maxRange; i++)
        {
            if (!weakPointList[useAtkName].colliders[i].collider.enabled) { continue; }

            weakPointList[useAtkName].colliders[i].hp = weakPointList[useAtkName].maxHp;

            Collider pointCollider = weakPointList[useAtkName].colliders[i].pointPosCollider;

            if (!WeakPointContainer.Instance.GetWeakPoint(pointCollider)) { continue; }

            WeakPointContainer.Instance.GetWeakPoint(pointCollider).OnActiveFinished();
        }
    }

    // 特定の部位のポイント画像を有効にする
    public virtual void OnWeakPointStart()
    {
        if (IsExecution) { return; }

        useAtkName = attack.GetUseAtkName;

        if (!weakPointList.ContainsKey(useAtkName)) { return; }

        int maxRange = weakPointList[useAtkName].colliders.Length;
        float maxHp = 0.0f;
        for (int i = 0; i < maxRange; i++)
        {
            weakPointList[useAtkName].colliders[i].hp = weakPointList[useAtkName].maxHp;

            maxHp += weakPointList[useAtkName].colliders[i].hp;

            Collider pointCollider = weakPointList[useAtkName].colliders[i].pointPosCollider;

            if (!WeakPointContainer.Instance.GetWeakPoint(pointCollider)) { continue; }

            WeakPointContainer.Instance.GetWeakPoint(pointCollider).OnActive();
        }

        gaugeController.SetMaxWeakPointHP(maxHp);

        isExecution = true;

        enableWeakPoint = true;

        isHitWeakPoint = false;
    }

    // 全ての判定を無効にする
    public virtual void OnWeakPointFinished()
    {
        if (!IsExecution) { return; }

        OnColliderFinished();

        isExecution = false;

        enableWeakPoint = false;

        isHitWeakPoint = false;
    }

    public virtual void OnHitMagic(GameObject magicObj)
    {
        if (!enableWeakPoint) { return; }

        var magic = magicObj.GetComponent<MagicalFX.MagicInfo>();

        if (!magic) { return; }

        int maxRange = weakPointList[useAtkName].colliders.Length;
        for (int i = 0; i < maxRange; i++)
        {
            // 当てられたコライダーと一致しない
            if (hitCollider != weakPointList[useAtkName].colliders[i].collider) 
            { continue; }

            isHitWeakPoint = true;

            weakPointList[useAtkName].colliders[i].hp--;
            if (weakPointList[useAtkName].colliders[i].hp > 0.0f) { continue; }

            weakPointList[useAtkName].colliders[i].hp = 0.0f;

            Collider pointCollider = weakPointList[useAtkName].colliders[i].pointPosCollider;

            if (!WeakPointContainer.Instance.GetWeakPoint(pointCollider)) { continue; }

            WeakPointContainer.Instance.GetWeakPoint(pointCollider).OnActiveFinished();
        }

        hitCollider = null;
    }

    public abstract class WeakPointColliderMap
    {
        public PointCollider[] colliders;
        public float maxHp = 1.0f;
    }

    [Serializable]
    public class PointCollider
    {
        public Collider collider;
        public Collider pointPosCollider;
        [HideInInspector] public float hp = 1.0f;
    }
}
