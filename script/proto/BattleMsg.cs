
//信息
using System.Collections.Generic;

[System.Serializable]
public class GameDate
{
    public string id { get; set; } = "";  //玩家id
    public string name { get; set; } = "";
    public int money { get; set; } = 0;
    public int color { get; set; } = 0;
    public int playOrder { get; set; } = 0;
    public int position { get; set; } = 0;
    public bool isSaozi { get; set; } = false;
    public bool isPoCan { get; set; } = false;
    public bool isGuaJi { get; set; } = false;
    public List<int> property { get; set; } = new List<int>();
    public GameDate(string id, string name, int money, int color, int order, int position)
    {
        this.id = id;
        this.name = name;
        this.money = money;
        this.color = color;
        this.playOrder = order;
        this.position = position;
        this.isSaozi = false;
        isPoCan = false;
        isGuaJi = false;
    }
}


//进入战场（服务端推送）
public class MsgEnterBattle:MsgBase {
	public MsgEnterBattle() {protoName = "MsgEnterBattle";}
	//服务端回
	public GameDate[] gameDates { get; set; }
	public int mapId { get; set; } = 1;	//地图，只有一张
    public int result { get; set; } = 0;
}

//战斗结果（服务端推送）
public class MsgBattleResult:MsgBase {
	public MsgBattleResult() {protoName = "MsgBattleResult";}
	//服务端回
	public string winner { get; set; } = "";	 //获胜的阵营
    public int money { get; set; } = 0;
    public int property { get; set; } = 0;
}

//玩家退出（服务端推送）
public class MsgLeaveBattle:MsgBase {
	public MsgLeaveBattle() {protoName = "MsgLeaveBattle";}
	//服务端回
	public string id { get; set; } = "";	//玩家id
}