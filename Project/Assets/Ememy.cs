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
    public abstract void CheckAttack(Vector3 position, AttackInfo ai);
    public abstract void Recreate();
}
