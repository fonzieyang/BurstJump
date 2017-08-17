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
    public const int MAP_LOW = -20;
    public const int MAP_HIGH = 20;
    public const int MAP_LEFT = -20;
    public const int MAP_RIGHT = 20;

    public Object weakEnemyProto_;
    public Object attackEnemyProto_;
    public Object defEnenmyProto_;
    public Object bossProto_;
    public List<GameObject> enemyList_ = new List<GameObject>();
    public int enemyNum_;

    static public EnemyCreator instance_;

    void Awake()
    {
        instance_ = this;
    }

    // Use this for initialization
    void Start () {
        for (int i = 0; i < enemyNum_ * 0.7; i++)
        {
            var e = GameObject.Instantiate(weakEnemyProto_, transform) as GameObject;
            var pos = e.transform.position;
            pos.x = Random.Range(MAP_LEFT, MAP_RIGHT);
            pos.z = Random.Range(MAP_RIGHT, MAP_HIGH);
            e.transform.position = pos;
            e.SetActive(true);
        }
        for (int i = 0; i < enemyNum_ * 0.2; i++)
        {
            var e = GameObject.Instantiate(defEnenmyProto_, transform) as GameObject;
            var pos = e.transform.position;
            pos.x = Random.Range(MAP_LEFT, MAP_RIGHT);
            pos.z = Random.Range(MAP_RIGHT, MAP_HIGH);
            e.transform.position = pos;
            e.SetActive(true);
        }
        for (int i = 0; i < enemyNum_ * 0.1; i++)
        {
            var e = GameObject.Instantiate(attackEnemyProto_, transform) as GameObject;
            var pos = e.transform.position;
            pos.x = Random.Range(MAP_LEFT, MAP_RIGHT);
            pos.z = Random.Range(MAP_RIGHT, MAP_HIGH);
            e.transform.position = pos;
            e.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // create enemy logic here
    }

    public void RegisetEnemy(GameObject enemy)
    {
        enemyList_.Add(enemy);
    }

    public void EnemyKilled(GameObject enemy)
    {
        var pos = enemy.transform.position;
        pos.x = Random.Range(MAP_HIGH, MAP_LOW);
        pos.z = Random.Range(MAP_LEFT, MAP_RIGHT);
        enemy.transform.position = pos;
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
        {
            e.Recreate();
        }
    }

    public bool CheckAttack(AttackInfo ai)
    {
        bool result = false;
        foreach (var e in enemyList_)
        {
            var enemy = e.GetComponent<Enemy>();
            result |= enemy.CheckAttack(ai);
        }
        return result;
    }
}
