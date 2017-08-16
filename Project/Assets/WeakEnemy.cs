using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakEnemy : Enemy
{
    Vector3 movingDircetion_;
    float speed_;
    float nextDirectionUpdateTime_;
    float lastPositionUpdateTime_;

	// Use this for initialization
	void Start () {
        var p = transform.position;
        p.x = Random.Range(EnemyCreator.MAP_LOW, EnemyCreator.MAP_HIGH);
        p.z = Random.Range(EnemyCreator.MAP_LEFT, EnemyCreator.MAP_RIGHT);
        transform.position = p;
        lastPositionUpdateTime_ = Time.time;
    }

    void UpdateDirection()
    {
        movingDircetion_ = new Vector3();
        float dir = Random.Range(0, 2 * Mathf.PI);
        movingDircetion_.x = Mathf.Cos(dir);
        movingDircetion_.z = Mathf.Sin(dir);
        movingDircetion_.y = 0;
        transform.forward = movingDircetion_;
        nextDirectionUpdateTime_ = Time.time + Random.Range(0, 5);
    }
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextDirectionUpdateTime_)
        {
            UpdateDirection();
        }

        var t = Time.time - lastPositionUpdateTime_;
        var pos = transform.position;
        pos += movingDircetion_* t;
        while (pos.x < EnemyCreator.MAP_LOW + 0.1f || pos.x > EnemyCreator.MAP_HIGH - 0.1f || pos.z < EnemyCreator.MAP_LEFT+0.1f || pos.z > EnemyCreator.MAP_RIGHT + 0.1f)
        {
            UpdateDirection();
            t = Time.time - lastPositionUpdateTime_;
            pos = transform.position;
            pos += movingDircetion_ * t;
        }
        transform.position = pos;
        lastPositionUpdateTime_ = Time.time;
    }

    public override void CheckAttack(AttackInfo ai)
    {
        if (ai.attackType == AttackType.normal)
        {
            if ((ai.position - transform.position).magnitude< 0.5f) {
                EnemyCreator.instance_.EnemyKilled(gameObject);
            }
        }
    }

    public override void Recreate()
    {
        return;
    }
}
