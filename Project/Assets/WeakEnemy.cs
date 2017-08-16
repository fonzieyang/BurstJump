using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakEnemy : Enemy
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void CheckAttack(Vector3 position, AttackInfo ai)
    {
        if (ai.attackType == AttackType.normal)
        {
            if ((position - transform.position).magnitude< 0.5f) {
                EmemyCreator.instance_.EnemyKilled(gameObject);
            }
        }
    }

    public override void Recreate()
    {
        return;
    }
}
