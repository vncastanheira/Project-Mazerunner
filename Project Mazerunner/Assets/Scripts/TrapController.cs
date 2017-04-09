using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class TrapController : NetworkBehaviour
{
    /// <summary> Trap is ready to be activated </summary>
    [SyncVar] bool ready = true;

    [Header("Events")]
    public UnityEvent OnTrapTriggered;

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMain>();
        if (ready && player != null)
        {
            player.RpcKill();
            RpcTrigger();
        }
    }

    [ClientRpc]
    public void RpcTrigger()
    {
        ready = false;
        OnTrapTriggered.Invoke();
    }
}
