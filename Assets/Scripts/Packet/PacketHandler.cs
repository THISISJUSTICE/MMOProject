using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler
{
	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
		S_EnterGame enterGamePacket = packet as S_EnterGame;

        Managers.Obj.Add(enterGamePacket.Player, myPlayer:true);
	}

	public static void S_LeaveGameHandler(PacketSession session, IMessage packet){
		S_LeaveGame leaveGamePacket = packet as S_LeaveGame;

		Managers.Obj.RemoveMyPlayer();
	}

	public static void S_SpawnHandler(PacketSession session, IMessage packet){
		S_Spawn spawnGamePacket = packet as S_Spawn;

		foreach(PlayerInfo player in spawnGamePacket.Player){
			Managers.Obj.Add(player, myPlayer:false);
		}
	}

	public static void S_DespawnHandler(PacketSession session, IMessage packet){
		S_Despawn despawnGamePacket = packet as S_Despawn;

		foreach(int id in despawnGamePacket.PlayerID){
			Managers.Obj.Remove(id);
		}
	}

	public static void S_MoveHandler(PacketSession session, IMessage packet){
		S_Move movePacket = packet as S_Move;
		GameObject go = Managers.Obj.FindByID(movePacket.PlayerID);
		if(go == null) return;

		CreatureController cc = go.GetComponent<CreatureController>();
		if(cc == null) return;
		
		cc.PosInfo = movePacket.PosInfo;
	}

    public static void S_SkillHandler(PacketSession session, IMessage packet)
    {
        S_Skill skillPacket = packet as S_Skill;
		GameObject go = Managers.Obj.FindByID(skillPacket.PlayerID);
		if(go == null) return;

		PlayerController pc = go.GetComponent<PlayerController>();
		if(pc == null) return;
		
		pc.UseSkill(skillPacket.Info.SkillID);
    }
}
