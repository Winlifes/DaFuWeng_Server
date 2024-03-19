//玩家信息
public class Player
{
    //id
    public string id = "";

    public string name = "";
    //指向ClientState
    public ClientState state;
    //构造函数
    public Player(ClientState state)
    {
        this.state = state;
    }

    //在哪个房间
    public int roomId = -1;

    //数据库数据
    public PlayerData data;

    #region 大富翁局内属性
    public int money = 0;
    public int color = 0;
    public int playOrder = 0;
    public int position = 0;
    public bool isSaozi = false;
    public bool isPoCan = false;
    public bool isGuaJi = false;
    public List<int> property = new List<int>();
    #endregion

    #region 斗地主局内属性

    #endregion

    #region 轮盘赌局内属性
    public int health = 0;
    public List<int> items = new List<int>();//id,num
    public List<LPD_State> lPD_States = new List<LPD_State>();
    #endregion

    //发送信息
    public void Send(MsgBase msgBase)
    {
        NetManager.Send(state, msgBase);
    }


}


