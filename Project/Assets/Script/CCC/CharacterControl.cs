using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

    public int hpMax = 10;


    public float jumpHeight = 5;
    public float tf = 1;
    public float horizontalSpeed = 3;
    public float g = 9.8f;
    public float hitRadius = 3;
    public float attack = 1;

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
    Transform trans;

    void Awake()
    {
        hp = hpMax;
        trans = transform;
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
            case State.JumpDown:
                {
                    vSpeed = vSpeed - g * 0.5f * deltaTime;
                    var vDelta = vSpeed * deltaTime;                       
                    var hDelta = horizontalSpeed * deltaTime * move.normalized;

                    if (vDelta < 0)
                    {
                        vDelta = 0;
                        SetState(State.JumpDown);
                        DoHit();
                    } 

                    hDelta.y = vDelta;
                    UpdatePos(hDelta);
                }
                break;
            case State.JumpUp:
                {
                    vSpeed = vSpeed - g * 0.5f * deltaTime;
                    var vDelta = vSpeed * deltaTime;
                    var hDelta = horizontalSpeed * deltaTime * move.normalized;
                    hDelta.y = vDelta;
                    UpdatePos(hDelta);
                    if (trans.position.y <= 0)
                    {
                        var pos = trans.position;
                        pos.y = 0;
                        trans.position = pos;
                        SetState(State.Stay);
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
        // TODO: using capsule first

        move = Vector3.zero;
    }

    void SetState(State s)
    {
        this.state = s;
        timeline = 0;

        if (s == State.JumpUp)
        {
            vSpeed = Mathf.Sqrt(2*jumpHeight);
        }
    }

    void UpdatePos(Vector3 delta)
    {
        trans.position += delta;
    }

    void DoHit()
    {
        
    }
}
