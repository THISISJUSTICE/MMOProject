using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ObjectManager
{
    public MyPlayerController MyPlayer{get; set;}
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    public void Add(PlayerInfo info, bool myPlayer = false){
        if(myPlayer){
            GameObject go = Managers.Resource.Instantiate("Creatures/MyPlayer");
            go.name = info.Name;
            _objects.Add(info.PlayerID, go);

            MyPlayer = go.GetComponent<MyPlayerController>();
            MyPlayer.id = info.PlayerID;
            MyPlayer.PosInfo = info.PosInfo;
        }
        else{
            if(info.PlayerID == MyPlayer.id) return;
            GameObject go = Managers.Resource.Instantiate("Creatures/Player");
            go.name = info.Name;
            if(!_objects.ContainsKey(info.PlayerID))
                _objects.Add(info.PlayerID, go);

            PlayerController pc = go.GetComponent<PlayerController>();
            pc.id = info.PlayerID;
            pc.PosInfo = info.PosInfo;
        }
    }

    public void Remove(int id){
        GameObject go = FindByID(id);
        if(go == null) return;

        _objects.Remove(id);
        Managers.Resource.Destroy(go);
    }

    public void RemoveMyPlayer(){
        if(MyPlayer == null) return;

        Remove(MyPlayer.id);
        MyPlayer = null;

    }

    public GameObject FindByID(int id){
        GameObject go = null;
        _objects.TryGetValue(id, out go);
        return go;
    }

    public GameObject Find(Vector3Int cellPos){
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
    }

}
