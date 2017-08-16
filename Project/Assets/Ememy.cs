using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    normal,
}

public struct AttackInfo
{
    AttackType attackType;
}

public abstract class Enemy : MonoBehaviour
{
    public uint hp_;

    public abstract void CheckAttack(Vector3 position, AttackInfo ai)
    {
        if (ai.attackType == AttackType.normal)
        {
            if ((position - transform.position).magnitude < 0.5f) {
                // 
                Destroy(gameObject);
            }
        }
    }
}
