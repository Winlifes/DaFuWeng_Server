using System;
using System.Collections.Generic;
//大厅玩家信息
public class Player {
	//id
	public string id = "";

	public string name = "";
	//指向ClientState
	public ClientState state;
	//构造函数
	public Player(ClientState state){
		this.state = state;
	}

	//在哪个房间
	public int roomId = -1;

	//数据库数据
	public PlayerData data;

	//局内属性
    public int money = 0;
    public int color = 0;
    public int playOrder = 0;
    public int position = 0;
    public bool isSaozi = false;
    public bool isPoCan = false;
    public bool isGuaJi = false;
    public List<int> property = new List<int>();

    //发送信息
    public void Send(MsgBase msgBase){
		NetManager.Send(state, msgBase);
	}


}


