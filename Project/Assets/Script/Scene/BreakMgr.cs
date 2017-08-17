using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakMgr : MonoBehaviour {

    static public BreakMgr instance;

    List<BreakObj> objList = new List<BreakObj>();
    List<BreakObj> removeList = new List<BreakObj>();

    void Awake()
    {
        instance = this;
    }

    public void AddObj(BreakObj obj)
    {
        objList.Add(obj);
    }

    public void CheckObj(Vector3 pos, float radius)
    {
        for (int i=0; i<objList.Count; ++i)
        {            
            if ((objList[i].transform.position - pos).magnitude < radius)
            {
                //objList[i].obj.Explode();
                objList[i].obj.Explode();
                var render = objList[i].GetComponent<Renderer>();
                if (render != null)
                    render.enabled = false;
                removeList.Add(objList[i]);
            }
        }

        for (int i=0; i<removeList.Count; ++i)
        {
            objList.Remove(removeList[i]);
        }
        removeList.Clear();
    }
}
