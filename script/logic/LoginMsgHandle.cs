
using DaFuWeng_Server;

public partial class MsgHandler
{


	//注册协议处理
	public static void MsgRegister(ClientState c, MsgBase msgBase)
	{
		MsgRegister msg = (MsgRegister)msgBase;
		//注册
		if (DbManager.Register(msg.id, msg.pw, msg.mail))
		{
			msg.result = 0;
		}
		else
		{
			msg.result = 1;
		}
		NetManager.Send(c, msg);
	}

    //版本协议处理
    public static void MsgVersion(ClientState c, MsgBase msgBase)
    {
        MsgVersion msg = (MsgVersion)msgBase;
        //版本对比
        if (msg.verion == Program.version)
        {
            msg.result = 0;
        }
        else
        {
            msg.result = 1;
        }
        NetManager.Send(c, msg);
    }

    //登陆协议处理
    public static void MsgLogin(ClientState c, MsgBase msgBase)
	{
		MsgLogin msg = (MsgLogin)msgBase;
		//密码校验
		if (!DbManager.CheckPassword(msg.id, msg.pw))
		{
			msg.result = 1;
			NetManager.Send(c, msg);
			return;
		}
		//不允许再次登陆
		if (c.player != null)
		{
			msg.result = 1;
			NetManager.Send(c, msg);
			return;
		}
		//如果已经登陆，踢下线
		if (PlayerManager.IsOnline(msg.id))
		{
			//发送踢下线协议
			Player other = PlayerManager.GetPlayer(msg.id);
			MsgKick msgKick = new MsgKick();
			msgKick.reason = 0;
			other.Send(msgKick);
			//断开连接
			NetManager.Close(other.state);
		}
		//获取玩家数据
		PlayerData playerData = DbManager.GetPlayerData(msg.id);
		if (playerData == null)
		{
			msg.result = 2;
			NetManager.Send(c, msg);
			return;
		}
        //构建Player
        Player player = new Player(c);
        player.id = msg.id;
		player.name = DbManager.GetPlayerName(msg.id);
        player.data = playerData;
        PlayerManager.AddPlayer(msg.id, player);
        c.player = player;
        NetManager.Send(c, msg);
	}

	public static void MsgSetName(ClientState c, MsgBase msgBase)
	{
		MsgSetName msg = (MsgSetName)msgBase;
		//获取玩家数据
		PlayerData playerData = DbManager.GetPlayerData(msg.id);
		if (playerData != null)
		{
			msg.result = 1;
			NetManager.Send(c, msg);
			return;
		}
		string name = DbManager.GetPlayerName(msg.id);
		if(name != null)
		{
            msg.result = 1;
            NetManager.Send(c, msg);
            return;
        }
		if(DbManager.IsNameExist(msg.name))
		{
            msg.result = 2;
            NetManager.Send(c, msg);
            return;
        }
		DbManager.SetName(msg.id, msg.name);
        DbManager.CreatePlayer(msg.id);
        //构建Player
        Player player = new Player(c);
		player.id = msg.id;
		player.name = msg.name;
		player.data = new PlayerData();
        PlayerManager.AddPlayer(msg.id, player);
		c.player = player;
		//返回协议
		msg.result = 0;
		player.Send(msg);
	}
}
