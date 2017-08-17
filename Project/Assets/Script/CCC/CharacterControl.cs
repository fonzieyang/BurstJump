using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

    static public CharacterControl instance;

    // config
    public Animator anim;
    public int hpMax = 10;
    public float jumpHeight = 5;
    public float tf = 1;
    public float horizontalSpeed = 3;
    public float g = 9.8f;
    public float hitRadius = 3;
    public float attack = 1;
    public float downG = 20;
    public float extraHSpeed = 2;

    public float attackAccCount = 5;
    public float attackBonusHSpeedFactor = 2;
    public float attackBonusHeightFactor = 0.5f;
    public float attackBonusGFactor = 30;
    public float attackBonusStayFactor = 0.5f;
    public float attackExposiveRadius = 10;
    public float attackExposiveStayTime = 2;

    public enum State
    {
        JumpUp,
        JumpDown,
        Stay,
    }

    // role state
    State state = State.Stay;
    float timeline = 0;
    Vector3 move;

    float realHeight;
    float vSpeed;
    [System.NonSerialized]
    public Transform trans;
    [SerializeField]
    int hp;
    [SerializeField]
    float currentG;
    [SerializeField]
    float currentHSpeed;
    [SerializeField]
    int attackAcc;


    // sound
    public AudioSource jumpSound;
    public AudioSource jumpDownSound;
    public AudioSource getHurtSound;
    public AudioSource getItemSound;

    // effect
    public GameObject explosiveEffect;


    float GetCurG()
    {
        return currentG + attackAcc * attackBonusGFactor;
    }

    public void ModifyHp(int hpDelta)
    {
        hp += hpDelta;
        if (hp <= 0)
        {
            Die();
        }

        if (hp > hpMax)
        {
            hp = hpMax;
        }

        if (hpDelta < 0)
        {
            OnHit();
        }
    }


    void Awake()
    {
        hp = hpMax;
        trans = transform;
        instance = this;
        currentG = g;
        currentHSpeed = horizontalSpeed;
    }

    public void Move(Vector3 m)
    {
        move = m;
    }


    void Update()
    {
        // update logic
        float deltaTime = Time.deltaTime;
        timeline += deltaTime;

        switch(state)
        {
            case State.JumpUp:
                {
                    UpdateInAir(deltaTime);
                    if (vSpeed <= 0)
                    {
                        SetState(State.JumpDown);    
                    }
                }
                break;
            case State.JumpDown:
                {
                    UpdateInAir(deltaTime);
                    if (trans.position.y <= 0)
                    {
                        bool hit = DoHit();

                        if (hit)
                        {
                            ++attackAcc;
                        }
                        else
                        {
                            attackAcc = 0;
                        }

                        if (attackAcc > attackAccCount)
                        {
                            attackAcc = 0;
                            DoExplosive();
                        }

                        SetState(State.Stay);
                        ItemMgr.instance.CheckPick(trans.position);
                    }
                }
                break;
            case State.Stay:
                {
                    float stayTime = realHeight * tf + attackAcc * attackBonusStayFactor;
                    if (timeline < stayTime)
                        break;
                    if (move != Vector3.zero)
                    {
                        SetState(State.JumpUp);
                    }
                }
                break;
        }

        if (move != Vector3.zero)
        {
            trans.forward = move.normalized;
        }

        // update anim
        anim.SetBool("OnGround", state == State.Stay);
        anim.SetFloat("Jump", vSpeed);

        move = Vector3.zero;
    }

    void SetState(State s)
    {
        if (state == State.JumpUp)
        {
            currentHSpeed = horizontalSpeed;
        }

        this.state = s;
        timeline = 0;

        if (s == State.JumpUp)
        {
            vSpeed = Mathf.Sqrt(2*(jumpHeight + attackAcc*attackBonusHeightFactor)*GetCurG());
        }

        switch(s)
        {
            case State.JumpDown:
                
                break;
            case State.JumpUp:
                jumpSound.Play();
                break;
        }

        currentG = g;

    }

    void UpdatePos(Vector3 delta)
    {
        var newpos = trans.position + delta;
        newpos.x = Mathf.Clamp(newpos.x, (float)EnemyCreator.MAP_LEFT, (float)EnemyCreator.MAP_RIGHT);
        newpos.z = Mathf.Clamp(newpos.z, (float)EnemyCreator.MAP_LOW, (float)EnemyCreator.MAP_HIGH);
        newpos.y = Mathf.Max(newpos.y, 0);
        trans.position = newpos;
    }

    void UpdateInAir(float deltaTime)
    {
        vSpeed = vSpeed - GetCurG() * 0.5f * deltaTime;
        var vDelta = vSpeed * deltaTime;
        var hDelta = (currentHSpeed + attackAcc * attackBonusHSpeedFactor) * deltaTime * move.normalized;
        hDelta.y = vDelta;
        UpdatePos(hDelta);
    }

    bool DoHit()
    {
        AttackInfo atk = new AttackInfo();
        atk.attackType = AttackType.normal;
        atk.position = trans.position;
        atk.impactWaveRadius = hitRadius;

        jumpDownSound.Play();
        explosiveEffect.SetActive(false);
        explosiveEffect.SetActive(true);

        BreakMgr.instance.CheckObj(trans.position, hitRadius);

        return EnemyCreator.instance_.CheckAttack(atk) != 0;
    }

    void DoExplosive()
    {
        
    }

    void Die()
    {
        
    }

    void OnHit()
    {
        //getHurtSound.Play();
    }

    public void Down()
    {
        if (state == State.JumpDown || (state == State.JumpUp && timeline > 0.3f))
        {
            currentG = g + downG;
            currentHSpeed = horizontalSpeed + extraHSpeed;
        }
    }
}
