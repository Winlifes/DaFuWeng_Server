﻿using System;


public partial class MsgHandler {
	
	//查询角色信息
	public static void MsgGetPlayerInfo(ClientState c, MsgBase msgBase){
        MsgGetPlayerInfo msg = (MsgGetPlayerInfo)msgBase;
		Player player = c.player;
		if(player == null) return;

		msg.name = player.name;
		msg.level = player.data.level;
		msg.coin = player.data.coin;
		msg.win = player.data.win;
		msg.lost = player.data.lost;

		player.Send(msg);
	}




	//请求房间列表
	public static void MsgGetRoomList(ClientState c, MsgBase msgBase){
		MsgGetRoomList msg = (MsgGetRoomList)msgBase;
		Player player = c.player;
		if(player == null) return;

		player.Send(RoomManager.ToMsg());
	}
	
	//创建房间
	public static void MsgCreateRoom(ClientState c, MsgBase msgBase){
		MsgCreateRoom msg = (MsgCreateRoom)msgBase;
		Player player = c.player;
		if(player == null) return;
		//已经在房间里
		if(player.roomId >= 0 ){
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//创建
		Room room = RoomManager.AddRoom();
		room.AddPlayer(player.id);

		msg.result = 0;
		player.Send(msg);
	}

	//进入房间
	public static void MsgEnterRoom(ClientState c, MsgBase msgBase){
		MsgEnterRoom msg = (MsgEnterRoom)msgBase;
		Player player = c.player;
		if(player == null) return;
		//已经在房间里
		if(player.roomId >=0 ){
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//获取房间
		Room room = RoomManager.GetRoom(msg.id);
		if(room == null){
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//进入
		if(!room.AddPlayer(player.id)){
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//返回协议	
		msg.result = 0;
		player.Send(msg);
	}


	//获取房间信息
	public static void MsgGetRoomInfo(ClientState c, MsgBase msgBase){
		MsgGetRoomInfo msg = (MsgGetRoomInfo)msgBase;
		Player player = c.player;
		if(player == null) return;

		Room room = RoomManager.GetRoom(player.roomId);
		if(room == null){
			player.Send(msg);
			return;
		}

		player.Send(room.ToMsg());
	}

	//离开房间
	public static void MsgLeaveRoom(ClientState c, MsgBase msgBase){
		MsgLeaveRoom msg = (MsgLeaveRoom)msgBase;
		Player player = c.player;
		if(player == null) return;

		Room room = RoomManager.GetRoom(player.roomId);
		if(room == null){
			msg.result = 1;
			player.Send(msg);
			return;
		}

		room.RemovePlayer(player.id);
		//返回协议
		msg.result = 0;
		player.Send(msg);
	}


	//请求开始战斗
	public static void MsgStartBattle(ClientState c, MsgBase msgBase){
		MsgStartBattle msg = (MsgStartBattle)msgBase;
		Player player = c.player;
		if(player == null) return;
		//room
		Room room = RoomManager.GetRoom(player.roomId);
		if(room == null){
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//是否是房主
		if(!room.IsOwner(player)){
			msg.result = 2;
			player.Send(msg);
			return;
		}
		//开战
		if(!room.StartBattle()){
			msg.result = 1;
			player.Send(msg);
			return;
		}
		//成功
		msg.result = 0;
		player.Send(msg);
	}

	public static void MsgEnterBattle(ClientState c, MsgBase msgBase)
	{
		MsgEnterBattle msg = (MsgEnterBattle)msgBase;
        Player player = c.player;
        if (player == null) return;

        Room room = RoomManager.GetRoom(player.roomId);
		if(room == null)
		{
            msg.result = 1;
            player.Send(msg);
            return;
        }
        msg.mapId = 1;
        msg.gameDates = new GameDate[room.playerIds.Count];

		int i = 0;
        foreach (string id in room.playerIds.Keys)
        {
            
            msg.gameDates[i] = new GameDate(player.id, player.name, player.money, player.color, player.playOrder, player.position);
            i++;
        }
        room.Broadcast(msg);
    }

}

