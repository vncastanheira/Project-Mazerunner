using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
public class DoorController : NetworkBehaviour
{
    NetworkAnimator animator;
    [SerializeField] public string OpenParameter;

    [Header("Configurations")]
    [SerializeField] public DoorType doorType;

    #region Numbered
    [SerializeField] public int TotalCount;
    [SyncVar] int _count = 0;
    #endregion

    void Start () {
        animator = GetComponent<NetworkAnimator>();
    }

    public void Activate()
    {
        switch (doorType)
        {
            case DoorType.Simple:
                Open();
                break;
            case DoorType.Numbered:
                if (_count >= TotalCount)
                    Open();
                else
                    _count++;

                break;
            default:
                break;
        }
    }
	
    void Open()
    {
        animator.SetTrigger(OpenParameter);
    }
}

public enum DoorType
{
    Simple,
    Numbered,
    //Timed
}
