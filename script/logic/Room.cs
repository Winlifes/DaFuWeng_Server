using System;

public class Room
{
    //id
    public int id = 0;
    //最大玩家数
    public int maxPlayer = 6;
    //玩家列表
    public Dictionary<string, bool> playerIds = new Dictionary<string, bool>();

    public Dictionary<string, string> playerNames = new Dictionary<string, string>();

    //房主id
    public string ownerId = "";
    //状态
    public enum Status
    {
        PREPARE = 0,
        FIGHT = 1,
    }
    public Status status = Status.PREPARE;

    public Battle battle = null;

    //添加玩家
    public bool AddPlayer(string id)
    {
        //获取玩家
        Player player = PlayerManager.GetPlayer(id);
        if (player == null)
        {
            Console.WriteLine("room.AddPlayer fail, player is null");
            return false;
        }
        //房间人数
        if (playerIds.Count >= maxPlayer)
        {
            Console.WriteLine("room.AddPlayer fail, reach maxPlayer");
            return false;
        }
        //准备状态才能加人
        if (status != Status.PREPARE)
        {
            Console.WriteLine("room.AddPlayer fail, not PREPARE");
            return false;
        }
        //已经在房间里
        if (playerIds.ContainsKey(id))
        {
            Console.WriteLine("room.AddPlayer fail, already in this room");
            return false;
        }
        //加入列表
        playerIds[id] = true;
        playerNames[player.name] = player.id;
        player.roomId = this.id;
        //设置房主
        if (ownerId == "")
        {
            ownerId = player.id;
        }
        //广播
        Broadcast(ToMsg());
        return true;
    }


    //是不是房主
    public bool IsOwner(Player player)
    {
        return player.id == ownerId;
    }

    //删除玩家
    public bool RemovePlayer(string id)
    {
        //获取玩家
        Player player = PlayerManager.GetPlayer(id);
        if (player == null)
        {
            Console.WriteLine("room.RemovePlayer fail, player is null");
            return false;
        }
        //没有在房间里
        if (!playerIds.ContainsKey(id))
        {
            Console.WriteLine("room.RemovePlayer fail, not in this room");
            return false;
        }
        //删除列表
        playerIds.Remove(id);
        playerNames.Remove(player.name);
        //设置玩家数据
        player.roomId = -1;
        //设置房主
        if (ownerId == player.id)
        {
            ownerId = SwitchOwner();
        }
        //战斗状态退出
        if (status == Status.FIGHT)
        {
            battle.Remove(player);
            MsgGuaJi msg = new MsgGuaJi();
            msg.name = player.name;
            Broadcast(msg);
        }
        //房间为空
        if (playerIds.Count == 0)
        {
            RoomManager.RemoveRoom(this.id);
        }
        //广播
        Broadcast(ToMsg());
        return true;
    }

    //选择房主
    public string SwitchOwner()
    {
        //选择第一个玩家
        foreach (string id in playerIds.Keys)
        {
            return id;
        }
        //房间没人
        return "";
    }


    //广播消息
    public void Broadcast(MsgBase msg)
    {
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.Send(msg);
        }
    }

    //生成MsgGetRoomInfo协议
    public MsgBase ToMsg()
    {
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        int count = playerIds.Count;
        msg.players = new PlayerInfo[count];
        //players
        int i = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            PlayerInfo playerInfo = new PlayerInfo();
            //赋值
            playerInfo.id = player.id;
            playerInfo.name = player.name;
            playerInfo.level = player.data.level;
            playerInfo.win = player.data.win;
            playerInfo.lost = player.data.lost;
            playerInfo.isOwner = 0;
            if (IsOwner(player))
            {
                playerInfo.isOwner = 1;
            }

            msg.players[i] = playerInfo;
            i++;
        }
        return msg;
    }

    //能否开战
    public bool CanStartBattle(int pid)
    {
        //已经是战斗状态
        if (status != Status.PREPARE)
        {
            return false;
        }
        switch(pid)
        {
            case 0:
                if (playerIds.Count < 2 || playerIds.Count > 6)
                {//大富翁
                    return false;
                }
                break;
            case 1:
                if (playerIds.Count != 3)
                {//斗地主
                    return false;
                }
                break;
            case 3:
                if (playerIds.Count != 2)
                {//轮盘赌
                    return false;
                }
                break;
            default: return false;
        }
        return true;
    }

    //开战
    public bool StartBattle(int pid)
    {
        if (!CanStartBattle(pid))
        {
            return false;
        }
        //状态
        status = Status.FIGHT;
        battle = BattleManager.AddBattle(pid);
        battle.Start(this,playerIds.Count);

        return true;
    }




    //定时更新
    public void Update()
    {
        
    }

    public string GetPlayerId(string name)
    {
        if (playerNames.ContainsKey(name))
        {
            return playerNames[name];
        }
        return null;
    }

}


