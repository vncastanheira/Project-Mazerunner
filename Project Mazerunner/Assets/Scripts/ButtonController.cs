using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[System.Serializable]
public class ButtonController : NetworkBehaviour
{
    [Header("Events")]
    public UnityEvent OnButtonPressed;

    [ClientRpc]
    public void RpcPress()
    {
        OnButtonPressed.Invoke();
    }
}
