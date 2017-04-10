using UnityEngine;
using UnityEngine.Networking;

public class ItemController : NetworkBehaviour
{
    public ItemTypes ItemType;

    [Server]
    void OnTriggerEnter(Collider other)
    {
        switch (ItemType)
        {
            case ItemTypes.Gun:
                var player = other.GetComponent<PlayerShooting>();
                if (player != null && !player.HasGun)
                {
                    player.RpcGetGun();
                    NetworkServer.Destroy(gameObject);
                }
                break;
            case ItemTypes.Battery:
                var playerBattery = other.GetComponent<BatteryManager>();
                if (playerBattery != null)
                {
                    playerBattery.RpcAddStock(1);
                    NetworkServer.Destroy(gameObject);
                }
                break;
            default:
                break;
        }
    }
}

public enum ItemTypes
{
    Gun,
    Battery
}
