using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    normal,
}

public struct AttackInfo
{
    public Vector3 position;
    public AttackType attackType;
}

public abstract class Enemy : MonoBehaviour
{
    public abstract void CheckAttack( AttackInfo ai);
    public abstract void Recreate();
}
