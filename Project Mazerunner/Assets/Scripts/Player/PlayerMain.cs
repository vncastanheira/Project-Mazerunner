﻿using UnityEngine;
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

    [Header("Components")]
    public VisibleFrame FrameAnimation;
    public NetworkAnimator NetAnimator;
    PlayerLook playerLook;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    LightManager lightManager;

    private void Start()
    {
        playerLook = GetComponent<PlayerLook>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent<PlayerShooting>();

        lightManager = GetComponentInChildren<LightManager>();
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
            FrameAnimation.isAlive = false;
            NetAnimator.animator.SetBool("Alive", false);
            PlayerCanvas.canvas.Died();
        }
    }

#endregion
}
