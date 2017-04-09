using UnityEngine;
using UnityEngine.Networking;

public class PlayerInteraction : NetworkBehaviour
{
    public Camera playerCamera;
    public string ButtonCommand;
    public LayerMask InteractiveLayer;

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetButtonDown(ButtonCommand))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            CmdInteract(ray.origin, ray.direction);
        }
    }

    [Command]
    void CmdInteract(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;
        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out hit, 1.0f, InteractiveLayer, QueryTriggerInteraction.Collide))
        {
            var button = hit.collider.GetComponent<ButtonController>();
            if (button != null)
            {
                button.RpcPress();
            }
        }
    }
}
