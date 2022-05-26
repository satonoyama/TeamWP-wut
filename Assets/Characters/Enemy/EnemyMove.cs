using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStatus))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField] protected MovementController target;
    [SerializeField] protected EnemyStatus status;
     protected NavMeshAgent agent;

    [SerializeField] protected LegColliderMap[] legColliders;
    [SerializeField] protected ParticleSystem smoke;
    protected bool canGenerateParticle = false;

    [SerializeField] protected LayerMask raycastLayerMask;
    [SerializeField] protected Vector3 rayDir = new();
    [SerializeField] protected float rayDist = 1.0f;
    protected readonly RaycastHit[] _raycastHits = new RaycastHit[1];

    protected Vector3 moveVelocity;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<EnemyStatus>();
    }

    protected virtual void Update()
    {
        UpdateMove(target.transform.position);
    }

    protected virtual void UpdateMove(Vector3 position)
    {
        if(!status.CanMove) { return; }

        agent.destination = position;

        if (!canGenerateParticle) { return; }

        for(int i = 0; i < legColliders.Length; i++)
        {
            var pos = legColliders[i].collider.transform.position;

            Debug.DrawRay(pos, rayDir);

            // _raycastHitsに、ヒットしたColliderや座標情報などが格納される。RaycastAllと
            // RaycastNonAllocは同等の機能だが、RaycastNonAllocだとメモリにゴミが残らないのでこちらを推奨
            int hitCount = Physics.RaycastNonAlloc(pos, rayDir, _raycastHits, rayDist, raycastLayerMask);

            // 地面に触れてない場合は砂埃を発生させない
            if(hitCount == 0) { continue; }

            // 足元に砂埃を発生させる
            var dustSmoke = Instantiate(smoke, pos, Quaternion.identity);
            dustSmoke.Play();
        }

        canGenerateParticle = false;
    }

    public void OnCanGenerateRunSmoke()
    {
        canGenerateParticle = true;
    }

    [Serializable]
    public class LegColliderMap
    {
        public Collider collider;       // 足元コライダー
    }
}
