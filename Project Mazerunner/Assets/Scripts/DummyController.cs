using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DummyController : NetworkBehaviour
{
    [ClientRpc]
    public void RpcHit()
    {
        Debug.Log("You hit the dummy");
    }

}
