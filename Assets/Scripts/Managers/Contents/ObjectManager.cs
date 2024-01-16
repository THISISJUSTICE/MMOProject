using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ObjectManager
{
    public MyPlayerController MyPlayer{get; set;}
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    public static GameObjectType GetObjectTypeByID(int id){
        int type = (id >> 24) & 0x7F;
        return (GameObjectType)type;
    }

    public void Add(ObjectInfo info, bool myPlayer = false){
        GameObjectType objectType = GetObjectTypeByID(info.ObjectID);
        if(objectType == GameObjectType.Player){
            if(myPlayer){
                GameObject go = Managers.Resource.Instantiate("Creatures/MyPlayer");
                go.name = info.Name;
                _objects.Add(info.ObjectID, go);

                MyPlayer = go.GetComponent<MyPlayerController>();
                MyPlayer.id = info.ObjectID;
                MyPlayer.PosInfo = info.PosInfo;
                MyPlayer.Stat = info.StatInfo;
                MyPlayer.SyncPos();
            }
            else{
                GameObject go = Managers.Resource.Instantiate("Creatures/Player");
                go.name = info.Name;
                _objects.Add(info.ObjectID, go);

                PlayerController pc = go.GetComponent<PlayerController>();
                pc.id = info.ObjectID;
                pc.PosInfo = info.PosInfo;
                pc.Stat = info.StatInfo;
                pc.SyncPos();
            }
        }

        else if(objectType == GameObjectType.Monster){
            GameObject go = Managers.Resource.Instantiate("Creatures/Monster");
            go.name = info.Name;
            _objects.Add(info.ObjectID, go);

            MonsterController mc = go.GetComponent<MonsterController>();
            mc.id = info.ObjectID;
            mc.PosInfo = info.PosInfo;
            mc.Stat = info.StatInfo;
            mc.SyncPos();
        }

        else if(objectType == GameObjectType.Projectile){
            GameObject go = Managers.Resource.Instantiate("Creatures/Arrow");
            go.name = "Arrow";
            _objects.Add(info.ObjectID, go);

            ArrowController ac = go.GetComponent<ArrowController>();
            ac.PosInfo = info.PosInfo;
            ac.Stat = info.StatInfo;
            ac.SyncPos();
        }
        
    }

    public void Remove(int id){
        GameObject go = FindByID(id);
        if(go == null) return;

        _objects.Remove(id);
        Managers.Resource.Destroy(go);
    }

    public GameObject FindByID(int id){
        GameObject go = null;
        _objects.TryGetValue(id, out go);
        return go;
    }

    public GameObject FindCreature(Vector3Int cellPos){
        foreach(GameObject obj in _objects.Values){
            CreatureController cc = obj.GetComponent<CreatureController>();
            if(cc == null) continue;

            if(cc.CellPos == cellPos) return obj;
        }

        return null;
    }

    public GameObject Find(Func<GameObject, bool> condition){
        foreach(GameObject obj in _objects.Values){
            CreatureController cc = obj.GetComponent<CreatureController>();
            if(cc == null) continue;

            if(condition(obj)) return obj;
        }

        return null;
    }

    public void Clear(){
        foreach(GameObject obj in _objects.Values){
            Managers.Resource.Destroy(obj);
        }

        _objects.Clear();
        MyPlayer = null;
    }

}
