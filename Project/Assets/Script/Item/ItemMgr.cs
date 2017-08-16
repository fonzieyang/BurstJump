using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMgr : MonoBehaviour {

    static public ItemMgr instance;

    List<ItemBase> itemList = new List<ItemBase>();
    List<ItemBase> removeList = new List<ItemBase>();

    void Awake()
    {
        instance = this;
    }

    public void AddItem(ItemBase item)
    {
        itemList.Add(item);
    }

    public void RemoveItem(ItemBase item)
    {
        itemList.Remove(item);
        GameObject.Destroy(item.gameObject);
    }       

    public void CheckPick(Vector3 pos)
    {
        for (int i=0; i<itemList.Count; ++i)
        {
            if ((itemList[i].trans.position - pos).magnitude < itemList[i].radius)
            {
                itemList[i].OnPick();
                removeList.Add(itemList[i]);
            }
        }

        for (int i=0; i<removeList.Count; ++i)
        {
            RemoveItem(removeList[i]);
        }
        removeList.Clear();
    }
}
