using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPItem : ItemBase {

    public int addHp;

    public override void OnPick()
    {
        CharacterControl.instance.ModifyHp(addHp);
    }
}
