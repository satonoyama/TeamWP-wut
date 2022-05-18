using System;
using UnityEngine;

public class EnemyWeakPoint : MonoBehaviour
{
    [SerializeField] private WeakPointColliderMap[] weakPointColliders;

    private bool isExecution = false;

    // �P�����ł����肪�L���ɂȂ��Ă����True��Ԃ�
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
    // �ȉ��AUI���Ŏg���\�����������
    //
    // ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== ===== =====

    // ���肪�L���ɂȂ��Ă��镔�ʂ̑��ő�HP�擾
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

    // ���肪�L���ɂȂ��Ă��镔�ʂ̑�HP�擾
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

    // ����̔����L���ɂ���
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

    // �S�Ă̔���𖳌��ɂ���
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

            // HP�������Ȃ������ʂ𖳌��ɂ���
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
