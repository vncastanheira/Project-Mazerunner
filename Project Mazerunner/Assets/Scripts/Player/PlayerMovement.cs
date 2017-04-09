using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    CharacterController c_character;
    public float c_speed;

    // Use this for initialization
    void Start () {
        c_character = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	public void UpdateLocal () {
        var walk = Input.GetAxis("Vertical") * c_character.transform.TransformDirection(Vector3.forward);
        var strafe = Input.GetAxis("Horizontal") * c_character.transform.TransformDirection(Vector3.right);

        c_character.SimpleMove((walk + strafe) * c_speed);
    }
}
