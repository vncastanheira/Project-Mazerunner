using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BossManager : NetworkBehaviour
{
    public BossController Boss;
    BossController _boss;

    public override void OnStartServer()
    {
        var spawnPoint = GameObject.FindGameObjectWithTag("BossPoint");
        _boss = Instantiate(Boss, spawnPoint.transform.position, Boss.transform.rotation);
        NetworkServer.Spawn(_boss.gameObject);
    }

    private void Update()
    {
        if (isServer)
        {
            if(_boss == null)
            {
                var spawnPoint = GameObject.FindGameObjectWithTag("BossPoint");
                _boss = Instantiate(Boss, spawnPoint.transform.position, Boss.transform.rotation);
                NetworkServer.Spawn(_boss.gameObject);
            }
        }
    }
}
