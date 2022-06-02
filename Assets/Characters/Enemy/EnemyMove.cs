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

    [SerializeField] protected GameObject[] legObjects;
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

        for(int i = 0; i < legObjects.Length; i++)
        {
            var pos = legObjects[i].transform.position;

            Debug.DrawRay(pos, rayDir, Color.red);

            int hitCount = Physics.RaycastNonAlloc(pos, rayDir, _raycastHits, rayDist, raycastLayerMask);

            // �n�ʂɐG��ĂȂ��ꍇ�͍����𔭐������Ȃ�
            if(hitCount == 0) { continue; }

            // �����ɍ����𔭐�������
            var dustSmoke = Instantiate(smoke, pos, Quaternion.identity);
            dustSmoke.Play();
        }

        canGenerateParticle = false;
    }

    public void OnCanGenerateRunSmoke()
    {
        canGenerateParticle = true;
    }
}
