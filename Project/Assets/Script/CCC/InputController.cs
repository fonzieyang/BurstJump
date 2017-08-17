using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public CharacterControl character;
    public float joystickVal = 0.3f;

	void Update () {
        bool useKeyboard = false;
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
            
        useKeyboard = isMove;

        // no keyboard input
        if (!useKeyboard)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (h > joystickVal)
            {
                isMove = true;
                move += Vector3.right;
            }
            else if (h < -joystickVal)
            {
                isMove = true;
                move += Vector3.left;
            }

            if (v > joystickVal)
            {
                isMove = true;
                move += Vector3.forward;
            }
            else if (v < -joystickVal)
            {
                isMove = true;
                move += Vector3.back;                
            }                
        }


        if (isMove)
        {
            move.Normalize();
            character.Move(move);
        }


        // check button

        bool isDown = Input.GetButtonDown("Fire1");
        if (isDown)
        {
            character.Down();
        }
	}
}
