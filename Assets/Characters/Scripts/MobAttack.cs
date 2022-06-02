using UnityEngine;

public class MobAttack : MonoBehaviour
{
    protected bool isHit = false;

    protected virtual void Start() { }

    protected virtual void Update() { }

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
        [HideInInspector] public string attackName;
    }
}
