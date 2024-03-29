﻿//查询玩家信息
public class MsgGetPlayerInfo:MsgBase {
	public MsgGetPlayerInfo() {protoName = "MsgGetPlayerInfo"; }
	//服务端回
	public string name { get; set; } = "";
	public int level { get; set; } = 0;
	public int coin { get; set; } = 0;
	public int win { get; set; } = 0;
	public int lost { get; set; } = 0;
}

//房间信息
public class RoomInfo{
	public int id { get; set; } = 0;		//房间id
	public int count { get; set; } = 0;	//人数
	public int status { get; set; } = 0;	//状态0-准备中 1-战斗中
}

//请求房间列表
public class MsgGetRoomList:MsgBase {
	public MsgGetRoomList() {protoName = "MsgGetRoomList";}
	//服务端回
	public RoomInfo[] rooms { get; set; }
}

//创建房间
public class MsgCreateRoom:MsgBase {
	public MsgCreateRoom() {protoName = "MsgCreateRoom";}
	//服务端回
	public int result { get; set; } = 0;
}




//进入房间
public class MsgEnterRoom:MsgBase {
	public MsgEnterRoom() {protoName = "MsgEnterRoom";}
	//客户端发
	public int id { get; set; } = 0;
	//服务端回
	public int result { get; set; } = 0;
}




//获取房间信息
public class MsgGetRoomInfo:MsgBase {
	public MsgGetRoomInfo() {protoName = "MsgGetRoomInfo";}
	//服务端回
	public int id { get; set; } = 0;
	public PlayerInfo[] players { get; set; }
	public int result { get; set; } = 0;
}

//离开房间
public class MsgLeaveRoom:MsgBase {
	public MsgLeaveRoom() {protoName = "MsgLeaveRoom";}
	//服务端回
	public int result { get; set; } = 0;
}

//开战
public class MsgStartBattle:MsgBase {
	public MsgStartBattle() {protoName = "MsgStartBattle";}
	//战局类型
	public int pid { get; set; } = 0;//0:大富翁	1：斗地主  2:轮盘赌
	//服务端回
	public int result { get; set; } = 0;
}