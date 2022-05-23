using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStatus))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField] protected MovementController target;
    [SerializeField] protected EnemyStatus status;
     protected NavMeshAgent agent;

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
    }
}
