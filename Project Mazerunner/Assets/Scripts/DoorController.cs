using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
public class DoorController : NetworkBehaviour
{
    NetworkAnimator animator;
    NavMeshObstacle obstacle;
    [SerializeField] public string OpenParameter;
    [SerializeField] public UnityEvent OnOpen;


    [Header("Configurations")]
    [SerializeField] public DoorType doorType;

    #region Numbered
    [SerializeField] public int TotalCount;
    [SyncVar] int _count = 0;
    #endregion

    #region Timed
    [SerializeField] public float Timer;
    [SyncVar] float _timer = 0;
    [SyncVar] bool isOpen = false;

    [SerializeField] public string CloseParameter;
    [SerializeField] public UnityEvent OnClose;
    #endregion

    void Start()
    {
        animator = GetComponent<NetworkAnimator>();
    }

    private void Update()
    {
        if (isServer && _timer > 0 && !isOpen)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0) // Close doors
            {
                _count = 0;
                Close();
            }
        }
    }

    public void Activate()
    {
        switch (doorType)
        {
            case DoorType.Simple:
                Open();
                break;
            case DoorType.Numbered:
                _count++;
                if (_count >= TotalCount)
                    Open();

                break;
            case DoorType.Timed:
                if (!isOpen) // If door is closed
                {
                    if (_count == 0) // start timer
                    {
                        _timer = Timer;
                        _count++;
                    }
                    else if (_timer > 0)
                    {
                        _count++;
                    }

                    if (_count >= TotalCount) // if count is 
                    {
                        Open();
                    }
                }
                break;
            default:
                break;
        }
    }

    void Open()
    {
        animator.SetTrigger(OpenParameter);
        OnOpen.Invoke();
        isOpen = true;
    }

    void Close()
    {
        animator.SetTrigger(CloseParameter);
        isOpen = false;
        OnClose.Invoke();
    }
}

public enum DoorType
{
    Simple,
    Numbered,
    Timed
}
