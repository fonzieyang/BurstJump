using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public CharacterControl character;

	void Update () {
        bool isMove = false;
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            isMove = true;
            move += Vector3.forward;
        } 
        else if (Input.GetKey(KeyCode.S))
        {
            isMove = true;
            move += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            isMove = true;
            move += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            isMove = true;
            move += Vector3.right;
        }

        if (isMove)
        {
            move.Normalize();
            character.Move(move);
        }
	}
}
