//房间玩家信息的封装
public class PlayerInfo
{
    public string id { get; set; } = "lpy";   //账号
    public string name { get; set; } = "";
    public int level { get; set; } = 0;
    public int camp { get; set; } = 0;      //阵营
    public int win { get; set; } = 0;           //胜利数
    public int lost { get; set; } = 0;      //失败数
    public int isOwner { get; set; } = 0;		//是否是房主
}