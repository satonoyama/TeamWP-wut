using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStatus))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField] private MovementController target;
    private NavMeshAgent agent;
    private EnemyStatus status;

    [SerializeField] private float descentSpeed = 0.01f;
    private Vector3 moveVelocity;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<EnemyStatus>();
    }

    private void Update()
    {
        if (status.IsFly)
        {
            moveVelocity.y -= descentSpeed;

            status.transform.position = moveVelocity;
        }

        UpdateMove(target.transform.position);
    }

    public void UpdateMove(Vector3 position)
    {
        if(!status.CanMove) { return; }

        agent.destination = position;
    }
}
