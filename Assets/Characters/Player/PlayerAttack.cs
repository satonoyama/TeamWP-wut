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

    protected override void Start()
    {
        base.Start();

        status = GetComponent<PlayerStatus>();
    }

    protected override void Update()
    {
        CooldownCount();
    }

    protected override void CooldownCount()
    {
        for(int i = 0; i < magics.Length; i++)
        {
            if(magics[i].canDo) { continue; }

            magics[i].cooldonwCount -= Time.deltaTime;

            // TODO : UIに値を渡すかも

            if(magics[i].cooldonwCount <= 0.0f)
            {
                magics[i].canDo = true;

                // TODO : スキルにも対応するなら、ここでUIを使用可能状態に
            }
        }
    }

    public void OnAttackStart(MagicID id)
    {
        if (!status.IsAttackable || !magics[(int)id].canDo) { return; }

        nowUseID = id;

        magics[(int)id].collider.enabled = true;

        status.GoToAttackStateIfPossible("");
    }

    public override void OnHitAttack(Collider collider)
    {
        base.OnHitAttack(collider);

        var targetMobe = GetComponent<EnemyStatus>();

        if (!targetMobe) { return; }

        targetMobe.Damage(magics[(int)nowUseID].power);
    }

    public override void OnAttackFinished()
    {
        base.OnAttackFinished();

        magics[(int)nowUseID].collider.enabled = false;

        magics[(int)nowUseID].canDo = false;

        magics[(int)nowUseID].cooldonwCount = magics[(int)nowUseID].cooldown;

        status.GoToNormalStateIfPossible();
    }

    [Serializable]
    public class MagicColliderMap : AttackColliderMap
    {
        public MagicID id;
        public float useSpValue = 1.0f;
        [HideInInspector] public float cooldonwCount = 0.0f;
        [HideInInspector] public bool canDo = true;
    }
}
