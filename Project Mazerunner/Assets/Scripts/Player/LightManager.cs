using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vnc.Network;

[RequireComponent(typeof(BatteryManager))]
public class LightManager : NetworkLight
{
    [SerializeField, HideInInspector] BatteryManager batteryManager;

    void Start()
    {
        batteryManager = GetComponent<BatteryManager>();
    }
}
