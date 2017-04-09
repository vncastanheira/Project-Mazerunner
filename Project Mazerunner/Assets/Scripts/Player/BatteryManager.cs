using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[System.Serializable]
public class BatteryManager : NetworkBehaviour
{
    [Header("Battery Settings")]
    [SerializeField, Range(0, 1000)] public float MaxBatteryLife = 100.0f;
    [SerializeField, SyncVar] float batteryLife;
    //[SerializeField, SyncVar(hook = "OnHealthChanged")] float batteryLife;

    public float BatteryLife
    {
        get
        {
            return batteryLife;
        }
    }
    public bool HasCharge
    {
        get
        {
            return batteryLife > 0;
        }
    }

    [ServerCallback]
    void OnEnable()
    {
        batteryLife = MaxBatteryLife;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            PlayerCanvas.canvas.SetBaterry(batteryLife);
        }
    }

#region Update

    [Command]
    public void CmdUpdateBattery(bool isUsing, float deltaTime)
    {
        RpcUpdateBaterry(isUsing, deltaTime);
    }

    /// <summary>
    /// Update the battery on clients
    /// </summary>
    /// <param name="isUsing">If the battery is being used</param>
    [ClientRpc]
    public void RpcUpdateBaterry(bool isUsing, float deltaTime)
    {
        if(isUsing)
            batteryLife -= deltaTime;
    }

#endregion

    [Command]
    public void CmdUseBattery(float value)
    {
        if (value < batteryLife)
        {
            RpcUseBattery(value);
        }
    }

    [ClientRpc]
    public void RpcUseBattery(float value)
    {
        batteryLife -= value;
    }

}

[System.Serializable]
public class BatteryEvent : UnityEvent<float> { }