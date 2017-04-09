using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/// <summary>
/// Enable GameObjects if this entity is local
/// </summary>
public class AuthorityToggle : NetworkBehaviour
{
    public UnityBoolEvent OnLocalEnable;
    public UnityBoolEvent OnLocalDisable;

    public UnityBoolEvent OnRemoteEnable;
    public UnityBoolEvent OnRemoteDisable;

    void Start()
    {
        if (isLocalPlayer)
        {
            OnLocalEnable.Invoke(true);
            OnLocalDisable.Invoke(false);
        }
        else
        {
            OnRemoteEnable.Invoke(true);
            OnRemoteDisable.Invoke(false);
        }
    }
}
