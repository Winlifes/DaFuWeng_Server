//进入战场（服务端推送）
public class MsgEnterDFWBattle : MsgBase
{
    public MsgEnterDFWBattle() { protoName = "MsgEnterDFWBattle"; }
    //服务端回
    public GameData[] gameDates { get; set; }
    public int mapId { get; set; } = 1;	//地图，只有一张
    public int result { get; set; } = 0;
}

//同步信息
public class MsgSaizi:MsgBase {
	public MsgSaizi() {protoName = "MsgSaizi"; }
	//服务端补充
	public int bushu { get; set; } = 0;
	public int position { get; set; } = 0;
	public string name { get; set; } = "";
    public int tId { get; set; } = -1;
    public int fId { get; set; } = -1;
    public int result { get; set; } = 0;
}

public class MsgSkip:MsgBase
{
	public MsgSkip() { protoName = "MsgSkip"; }
	public int curOrder { get; set; } = 0;
	public string name { get; set; } = "";
    public int result { get; set; } = 0;
}

public class MsgPoCan:MsgBase
{
	public MsgPoCan() { protoName = "MsgPoCan"; }
	public string name { get; set; } = "";
    public int[] hid { get; set; }
}

public class MsgGuaJi:MsgBase
{
    public MsgGuaJi() { protoName = "MsgGuaJi"; }
    public string name { get; set; } = "";
}

public class MsgUpdateMoney:MsgBase
{
	public MsgUpdateMoney() { protoName = "MsgUpdateMoney"; }
	public string name { get; set; } = "";
	public int money { get; set; } = 0;
}

public class MsgSend : MsgBase
{
    public MsgSend() { protoName = "MsgSend"; }
	public string name { get; set; } = "";
    public string content { get; set; } = "";
}

public class MsgBuy : MsgBase
{
    public MsgBuy() { protoName = "MsgBuy"; }
    public int houseId { get; set; } = 0;
	public string name { get; set; } = "";
	public int color { get; set; } = 0;
	public int money { get; set; } = 0;
    public int result { get; set; } = 0;
}

public class MsgBuild : MsgBase
{
    public MsgBuild() { protoName = "MsgBuild"; }
    public int houseId { get; set; } = 0;
	public int houseLevel { get; set; } = 0;
	public int maxLevel { get; set; } = 0;
    public string name { get; set; } = "";
    public int money { get; set; } = 0;
    public int result { get; set; } = 0;
}

public class MsgSale : MsgBase
{
    public MsgSale() { protoName = "MsgSale"; }
    public int houseId { get; set; } = 0;
    public int houseLevel { get; set; } = 0;
    public string name { get; set; } = "";
    public int money { get; set; } = 0;
    public int result { get; set; } = 0;
}

public class MsgPawn : MsgBase
{
    public MsgPawn() { protoName = "MsgPawn"; }
    public string name { get; set; } = "";
    public int[] hid { get; set; }
    public int money { get; set; } = 0;
    public int result { get; set; } = 0;
}

public class MsgRansom : MsgBase
{
    public MsgRansom() { protoName = "MsgRansom"; }
    public string name { get; set; } = "";
    public int color { get; set; } = 0;
    public int[] hid { get; set; }
    public int money { get; set; } = 0;
    public int result { get; set; } = 0;
}

public class MsgVehicle : MsgBase
{
    public MsgVehicle() { protoName = "MsgVehicle"; }
    public string name { get; set; } = "";
    public int pos { get; set; } = 0;
    public int result { get; set; } = 0;
}

//战斗结果（服务端推送）
public class MsgBattleDFWResult : MsgBase
{
    public MsgBattleDFWResult() { protoName = "MsgBattleDFWResult"; }
    //服务端回
    public string winner { get; set; } = "";	 //获胜的阵营
    public int money { get; set; } = 0;
    public int property { get; set; } = 0;
}