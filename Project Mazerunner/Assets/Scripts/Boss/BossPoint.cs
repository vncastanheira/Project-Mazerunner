using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BossPoint : NetworkBehaviour {

    [SyncVar] int players = 0;
    public int Players
    {
        get { return players; }
    }
    public bool isFree
    {
        get
        {
            return players == 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isServer && other.CompareTag("Player"))
        {
            players++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isServer && other.CompareTag("Player"))
        {
            players--;
        }

    }
}
