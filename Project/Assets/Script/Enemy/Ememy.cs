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
    public float impactWaveRadius;
    public AttackType attackType;
}

public abstract class Enemy : MonoBehaviour
{
    public abstract bool CheckAttack( AttackInfo ai);
    public abstract void Recreate();
}
