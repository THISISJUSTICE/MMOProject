using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerSession : PacketSession
{
	public override void OnConnected(EndPoint endPoint)
	{
		Debug.Log($"OnConnected : {endPoint}");

		S_Chat chat = new S_Chat()
		{
			Context = "안녕하세요"
		};

		ushort size = (ushort)chat.CalculateSize();
		byte[] sendBuffer = new byte[size + 4];
		Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
		ushort protocolID = (ushort)MsgId.SChat;
		Array.Copy(BitConverter.GetBytes(protocolID), 0, sendBuffer, 2, sizeof(ushort));
		Array.Copy(chat.ToByteArray(), 0, sendBuffer, 4, size);
		Debug.Log($"chat size: {chat.CalculateSize()}");
		Send(new ArraySegment<byte>(sendBuffer));
	}

	public override void OnDisconnected(EndPoint endPoint)
	{
		Debug.Log($"OnDisconnected : {endPoint}");
	}

	public override void OnRecvPacket(ArraySegment<byte> buffer)
	{
		//Debug.Log("OnRecvPacket");
		PacketManager.Instance.OnRecvPacket(this, buffer);
	}

	public override void OnSend(int numOfBytes)
	{
		//Debug.Log($"Transferred bytes: {numOfBytes}");
	}
}