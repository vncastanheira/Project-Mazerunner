using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class EscapeArea : NetworkBehaviour{

    public Camera Spectate;
    public UnityEvent OnPlayerEscaped;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isServer)
            {
                OnPlayerEscaped.Invoke();
                var player = other.GetComponent<PlayerMain>();
                player.RpcEscape();
                //NetworkServer.Destroy(player.gameObject);
            }
        }
    }
}
