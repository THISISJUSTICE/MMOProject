using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Net;
using UnityEngine;

public class ServerSession : PacketSession
{
	public void Send(IMessage packet)
	{
		ushort size = (ushort)packet.CalculateSize();
		byte[] sendBuffer = new byte[size + 4];
		Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));

		string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
		MsgId msgID = (MsgId)Enum.Parse(typeof(MsgId), msgName);
		Array.Copy(BitConverter.GetBytes((ushort)msgID), 0, sendBuffer, 2, sizeof(ushort));

		Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

		Send(new ArraySegment<byte>(sendBuffer));
	}
	
	public override void OnConnected(EndPoint endPoint)
	{
		Debug.Log($"OnConnected : {endPoint}");

		PacketManager.Instance.CustomHandler = (s, m, i) => 
		{
			PacketQueue.Instance.Push(i, m);
		};
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