using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObj : MonoBehaviour {

    //public ExploderObject obj;

    void Start()
    {
        //if (obj == null)
        {
      //      obj = gameObject.GetComponent<ExploderObject>();
            //if (obj == null)
              //  obj = gameObject.AddComponent<ExploderObject>();

            gameObject.isStatic = false;
        }

        BreakMgr.instance.AddObj(this);
    }
}
