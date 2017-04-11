using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerBanner : NetworkBehaviour
{
    public string DropCommand;

    [Header("Prefabs")]
    public GameObject banner;
    [SyncVar] bool dropped = false;

    GameObject instance;

    public void UpdateLocal()
    {
        if (Input.GetButtonDown(DropCommand))
        {
            CmdDropBanner(transform.position);
            dropped = true;
        }
    }

    [Command]
    public void CmdDropBanner(Vector3 position)
    {
        if (instance != null)
        {
            NetworkServer.Destroy(instance);
        }

        position.y = 0.0f;
        instance = Instantiate(banner, position, banner.transform.rotation);
        NetworkServer.Spawn(instance);
    }
}
