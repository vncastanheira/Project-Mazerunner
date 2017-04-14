using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class BossController : NetworkBehaviour
{
    NavMeshAgent agent;

    [Header("Player Tracking")]
    public LayerMask PlayerLayer;
    public Transform Eyes;
    GameObject trackedPlayer;
    List<PlayerMain> totalPlayers;

    [Header("Configurations")]
    public float Timer;
    public float sightDistance;

    Vector3 goal;
    float _timer = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        totalPlayers = FindObjectsOfType<PlayerMain>().ToList();
    }

    private void Update()
    {
        if (isServer)
        {
            if (TrackPlayer())
            {
                ChasePlayer();
            }
            else
            {
                if (trackedPlayer != null)
                {
                    ChasePlayer();
                }
                else
                {
                    // if no player was found, walk randomly
                    if (goal == null || DestinationReached())
                        goal = RandomInPlayersArea();

                    agent.destination = goal;
                }
            }
        }
    }

    /// <summary>
    /// Track the player, set the timing for chasing
    /// </summary>
    /// <returns></returns>
    bool TrackPlayer()
    {
        RaycastHit hit;
        if (Physics.Linecast(Eyes.transform.position, Eyes.transform.position + (Eyes.transform.forward * sightDistance), out hit, PlayerLayer))
        {
            Debug.Log("Player found");
            trackedPlayer = hit.collider.gameObject;
            _timer = Timer; // start timer for chasing
            return true;
        }
        return false;
    }

    /// <summary>
    /// Chase player. If player is out of sight,
    /// timer will slowly go down
    /// </summary>
    void ChasePlayer()
    {
        _timer -= Time.deltaTime;
        agent.destination = trackedPlayer.transform.position;
        if (_timer <= 0)
            trackedPlayer = null;
    }

    /// <summary>
    /// Find a point in navmesh closer to the position
    /// </summary>
    /// <param name="position">Sampled position</param>
    Vector3 RandomDirection(Vector3 position)
    {
        var randomDirection = Random.insideUnitSphere * 10;
        randomDirection += position;

        NavMeshHit navHit;
        
        NavMesh.SamplePosition(randomDirection, out navHit, 10, -1);

        return navHit.position;
    }

    /// <summary> Try to find a random position near the players </summary>
    Vector3 RandomInPlayersArea()
    {
        if (totalPlayers == null || totalPlayers.Count == 0)
            return RandomDirection(transform.position);

        switch (totalPlayers.Count)
        {
            case 1:
                return RandomDirection(totalPlayers[0].transform.position);
            case 2:
                var p1a = totalPlayers[0].transform.position;
                var p2a = totalPlayers[1].transform.position;
                return RandomDirection(Vector3.Lerp(p1a, p2a, 0.5f));
            case 3:
                var p1b = totalPlayers[0].transform.position;
                var p2b = totalPlayers[1].transform.position;
                var p3b = totalPlayers[3].transform.position;
                var midPoint = (p1b + p2b + p3b) / 3;
                return RandomDirection(midPoint);
            case 4:
                int i1 = Random.Range(0, totalPlayers.Count);
                int i2 = Random.Range(0, totalPlayers.Count);
                var p1c = totalPlayers[i1].transform.position;
                var p2c = totalPlayers[i2].transform.position;
                return RandomDirection(Vector3.Lerp(p1c, p2c, 0.5f));
            default:
                return RandomDirection(transform.position);
        }
    }

    /// <summary> Check if the boss is near the goal </summary>
    bool DestinationReached()
    {
        return agent.remainingDistance < 0.5f;
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * sightDistance));
    }
}
