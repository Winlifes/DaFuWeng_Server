using static Room;

public partial class MsgHandler
{
    //轮盘赌逻辑
    //进入战斗
    public static void MsgEnterLPDBattle(ClientState c, MsgBase msgBase)
    {
        MsgEnterLPDBattle msg = (MsgEnterLPDBattle)msgBase;
        Player player = c.player;
        if (player == null) return;

        Room room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            msg.result = 1;
            player.Send(msg);
            return;
        }
        msg.gameDatas = new LPD_GameData[room.playerIds.Count];

        int i = 0;
        foreach (string id in room.playerIds.Keys)
        {
            Player p = PlayerManager.GetPlayer(id);
            msg.gameDatas[i] = new LPD_GameData(p.id, p.health, p.name, p.playOrder);
            i++;
        }
        player.Send(msg);
    }

    public static void MsgShot(ClientState c, MsgBase msgBase)
    {
        MsgShot msg = (MsgShot)msgBase;
        Player player = c.player;
        if (player == null) return;
        //room
        Room room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        //status
        if (room.status != Room.Status.FIGHT)
        {
            return;
        }
        //battle
        if (room.battle == null)
        {
            return;
        }
        LPD_Battle battle = (LPD_Battle)room.battle;
        if (player.playOrder != battle.curOrder) return;

        Player otherPlayer = PlayerManager.GetPlayer(room.GetPlayerId(msg.beHitName));
        if (otherPlayer != null)
        {
            if (battle.bullets[battle.bullet-1] == 0)
            {
                msg.isHIt = true;
                if (!otherPlayer.lPD_States.Contains(LPD_State.DEFENSE))
                {
                    otherPlayer.health -= battle.bulletAttack;
                }
                else
                {
                    otherPlayer.lPD_States.Remove(LPD_State.DEFENSE);
                    MsgAddState msgAddState = new MsgAddState();
                    msgAddState.id = otherPlayer.id;
                    msgAddState.states = otherPlayer.lPD_States.ToArray();
                    room.Broadcast(msgAddState);
                }
                
                if(otherPlayer.lPD_States.Contains(LPD_State.UP))
                {
                    otherPlayer.lPD_States.Remove(LPD_State.UP);
                    MsgAddState msgAddState = new MsgAddState();
                    msgAddState.id = otherPlayer.id;
                    msgAddState.states = otherPlayer.lPD_States.ToArray();
                    room.Broadcast(msgAddState);
                }
                msg.health = otherPlayer.health;
                if (otherPlayer.health <= 0)
                {
                    battle.Remove(otherPlayer);
                }

            }
            else
            {
                msg.isHIt = false;
            }
            player.lPD_States.Remove(LPD_State.UP);
            MsgAddState msgAdd = new MsgAddState();
            msgAdd.id = player.id;
            msgAdd.states = player.lPD_States.ToArray();
            room.Broadcast(msgAdd);

            battle.bulletAttack = 1;
            battle.bullet--;
            
            if (player.name == otherPlayer.name && !msg.isHIt)
            {

            }
            else
            {
                battle.curOrder = (battle.curOrder % room.playerIds.Count) + 1;
                Player p = PlayerManager.GetPlayer(battle.curOrder);
                if (p != null)
                {
                    if(p.lPD_States.Contains(LPD_State.LOCKED))
                    {
                        battle.curOrder = (battle.curOrder % room.playerIds.Count) + 1;
                        p.lPD_States.Remove(LPD_State.LOCKED);
                        MsgAddState msgAddState = new MsgAddState();
                        msgAddState.id = p.id;
                        msgAddState.states = p.lPD_States.ToArray();
                        room.Broadcast(msgAddState);
                    }
                    else
                    {
                        p.lPD_States.Clear();
                        MsgAddState msgAddState = new MsgAddState();
                        msgAddState.id = p.id;
                        msgAddState.states = p.lPD_States.ToArray();
                        room.Broadcast(msgAddState);
                    }
                }
            }
            msg.curOrder = battle.curOrder;

            MsgAddState msgA = new MsgAddState();
            msgA.id = player.id;
            msgA.states = player.lPD_States.ToArray();
            room.Broadcast(msgA);

            room.Broadcast(msg);
        }


        
    }

    public static void MsgItem(ClientState c, MsgBase msgBase)
    {
        MsgItem msg = (MsgItem)msgBase;
        Player player = c.player;
        if (player == null) return;
        //room
        Room room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        //status
        if (room.status != Room.Status.FIGHT)
        {
            return;
        }
        //battle
        if (room.battle == null)
        {
            return;
        }
        LPD_Battle battle = (LPD_Battle)room.battle;
        if (player.playOrder != battle.curOrder)
            return;
        if (msg.use)
        {
            if(player.items.Contains(msg.item))
            {
                if(battle.ItemAction(msg.item, player))
                {
                    player.items.Remove(msg.item);
                    msg.id = player.id;
                    room.Broadcast(msg);
                }
                else
                {
                    msg.id = player.id;
                    msg.result = 1;
                    room.Broadcast(msg);
                }
                
            }
            

        }
    }

}
