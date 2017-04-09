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
    public ToggleEvent OnLocalEnable;
    public ToggleEvent OnLocalDisable;

    public ToggleEvent OnRemoteEnable;
    public ToggleEvent OnRemoteDisable;

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

[System.Serializable]
public sealed class ToggleEvent : UnityEvent<bool> { }
