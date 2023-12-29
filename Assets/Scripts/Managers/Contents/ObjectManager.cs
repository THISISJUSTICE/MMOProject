using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    List<GameObject> objects_ = new List<GameObject>();

    public void Add(GameObject go){
        objects_.Add(go);
    }

    public void Remove(GameObject go){
        objects_.Remove(go);
    }

    public GameObject Find(Vector3Int cellPos){
        foreach(GameObject obj in objects_){
            CreatureController cc = obj.GetComponent<CreatureController>();
            if(cc == null) continue;

            if(cc.CellPos == cellPos) return obj;
        }

        return null;
    }

    public void Clear(){
        objects_.Clear();
    }

}
