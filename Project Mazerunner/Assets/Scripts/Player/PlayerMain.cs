using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/// <summary> Manages if player is alive and inputs </summary>
[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteraction))]
public class PlayerMain : NetworkBehaviour
{
    [SyncVar(hook = "IsAliveHook")] bool isAlive = true;
    [Header("Events")]
    public UnityEvent OnDeath;

#region Components
    NetworkAnimator animator;
    PlayerLook playerLook;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    LightManager lightManager;
    BatteryManager batteryManager;
#endregion

    private void Start()
    {
        playerLook = GetComponent<PlayerLook>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent<PlayerShooting>();
        animator = GetComponent<NetworkAnimator>();
        lightManager = GetComponent<LightManager>();
        batteryManager = GetComponent<BatteryManager>();
    }

    public override void OnStartClient()
    {
        PlayerCanvas.canvas.JoinedMatch();
    }

    void Update()
    {
        if (isLocalPlayer && isAlive)
        {
            playerLook.UpdateLocal();
            playerMovement.UpdateLocal();
            playerShooting.UpdateLocal();
            lightManager.UpdateLocal();
            batteryManager.UpdateLocal();
        }
    }

    #region Death

    /// <summary> Kill the player </summary>
    [ClientRpc]
    public void RpcKill()
    {
        isAlive = false;
        OnDeath.Invoke();
    }

    void IsAliveHook(bool value)
    {
        if (isLocalPlayer)
        {
            animator.animator.SetBool("Alive", false);
            PlayerCanvas.canvas.Died();
        }
    }

#endregion
}
