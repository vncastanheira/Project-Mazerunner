using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class BossManager : NetworkBehaviour
{
    public static BossManager instance;

    [Header("References")]
    public BossController Boss;

    [Header("Configurations")]
    public LayerMask WallLayer;
    public float Timer;
    float _timer;
    Ray ray;

    BossPoint[] spawnPoints;
    BossController _boss;

    public override void OnStartServer()
    {
        instance = this;
        spawnPoints = FindObjectsOfType<BossPoint>();
        _timer = Timer;
    }


    private void Update()
    {
        if (isServer)
        {
            if (_timer > 0)
                _timer -= Time.deltaTime;

            if (_boss == null && _timer <= 0)
            {
                var spawnPoint = ChooseSpawn();
                if (spawnPoint != null)
                {
                    Debug.Log("Spawn found at " + spawnPoint.transform.position);
                    _boss = Instantiate(Boss, spawnPoint.transform.position, Boss.transform.rotation);
                    NetworkServer.Spawn(_boss.gameObject);
                }
                else
                {
                    Debug.Log("Spawn not found, restarting timer");
                    _timer = Timer;
                }
            }
        }
    }

    BossPoint ChooseSpawn()
    {
        foreach (var spawn in spawnPoints)
        {
            if (spawn.isFree)
            {
                return spawn;
            }
        }
        return null;
    }


    /// <summary>
    /// Restart the timer for the next respawn
    /// </summary>
    public void RestartTimer()
    {
        _timer = Timer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }
}
