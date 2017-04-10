using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[System.Serializable]
public class BatteryManager : NetworkBehaviour
{
    [Header("Battery Settings")]
    [SerializeField, Range(0, 1000)]
    public float MaxBatteryLife = 100.0f;
    [SerializeField, SyncVar] float batteryLife;
    [SerializeField, SyncVar] int batteryStock = 0;
    public string ChargeCommand;

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

    public void UpdateLocal()
    {
        if (isLocalPlayer)
        {
            if (Input.GetButtonDown(ChargeCommand))
            {
                if (batteryStock > 0)
                {
                    RpcCharge();
                }
                else
                {
                    Debug.Log("No stored battery");
                }
            }
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
        if (isUsing)
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

    /// <summary>
    /// Store a battery from the map
    /// </summary>
    [ClientRpc]
    public void RpcAddStock(int quantity)
    {
        if (isLocalPlayer)
        {
            batteryStock += quantity;
            PlayerCanvas.canvas.UpdateBatteryStock(batteryStock);
            PlayerCanvas.canvas.PickItem();
        }
    }

    /// <summary>
    /// Charge your battery from a battery stored
    /// </summary>
    [ClientRpc]
    public void RpcCharge()
    {
        if (isLocalPlayer)
        {
            if (batteryStock > 0)
            {
                batteryStock--;
                batteryLife = 100;
                PlayerCanvas.canvas.UseBatteryStock();
                PlayerCanvas.canvas.UpdateBatteryStock(batteryStock);
            }
        }
    }

}

[System.Serializable]
public class BatteryEvent : UnityEvent<float> { }