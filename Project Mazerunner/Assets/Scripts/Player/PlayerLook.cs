using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController))]
public class PlayerLook : NetworkBehaviour
{
    public Camera p_Camera;
    public LayerMask ignoreLayer;

    CharacterController p_character;
    bool lockedCursor = true;
    public float lean = 0.3f;

    void Start()
    {
        if (isLocalPlayer)
            transform.name = "Local Player";

        p_character = GetComponent<CharacterController>();
        ignoreLayer = LayerMask.GetMask("Player");
    }

    public void UpdateLocal()
    {
        //if (!isLocalPlayer)
        //    return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Look(mouseX, mouseY);
        //Lean();
        CursorLock();
    }

    void Look(float mouseX, float mouseY)
    {
        p_character.transform.rotation *= Quaternion.Euler(0f, mouseX, 0f);
        p_Camera.transform.localRotation *= Quaternion.Euler(-mouseY, 0f, 0f);
        p_Camera.transform.localRotation = ClampRotation(p_Camera.transform.localRotation, -90f, 90f);
    }

    void Lean()
    {
        var pos = p_Camera.transform.localPosition;
        if (Input.GetKey(KeyCode.E))
        {
            p_Camera.transform.localPosition = Vector3.MoveTowards(pos, Vector3.right * lean, 0.07f);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            p_Camera.transform.localPosition = Vector3.MoveTowards(pos, Vector3.right * lean * -1, 0.07f);
        }
        else
        {
            p_Camera.transform.localPosition = Vector3.MoveTowards(pos, Vector3.zero, 0.07f);
        }
    }

    void CursorLock()
    {
        // Unlock cursor from game windwo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lockedCursor = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lockedCursor = true;
        }

        if (lockedCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // From Standart Assets
    Quaternion ClampRotation(Quaternion q, float minAngle, float maxAngle)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, minAngle, maxAngle);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
