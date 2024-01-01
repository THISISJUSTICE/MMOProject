﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        Managers.Map.LoadMap(1);
        
        GameObject player = Managers.Resource.Instantiate("Creatures/Player");
        player.name = "Player";
        Managers.Obj.Add(player);

        for(int i=0; i<5; i++){
            GameObject monster = Managers.Resource.Instantiate("Creatures/Monster");
            monster.name = $"Monster{i + 1}";

            Vector3Int pos = new Vector3Int(){
                x = Random.Range(-15, 15),
                y = Random.Range(-5, 15)

            };

            MonsterController mc = monster.GetComponent<MonsterController>();
            mc.CellPos = pos;

            Managers.Obj.Add(monster);
        }

    }

    public override void Clear()
    {
        
    }
}
