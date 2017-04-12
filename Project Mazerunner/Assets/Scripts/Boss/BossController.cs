using UnityEngine.AI;
using UnityEngine.Networking;

public class BossController : NetworkBehaviour
{
    NavMeshAgent agent;
    PlayerMain goal;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isServer)
        {
            if (goal == null)
                goal = FindObjectOfType<PlayerMain>();

            if (goal != null)
                agent.destination = goal.transform.position;
        }
    }
    
    [Server]
    public void Kill()
    {
        NetworkServer.Destroy(gameObject);
        BossManager.instance.RestartTimer();
    }
}
