using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour {

    [System.NonSerialized]
    public Transform trans;
    public float radius = 5;

    void Awake()
    {
        trans = transform;
    }

    void Start()
    {
        ItemMgr.instance.AddItem(this);
    }

    public virtual void OnPick()
    {
        
    }
}
