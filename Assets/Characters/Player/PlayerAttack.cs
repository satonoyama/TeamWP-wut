using System;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerAttack : MobAttack
{
    public enum MagicID
    {
        eDark
    }
    private MagicID nowUseID = MagicID.eDark;

    [SerializeField] private MagicColliderMap[] magics;
    [SerializeField] private PlayerStatus status;

    // ����SP�ʂ���g�p�ʕ������Ă�SP������Ă��邩�ǂ���
    public bool CheckSP => (status.Sp - magics[(int)nowUseID].useSpValue) >= magics[(int)nowUseID].useSpValue;

    public float GetAtkPow => magics[(int)nowUseID].power;

    protected override void Start()
    {
        base.Start();

        status = GetComponent<PlayerStatus>();
    }

    protected override void Update()
    {
        CooldownCount();
    }

    // �� ���������K�؂ł͂Ȃ���������Ȃ����A�Ƃ肠�����g���Ă���
    private void CooldownCount()
    {
        for(int i = 0; i < magics.Length; i++)
        {
            if(magics[i].canDo) { continue; }

            // �҂��A�C�R���ł͂Ȃ�
            if(!magics[i].ability.IsCooltimeStop)
            {
                magics[i].canDo = true;
            }
        }
    }

    public void OnAttackStart(MagicID id)
    {
        if (!status.IsAttackable || !magics[(int)id].canDo) { return; }

        nowUseID = id;

        if (!CheckSP) { return; }

        int maxRange = magics[(int)id].collider.Length;
        for(int i = 0; i < maxRange; i++)
        {
            magics[(int)id].collider[i].enabled = true;
        }

        magics[(int)id].ability.OnActive();

        status.GoToAttackStateIfPossible("");
    }

    public override void OnHitAttack(Collider collider)
    {
        base.OnHitAttack(collider);

        var targetMobe = collider.GetComponent<EnemyStatus>();

        if (!targetMobe) { return; }

        targetMobe.Damage(magics[(int)nowUseID].power);
    }

    public override void OnAttackFinished()
    {
        base.OnAttackFinished();

        int maxRange = magics[(int)nowUseID].collider.Length;
        for (int i = 0; i < maxRange; i++)
        {
            magics[(int)nowUseID].collider[i].enabled = false;
        }

        magics[(int)nowUseID].canDo = false;

        status.GoToNormalStateIfPossible();
    }

    [Serializable]
    public class MagicColliderMap : AttackColliderMap
    {
        public MagicID id;
        public float useSpValue = 1.0f;
        public AbilityController ability = null;
        [HideInInspector] public bool canDo = true;
    }
}
