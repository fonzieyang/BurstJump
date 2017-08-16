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

public class EmemyCreator : MonoBehaviour {

    public const int MAP_LOW = -50;
    public const int MAP_HIGH = 50;
    public const int MAP_LEFT = -50;
    public const int MAP_RIGHT = 50;

    public GameObject weakEnemyProto_;
    public GameObject attackEnemyProto_;
    public GameObject defEnenmyProto_;
    public GameObject bossProto_;
    public GameObject[] enemyList_;

    public EmemyCreator instance_;

    // Use this for initialization
    void Start () {
        instance_ = this;
    }
	
	// Update is called once per frame
	void Update () {
		// create enemy logic here

	}

    void EnemyKilled(GameObject enemy)
    {
        enemy.transform.position = 
    }
}
