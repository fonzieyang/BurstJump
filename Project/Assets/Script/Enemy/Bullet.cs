using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed_ = 10f;
    public Vector3 dir_;


	// Use this for initialization
	void Start () {
		
	}

    void Shooted(Vector3 startPos)
    {
        var characterPos = CharacterControl.instance.transform.position;
        dir_ = (characterPos - startPos).normalized;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
