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
    int hp;
    float realHeight;
    float vSpeed;
    [System.NonSerialized]
    public Transform trans;
    [SerializeField]
    float currentG;
    [SerializeField]
    float currentHSpeed;

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
                    vSpeed = vSpeed - currentG * 0.5f * deltaTime;
                    var vDelta = vSpeed * deltaTime;                       
                    var hDelta = currentHSpeed * deltaTime * move.normalized;

                    if (vDelta < 0)
                    {
                        vDelta = 0;
                        SetState(State.JumpDown);
                    } 

                    hDelta.y = vDelta;
                    UpdatePos(hDelta);
                }
                break;
            case State.JumpDown:
                {
                    vSpeed = vSpeed - currentG * 0.5f * deltaTime;
                    var vDelta = vSpeed * deltaTime;
                    var hDelta = currentHSpeed * deltaTime * move.normalized;
                    hDelta.y = vDelta;
                    UpdatePos(hDelta);
                    if (trans.position.y <= 0)
                    {
                        DoHit();
                        SetState(State.Stay);
                        ItemMgr.instance.CheckPick(trans.position);
                    }
                }
                break;
            case State.Stay:
                {
                    float stayTime = realHeight * tf;
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
            vSpeed = Mathf.Sqrt(2*jumpHeight*currentG);
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

    void DoHit()
    {
        AttackInfo atk = new AttackInfo();
        atk.attackType = AttackType.normal;
        atk.position = trans.position;
        atk.impactWaveRadius = hitRadius;

        EnemyCreator.instance_.CheckAttack(atk);
    }

    public void Down()
    {
        if (state == State.JumpDown)
        {
            currentG = g + downG;
            currentHSpeed = horizontalSpeed + extraHSpeed;
        }
    }
}
