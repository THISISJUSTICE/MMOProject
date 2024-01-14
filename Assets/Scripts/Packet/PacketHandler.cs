using System;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
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

		CreatureController cc = go.GetComponent<CreatureController>();
		if(cc == null) return;

		cc.PosInfo = movePacket.PosInfo;
	}

    public static void S_SkillHandler(PacketSession session, IMessage packet)
    {
        S_Skill skillPacket = packet as S_Skill;
		GameObject go = Managers.Obj.FindByID(skillPacket.ObjectID);
		if(go == null) return;

		PlayerController pc = go.GetComponent<PlayerController>();
		if(pc == null) return;
		
		pc.UseSkill(skillPacket.Info.SkillID);
    }

    public static void S_ChangeHpHandler(PacketSession session, IMessage packet)
    {
        S_ChangeHp changePacket = packet as S_ChangeHp;
		GameObject go = Managers.Obj.FindByID(changePacket.ObjectID);
		if(go == null) return;

		CreatureController cc = go.GetComponent<CreatureController>();
		if(cc == null) return;
		
		cc.Stat.Hp = changePacket.Hp;
		//TODO: UI
		Debug.Log($"ChangeHP: {changePacket.Hp}");
    }
}
