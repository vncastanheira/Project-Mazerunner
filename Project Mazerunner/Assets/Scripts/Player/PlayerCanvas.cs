using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour {

    public static PlayerCanvas canvas;

    [SerializeField] Slider battery;
    Animator animator;

    void Awake()
    {
        if (canvas == null)
            canvas = this;
        else if (canvas != this)
            Destroy(gameObject);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void JoinedMatch()
    {
        animator.SetTrigger("Start");
    }


    public void SetBaterry(float value)
    {
        if(battery != null)
            battery.value = value;
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }

    public void PickItem()
    {
        animator.SetTrigger("Pick");
    }

    public void Died()
    {
        animator.SetTrigger("Die");
    }
}
