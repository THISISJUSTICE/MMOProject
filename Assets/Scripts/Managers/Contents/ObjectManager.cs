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
            MyPlayer.CellPos = new Vector3Int(info.PosX, info.PosY, 0);
        }
        else{
            GameObject go = Managers.Resource.Instantiate("Creatures/Player");
            go.name = info.Name;
            _objects.Add(info.PlayerID, go);

            PlayerController pc = go.GetComponent<PlayerController>();
            pc.id = info.PlayerID;
            pc.CellPos = new Vector3Int(info.PosX, info.PosY, 0);
        }
    }

    public void Add(int id, GameObject go){
        _objects.Add(id, go);
    }

    public void Remove(int id){
        _objects.Remove(id);
    }

    public void RemoveMyPlayer(){
        if(MyPlayer == null) return;

        Remove(MyPlayer.id);
        MyPlayer = null;

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
        _objects.Clear();
    }

}
