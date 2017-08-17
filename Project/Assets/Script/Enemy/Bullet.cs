using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed_ = 0.1f;
    public float effectRange_ = 0.5f;
    public Vector3 dir_;
    float lastUpdateTime_;


	// Use this for initialization
	void Start () {
		
	}

    public void Shooted(Vector3 startPos)
    {
        var characterPos = CharacterControl.instance.transform.position;
        dir_ = (characterPos - startPos).normalized;
        transform.position = startPos;
        lastUpdateTime_ = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        var t = Time.time - lastUpdateTime_;
        var pos = transform.position + t * dir_ * speed_;
        lastUpdateTime_ = Time.time;
        if (pos.x > EnemyCreator.MAP_RIGHT || pos.x < EnemyCreator.MAP_LEFT || pos.z < EnemyCreator.MAP_LEFT || pos.z > EnemyCreator.MAP_RIGHT)
        {
            Destroy(gameObject);
        }
        transform.position = pos;
        var dis = transform.position - CharacterControl.instance.transform.position;
        if (dis.magnitude < effectRange_)
        {
            // boom
            Destroy(gameObject);
        }
    }
}
