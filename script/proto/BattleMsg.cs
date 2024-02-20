//进入战场（服务端推送）
public class MsgEnterBattle:MsgBase {
	public MsgEnterBattle() {protoName = "MsgEnterBattle";}
	//服务端回
	public GameData[] gameDates { get; set; }
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