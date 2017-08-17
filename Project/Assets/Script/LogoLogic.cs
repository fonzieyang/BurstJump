using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoLogic : MonoBehaviour {
	
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("GameScene2");
        }
	}
}
