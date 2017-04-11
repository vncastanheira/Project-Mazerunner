using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            //RpcMove(goal.transform.position);
        }
    }

    //[ClientRpc]
    //void RpcMove(Vector3 position)
    //{
    //    agent.destination = position;
    //}
}
