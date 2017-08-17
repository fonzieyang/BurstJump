using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour {

    public const float CHAR_RADIUS = 1.5f;
    static public CharacterControl instance;

    public Slider HPSlider;
    public Text scoreTxt;

    public Image TenDigitImage;
    public Image SingltDigitImage;

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

    public float attackExposiveInterval = 0.1f;

    private int score;
    private int continueHit;


    public enum State
    {
        JumpUp,
        JumpDown,
        Stay,
        Sprint,
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
    public AudioSource jumpGroundSound;

    // effect
    public GameObject explosiveEffect;
    public GameObject bigExpEffect;


    float GetCurG()
    {
        return currentG + attackAcc * attackBonusGFactor;
    }

    public void ModifyHp(int hpDelta)
    {
        hp += hpDelta;
        HPSlider.value = hp;

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

        TenDigitImage.gameObject.SetActive(false);
        SingltDigitImage.gameObject.SetActive(false);
    }
    
    float startSprintTime_;
    float sprintAttackNum;
    float lastSprintUpdateTime_;
    float sprintTime_ = 0.8f;
    Vector3 sprintDir_;
    public float sprintSpeed_ = 10;
    
    bool needSprint_ = false;
    internal void Sprint(Vector3 dir)
    {
        sprintDir_ = dir;
        if (state == State.Stay)
        {
            StartSprint();
        } else if(state == State.JumpUp || state == State.JumpDown)
        {
            needSprint_ = true;
        }
    }

    public void StartSprint()
    {
        state = State.Sprint;
        startSprintTime_ = Time.time;
        lastSprintUpdateTime_ = Time.time;
        needSprint_ = false;
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

                        if (needSprint_)
                        {
                            StartSprint();
                            SetState(State.Sprint);
                        }
                        else
                        {
                            SetState(State.Stay);
                            ItemMgr.instance.CheckPick(trans.position);
                        }
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
            case State.Sprint:
                {
                    var t = Time.time - lastSprintUpdateTime_;
                    var dist = t * sprintSpeed_ * sprintDir_;
                    dist += transform.position;
                    if (dist.x < EnemyCreator.MAP_LEFT || dist.x > EnemyCreator.MAP_RIGHT
                        || dist.z < EnemyCreator.MAP_LEFT || dist.z > EnemyCreator.MAP_RIGHT)
                    {
                        state = State.Stay;
                    }
                    else
                    {
                        transform.position = dist;
                        lastSprintUpdateTime_ = Time.time;

                        float curOffset = lastSprintUpdateTime_ - startSprintTime_;
                        int num = (int)(curOffset / attackExposiveInterval);
                        if (num > sprintAttackNum)
                        {
                            sprintAttackNum = num;
                            DoHit();
                        }

                        if (lastSprintUpdateTime_ > startSprintTime_ + sprintTime_)
                        {
                            state = State.Stay;
                            sprintAttackNum = 0;
                        }
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

        explosiveEffect.SetActive(false);
        explosiveEffect.SetActive(true);

        bool breakSth = BreakMgr.instance.CheckObj(trans.position, hitRadius);

        if (breakSth)
        {
            jumpDownSound.Play();
        }
        else
        {
            jumpGroundSound.Play();
        }

        int num = EnemyCreator.instance_.CheckAttack(atk);

        switch (num)
        {
            case 0:
                //score += 0;
                continueHit = 0;
                break;

            case 1:
                score += 10;

                break;

            case 2:
                score += 30;
                break;

            case 3:
                score += 50;
                break;

            case 4:
                score += 70;
                break;

            default:
                score += num * 2;
                break;
        }

        scoreTxt.text = score.ToString();

        continueHit += num;

        int hundredDigit = continueHit / 100;
        int tenDigit = (continueHit % 100) / 10;
        int singleDigit = (continueHit % 100) % 10;
        
        if (tenDigit != 0)
        {
            TenDigitImage.gameObject.SetActive(true);
            SingltDigitImage.gameObject.SetActive(true);
            TenDigitImage.sprite = Resources.Load<Sprite>("Image/Number" + tenDigit.ToString());
            SingltDigitImage.sprite = Resources.Load<Sprite>("Image/Number" + singleDigit.ToString());
        }
        else
        {
            TenDigitImage.gameObject.SetActive(false);
            if (singleDigit != 0)
            {
                SingltDigitImage.gameObject.SetActive(true);
                SingltDigitImage.sprite = Resources.Load<Sprite>("Image/Number" + singleDigit.ToString());
            }
            else
            {
                SingltDigitImage.gameObject.SetActive(false);
            }
        }
        
        return num != 0;
    }

    void DoExplosive()
    {
        bigExpEffect.SetActive(false);
        bigExpEffect.SetActive(true);
        // TODO:
    }

    void Die()
    {
        
    }

    void OnHit()
    {
        getHurtSound.Play();
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
