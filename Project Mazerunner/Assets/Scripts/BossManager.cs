using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BossManager : NetworkBehaviour
{
    public GameObject Boss;

    public override void OnStartServer()
    {
        var spawnPoint = GameObject.FindGameObjectWithTag("BossPoint");
        var boss = Instantiate(Boss, spawnPoint.transform.position, Boss.transform.rotation);
        NetworkServer.Spawn(boss);
    }
}
