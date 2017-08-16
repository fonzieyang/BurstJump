using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    weak,
    attack,
    def,
    boss,
}

public class EnemyCreator : MonoBehaviour {
    public const int MAP_LOW = -50;
    public const int MAP_HIGH = 50;
    public const int MAP_LEFT = -50;
    public const int MAP_RIGHT = 50;

    public GameObject weakEnemyProto_;
    public GameObject attackEnemyProto_;
    public GameObject defEnenmyProto_;
    public GameObject bossProto_;
    public GameObject[] enemyList_;

    static public EnemyCreator instance_;

    // Use this for initialization
    void Start () {
        instance_ = this;
    }
	
	// Update is called once per frame
	void Update () {
		// create enemy logic here

	}

    public void EnemyKilled(GameObject enemy)
    {
        var pos = enemy.transform.position;
        pos.x = Random.Range(MAP_HIGH, MAP_LOW);
        pos.z = Random.Range(MAP_LEFT, MAP_RIGHT);
        transform.position = pos;
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
        {
            e.Recreate();
        }
    }
}
