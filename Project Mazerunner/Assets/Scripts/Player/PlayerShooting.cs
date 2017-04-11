using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[RequireComponent(typeof(BatteryManager))]
public class PlayerShooting : NetworkBehaviour
{
    [HideInInspector, SyncVar] public bool HasGun = false;

    [Header("Settings")]
    public string ButtonCommand;
    [Tooltip("How many battery charges it requires to shoot")]
    public float ChargeRequired = 10;
    public float Cooldown;
    public LayerMask CanHit;
    float _timer;

    [Header("References")]
    public Transform GunHole;
    public ParticleSystem plasmaExplosion;
    public ParticleSystem muzzleFlash;
    Camera playerCamera;
    Animator animator;
    BatteryManager manager;

    [Header("Events")]
    public UnityEvent OnPickingGun;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        manager = GetComponent<BatteryManager>();
        animator = GetComponent<Animator>();
        _timer = Cooldown;
    }

    public void UpdateLocal()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        if (Input.GetButtonDown(ButtonCommand) && HasGun)
        {
            // Check if there is enough battery to shoot
            if (manager.BatteryLife < ChargeRequired)
            {
                Debug.Log("Not enough battery for shooting");
                return;
            }

            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            CmdShoot(GunHole.position, ray.direction);
            manager.CmdUseBattery(ChargeRequired);
            PlayerCanvas.canvas.Shoot();
            muzzleFlash.Play(withChildren: true);
            animator.Play("GunShoot");
            Debug.Log("Shooting!");
            _timer = Cooldown;
        }
    }

    [Command]
    public void CmdShoot(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;
        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out hit, 100.0f, CanHit))
        {
            Debug.Log("You hit " + hit.transform.name);

            switch (hit.collider.tag)
            {
                case "Player":
                    var player = hit.collider.GetComponent<PlayerMain>();
                    if (player != null)
                        player.RpcKill();

                    break;
                case "Trap":
                    var trap = hit.collider.GetComponent<TrapController>();
                    if (trap != null)
                        trap.RpcTrigger();
                    break;
                default:
                    break;
            }

            var particles = Instantiate(plasmaExplosion, hit.point - (direction / 4), plasmaExplosion.transform.rotation);
            NetworkServer.Spawn(particles.gameObject);
        }

    }

    /// <summary>
    /// Get a gun from the map
    /// </summary>
    [ClientRpc]
    public void RpcGetGun()
    {
        if (isLocalPlayer)
        {
            //HasGun = true;
            OnPickingGun.Invoke();
            PlayerCanvas.canvas.PickItem();
        }
    }
}
