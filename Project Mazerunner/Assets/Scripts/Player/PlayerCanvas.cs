using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour {

    public static PlayerCanvas canvas;

    [Header("Battery Slider")]
    [SerializeField] Slider battery;

    [Header("Battery Stock")]
    public string Label;
    [SerializeField] Text stockValue; 
    Animator animator;

    [Header("Splash")]
    public CanvasRenderer Splash;

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

    public void UpdateBatteryStock(int stock)
    {
        stockValue.text = Label + " " + stock;
    }

    public void UseBatteryStock()
    {
        animator.SetTrigger("UseBattery");
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

    public void Escaped()
    {

    }
}
