using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour {
    public GameObject bulletProto_;
    public void ReleaseBullet()
    {
        var b = Instantiate(bulletProto_, null);
        var bullet = b.GetComponent<Bullet>();
        bullet.Shooted(transform.position);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
