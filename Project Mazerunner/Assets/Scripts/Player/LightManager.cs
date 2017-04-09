using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vnc.Network;

[RequireComponent(typeof(BatteryManager))]
public class LightManager : NetworkLight
{
    [SerializeField] BatteryManager batteryManager;

    private void Start()
    {
        batteryManager = GetComponentInChildren<BatteryManager>();
        flashLight = GetComponentInChildren<Light>();
    }

    public void UpdateLocal()
    {
        batteryManager.CmdUpdateBattery(isLightOn, Time.deltaTime);

        if (batteryManager.HasCharge)
        {
            if (Input.GetButtonDown(ButtonCommand))
            {
                isLightOn = !isLightOn;
            }
        }
        else
        {
            isLightOn = false;
        }
        CmdUpdateLight(isLightOn);
    }
}
