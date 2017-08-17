using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObj : MonoBehaviour {

    //public ExploderObject obj;
    public MeshExploder obj;
    public Transform proxyPos;
    public bool closeRenderer = true;

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

    public bool OnHit()
    {
        obj.Explode();

        if (closeRenderer)
        {
            var render = GetComponent<Renderer>();
            if (render != null)
                render.enabled = false;             
        }  

        return closeRenderer;
    }

    public Vector3 GetPos()
    {
        if (proxyPos != null)
        {
            return proxyPos.position;
        }
        else
        {
            return transform.position;
        }
    }
}
