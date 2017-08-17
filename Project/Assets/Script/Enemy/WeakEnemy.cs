﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakEnemy : Enemy
{
    Vector3 movingDircetion_;
    float speed_;
    float nextDirectionUpdateTime_;
    float lastPositionUpdateTime_;
    bool isFleeting_ = false;
    float fleetLen_ = 2;

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
        var characterPos = CharacterControl.instance.transform.position;
        if (!isFleeting_ && (characterPos - transform.position).magnitude < fleetLen_)
        {
            BeginFleet(characterPos);
        }
        if (isFleeting_ && (characterPos - transform.position).magnitude > fleetLen_)
        {
            isFleeting_ = false;
            UpdateDirection();
        }
    }

    private void BeginFleet(Vector3 characterPos)
    {
        isFleeting_ = true;
        var dir = transform.position - characterPos;
        float angle = Vector3.Angle(new Vector3(1, 0, 0), dir);
        angle += Random.Range(-90, 90);
        movingDircetion_.z = Mathf.Sin(angle * Mathf.Rad2Deg);
        movingDircetion_.x = Mathf.Sin(angle * Mathf.Rad2Deg);
        transform.forward = movingDircetion_;
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
        isFleeting_ = false;
        return;
    }
}