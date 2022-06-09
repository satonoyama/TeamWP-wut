using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStatus))]
public class EnemyMove : MonoBehaviour
{
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
        agent = GetComponentInChildren<NavMeshAgent>();
        status = GetComponentInChildren<EnemyStatus>();
    }

    protected virtual void Update()
    {
        UpdateMove(status.GetTarget.transform.position);
    }

    protected virtual void UpdateMove(Vector3 position)
    {
        if(!status.CanMove) { return; }

        agent.destination = position;

        GenerateRunSmoke();
    }

    protected virtual void GenerateRunSmoke()
    {
        if (!canGenerateParticle) { return; }

        for (int i = 0; i < legObjects.Length; i++)
        {
            var pos = legObjects[i].transform.position;

            int hitCount = Physics.RaycastNonAlloc(pos, rayDir, _raycastHits, rayDist, raycastLayerMask);

            // ’n–Ê‚ÉG‚ê‚Ä‚È‚¢ê‡‚Í»šº‚ð”­¶‚³‚¹‚È‚¢
            if (hitCount == 0) { continue; }

            // ‘«Œ³‚É»šº‚ð”­¶‚³‚¹‚é
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
