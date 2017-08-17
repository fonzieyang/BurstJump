using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakEnemy : Enemy
{
    public GameObject bulletProto_;
    Vector3 movingDircetion_;
    public Animator anim;
    float speed_;
    float nextDirectionUpdateTime_;
    float lastPositionUpdateTime_;
    bool isFleeting_ = false;
    float fleetLen_ = 2;
    float attackRange_ = 10;
    float attackCdTime_ = 3;
    float lastAttackTime_ = 0;
    bool isAttacking_ = false;
    float attackStartTime_ = 0;

    // Use this for initialization
    void Start () {
        EnemyCreator.instance_.RegisetEnemy(gameObject);
        var p = transform.position;
        p.x = Random.Range(EnemyCreator.MAP_LOW, EnemyCreator.MAP_HIGH);
        p.z = Random.Range(EnemyCreator.MAP_LEFT, EnemyCreator.MAP_RIGHT);
        transform.position = p;
        lastPositionUpdateTime_ = Time.time;
        Run();
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
        if (isAttacking_ == true)
        {

            if (Time.time > attackStartTime_ + 2)
            {
                isAttacking_ = false;
                BasicAttack();
            }
            else
            {
                lastPositionUpdateTime_ = Time.time;
                return;
            }
        }
        
        if (Time.time > nextDirectionUpdateTime_)
        {
            UpdateDirection();
        }

        var t = Time.time - lastPositionUpdateTime_;
        var pos = transform.position;
        pos += movingDircetion_* t;
        int counter = 0;
        while (pos.x < EnemyCreator.MAP_LOW + 0.1f || pos.x > EnemyCreator.MAP_HIGH - 0.1f || pos.z < EnemyCreator.MAP_LEFT+0.1f || pos.z > EnemyCreator.MAP_RIGHT + 0.1f)
        {
            UpdateDirection();
            t = Time.time - lastPositionUpdateTime_;
            pos = transform.position;
            pos += movingDircetion_ * t;
            counter++;
            if (counter > 3)
            {
                movingDircetion_ = -transform.position;
                movingDircetion_ = movingDircetion_.normalized;
                t = Time.time - lastPositionUpdateTime_;
                pos = transform.position;
                pos += movingDircetion_ * t;
                break;
            }
        }
        transform.position = pos;
        lastPositionUpdateTime_ = Time.time;
        var characterPos = CharacterControl.instance.transform.position;
        if (!isFleeting_)
        {
            var dis = (characterPos - transform.position).magnitude;
            if (dis < fleetLen_)
            {
                BeginFleet(characterPos);
            }
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
        angle += Random.Range(-10, 10);
        movingDircetion_.z = Mathf.Sin(angle * Mathf.Rad2Deg);
        movingDircetion_.x = Mathf.Sin(angle * Mathf.Rad2Deg);
        transform.forward = movingDircetion_;
    }

    public override bool CheckAttack(AttackInfo ai)
    {
        bool result = false;
        if (ai.attackType == AttackType.normal)
        {
            var dis = ai.position - transform.position;
            dis.y = 0;
            if (dis.magnitude< ai.impactWaveRadius) {
                result = true;
                EnemyCreator.instance_.EnemyKilled(gameObject);
            }
        }
        return result;
    }

    public override void Recreate()
    {
        isFleeting_ = false;
        EnemyCreator.instance_.DeleteEnemy(gameObject);
        return;
    }

    int scream;
    int basicAttack;
    int getHit;
    int walk;
    int die;
    int run;


    void Awake()
    {
        anim = GetComponent<Animator>();
        scream = Animator.StringToHash("Scream");
        basicAttack = Animator.StringToHash("Basic Attack");
        getHit = Animator.StringToHash("Get Hit");
        walk = Animator.StringToHash("Walk");
        die = Animator.StringToHash("Die");
        run = Animator.StringToHash("Run");
    }


    public void Scream()
    {
        anim.SetTrigger(scream);
    }

    public void BasicAttack()
    {
        anim.SetTrigger(basicAttack);
    }

    public void GetHit()
    {
        anim.SetTrigger(getHit);
    }

    public void Walk()
    {
        anim.SetTrigger(walk);
    }

    public void Die()
    {
        anim.SetTrigger(die);
    }

    public void Run()
    {
        anim.SetTrigger(run);
    }
}