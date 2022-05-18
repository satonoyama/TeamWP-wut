using System;
using UnityEngine;

public class EnemyWeakPoint : MonoBehaviour
{
    [SerializeField] private WeakPointColliderMap[] weakPointColliders;

    private bool isExecution = false;

    // １か所でも判定が有効になっていればTrueを返す
    public bool IsColliderEnable()
    {
        bool result = false;

        for(int i = 0; i < weakPointColliders.Length; i++)
        {
            if(weakPointColliders[i].collider.enabled)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    public bool IsExecution => isExecution;

    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====
    //
    // 以下、UI等で使う可能性があるもの
    //
    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====

    // 判定が有効になっている部位の総最大HP取得
    public float MaxHp()
    {
        float maxHp = 0.0f;

        for (int i = 0; i < weakPointColliders.Length; i++)
        {
            if (!weakPointColliders[i].collider.enabled) { continue; }

            maxHp += weakPointColliders[i].maxHp;

        }

        return maxHp;
    }

    // 判定が有効になっている部位の総HP取得
    public float Hp()
    {
        float hp = 0.0f;

        for (int i = 0; i < weakPointColliders.Length; i++)
        {
            if (!weakPointColliders[i].collider.enabled) { continue; }

            hp += weakPointColliders[i].hp;

        }

        return hp;
    }

    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====
    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====

    void Start()
    {
        for (int i = 0; i < weakPointColliders.Length; i++)
        {
            weakPointColliders[i].collider.enabled = false;
            weakPointColliders[i].hp = weakPointColliders[i].maxHp;
        }
    }

    // 特定の判定を有効にする
    public void OnCollisionEnable(string name)
    {
        for (int i = 0; i < weakPointColliders.Length; i++)
        {
            if(name.Equals(weakPointColliders[i].attackName))
            {
                weakPointColliders[i].collider.enabled = true;
                weakPointColliders[i].hp = weakPointColliders[i].maxHp;
            }
        }

        isExecution = true;
    }

    // 全ての判定を無効にする
    public void OnCollisionEnableFinished()
    {
        for (int i = 0; i < weakPointColliders.Length; i++)
        {
            weakPointColliders[i].collider.enabled = false;
            weakPointColliders[i].hp = 0.0f;
        }

        isExecution = false;
    }

    public void Damage(int dmg)
    {
        for (int i = 0; i < weakPointColliders.Length; i++)
        {
            if (!weakPointColliders[i].collider.enabled) { continue; }

            weakPointColliders[i].hp -= dmg;
            if(weakPointColliders[i].hp > 0.0f) { continue; }

            // HPが無くなった部位を無効にする
            weakPointColliders[i].collider.enabled = false;
            weakPointColliders[i].hp = 0.0f;
        }
    }

    [Serializable]
    public class WeakPointColliderMap
    {
        public Collider collider;
        public string attackName;
        public float maxHp = 1.0f;
        public float hp = 1.0f;
    }
}
