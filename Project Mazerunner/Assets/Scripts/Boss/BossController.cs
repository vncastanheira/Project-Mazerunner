using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class BossController : NetworkBehaviour
{
    NavMeshAgent agent;
    Vector3 goal;
    float _timer = 0;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isServer)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0 || DestinationReached())
            {
                // Wander  around
                goal = RandomDirection();
                _timer = 60;
            }

            //if (goal == null)
            //    goal = FindObjectOfType<PlayerMain>();

            if (goal != null)
                agent.destination = goal;

        }
    }


    Vector3 RandomDirection()
    {
        var randomDirection = Random.insideUnitSphere * 10;
        randomDirection += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, 10, -1);

        return navHit.position;
    }

    bool DestinationReached()
    {
        return agent.remainingDistance < 1;
    }

    [Server]
    public void Kill()
    {
        NetworkServer.Destroy(gameObject);
        BossManager.instance.RestartTimer();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(goal, Vector3.one);
    }
}
