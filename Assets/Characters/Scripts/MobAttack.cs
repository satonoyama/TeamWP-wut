using UnityEngine;

public class MobAttack : MonoBehaviour
{
    [SerializeField] protected float cooldownCounter = 0.0f; // ( Šm”F—p )Œã‚Åinspector‚©‚ç‚¢‚¶‚ê‚È‚­‚·‚é
    protected bool isHit = false;

    protected virtual void Start()
    {
        cooldownCounter = 0.0f;
    }

    protected virtual void Update()
    {
        CooldownCount();
    }

    protected virtual void CooldownCount()
    {
        if(cooldownCounter < 0.0f) { return; }

        cooldownCounter -= Time.deltaTime;
    }

    public virtual void OnAttackFinished()
    {
        isHit = false;
    }

    public virtual void OnAttackStart() {}

    public virtual void OnHitAttack(Collider collider)
    {
        if (isHit) { return; }

        isHit = true;
    }

    public abstract class AttackColliderMap
    {
        public Collider collider;
        public float power = 1.0f;
        public float cooldown = 1.0f;
        [HideInInspector] public string attackName;
    }
}
