using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    public NetworkManager netManager;

    int _escapedPlayers = 0;

    public void PlayerEscaped()
    {
        _escapedPlayers++;
        if (_escapedPlayers == netManager.numPlayers)
        {
            Debug.Log("You Win");
        }
    }
}
