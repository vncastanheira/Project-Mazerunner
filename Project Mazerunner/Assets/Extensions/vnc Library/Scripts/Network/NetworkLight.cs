using UnityEngine;
using UnityEngine.Networking;

namespace vnc.Network
{
    public class NetworkLight : NetworkBehaviour
    {
        [SerializeField, HideInInspector] protected Light flashLight;
        [SerializeField, SyncVar] bool lightOn;
        [Range(0.0f, 8.0f)] public float MaxIntensity;
        public string ButtonCommand;

        public bool isLightOn
        {
            get
            {
                return lightOn;
            }

            set
            {
                lightOn = value;
            }
        }

        [Command]
        public void CmdUpdateLight(bool isOn)
        {
            RpcSwitchLight(isOn);
        }


        [ClientRpc]
        public void RpcSwitchLight(bool isOn)
        {
            if (isOn)
            {
                flashLight.intensity = MaxIntensity;
            }
            else
            {
                flashLight.intensity = 0;
            }
        }
    }
}
