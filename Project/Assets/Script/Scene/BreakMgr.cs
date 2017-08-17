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

    public bool CheckObj(Vector3 pos, float radius)
    {
        bool hit = false;

        for (int i=0; i<objList.Count; ++i)
        {            
            if (objList[i] !=null && (objList[i].GetPos() - pos).magnitude < radius)
            {
                hit = true;
                if (objList[i].OnHit())
                    removeList.Add(objList[i]);
            }
        }

        for (int i=0; i<removeList.Count; ++i)
        {
            objList.Remove(removeList[i]);
        }
        removeList.Clear();

        return hit;
    }
}
