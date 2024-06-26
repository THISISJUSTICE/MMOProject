﻿using System;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;
using Random = UnityEngine.Random;

class PacketHandler
{
	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
		S_EnterGame enterGamePacket = packet as S_EnterGame;

        Managers.Obj.Add(enterGamePacket.Player, myPlayer:true);
	}

	public static void S_LeaveGameHandler(PacketSession session, IMessage packet){
		S_LeaveGame leaveGamePacket = packet as S_LeaveGame;

		Managers.Obj.Clear();
	}

	public static void S_SpawnHandler(PacketSession session, IMessage packet){
		S_Spawn spawnGamePacket = packet as S_Spawn;

		foreach(ObjectInfo obejct in spawnGamePacket.Objects){
			Managers.Obj.Add(obejct, myPlayer:false);
		}
	}

	public static void S_DespawnHandler(PacketSession session, IMessage packet){
		S_Despawn despawnGamePacket = packet as S_Despawn;

		foreach(int id in despawnGamePacket.ObjectIDs){
			Managers.Obj.Remove(id);
		}
	}

	public static void S_MoveHandler(PacketSession session, IMessage packet){
		S_Move movePacket = packet as S_Move;
		GameObject go = Managers.Obj.FindByID(movePacket.ObjectID);
		if(go == null) return;

		if(Managers.Obj.MyPlayer.id == movePacket.ObjectID) return;

		BaseController bc = go.GetComponent<BaseController>();
		if(bc == null) return;

		bc.PosInfo = movePacket.PosInfo;
	}

    public static void S_SkillHandler(PacketSession session, IMessage packet)
    {
        S_Skill skillPacket = packet as S_Skill;
		GameObject go = Managers.Obj.FindByID(skillPacket.ObjectID);
		if(go == null) return;

		CreatureController cc = go.GetComponent<CreatureController>();
		if(cc == null) return;
		
		cc.UseSkill(skillPacket.Info.SkillID);
    }

    public static void S_ChangeHpHandler(PacketSession session, IMessage packet)
    {
        S_ChangeHp changePacket = packet as S_ChangeHp;
		GameObject go = Managers.Obj.FindByID(changePacket.ObjectID);
		if(go == null) return;

		CreatureController cc = go.GetComponent<CreatureController>();
		if(cc == null) return;
		
		cc.Hp = changePacket.Hp;
    }

	public static void S_DieHandler(PacketSession session, IMessage packet)
    {
        S_Die diePacket = packet as S_Die;
		GameObject go = Managers.Obj.FindByID(diePacket.ObjectID);
		if(go == null) return;

		CreatureController cc = go.GetComponent<CreatureController>();
		if(cc == null) return;
		
		cc.Hp = 0;
		cc.OnDead();
    }

	public static void S_ConnectedHandler(PacketSession session, IMessage packet)
    {
        Debug.Log($"S_ConnectedHadler");
		C_Login loginPacket = new C_Login();
		loginPacket.UniqueID = SystemInfo.deviceUniqueIdentifier;
		Managers.Network.Send(loginPacket);
    }

	public static void S_LoginHandler(PacketSession session, IMessage packet)
    {
        S_Login loginPacket = packet as S_Login;
		Debug.Log($"LoginOk({loginPacket.LoginOk})");
		
		// TODO: 로비 UI에서 캐릭터 보여주고 선택
		if(loginPacket.Players == null || loginPacket.Players.Count == 0){
			C_CreatePlayer createPacket = new C_CreatePlayer();
			createPacket.Name = $"Player_{Random.Range(0, 10000).ToString("0000")}";
			Managers.Network.Send(createPacket);
		}
		else{
			// 무조건 첫번째 로그인
			LobbyPlayerInfo info = loginPacket.Players[0];
			C_EnterGame enterGamePacket = new C_EnterGame();
			enterGamePacket.Name = info.Name;
			Managers.Network.Send(enterGamePacket);
		}
    }

	public static void S_CreatePlayerHandler(PacketSession session, IMessage packet)
    {
        S_CreatePlayer createOkPacket = (S_CreatePlayer)packet;
		
		if(createOkPacket.Player == null){
			C_CreatePlayer createPacket = new C_CreatePlayer();
			createPacket.Name = $"Player_{Random.Range(0, 10000).ToString("0000")}";
			Managers.Network.Send(createPacket);
		}
		else{
			C_EnterGame enterGamePacket = new C_EnterGame();
			enterGamePacket.Name = createOkPacket.Player.Name;
			Managers.Network.Send(enterGamePacket);
		}
    }

	public static void S_ItemListHandler(PacketSession session, IMessage packet)
    {
        S_ItemList itemList = (S_ItemList)packet;

		foreach(ItemInfo item in itemList.Items){
			Debug.Log($"{item.TemplateId} : {item.Count}");

		}
		
		
    }

}
