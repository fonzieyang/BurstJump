using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObj : MonoBehaviour {

    //public ExploderObject obj;
    public MeshExploder obj;

    void Start()
    {
        if (obj == null)
        {
            obj = gameObject.GetComponent<MeshExploder>();
            if (obj == null)
                obj = gameObject.AddComponent<MeshExploder>();

            obj.useGravity = true;
            obj.fadeWaitTime = 4;
        }

        BreakMgr.instance.AddObj(this);
    }
}
