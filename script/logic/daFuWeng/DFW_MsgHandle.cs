using System;
using static Room;


public partial class MsgHandler
{
    //大富翁逻辑
    //进入战斗
    public static void MsgEnterDFWBattle(ClientState c, MsgBase msgBase)
    {
        MsgEnterDFWBattle msg = (MsgEnterDFWBattle)msgBase;
        Player player = c.player;
        if (player == null) return;

        Room room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            msg.result = 1;
            player.Send(msg);
            return;
        }
        msg.mapId = 1;
        msg.gameDates = new GameData[room.playerIds.Count];

        int i = 0;
        foreach (string id in room.playerIds.Keys)
        {
            Player p = PlayerManager.GetPlayer(id);
            msg.gameDates[i] = new GameData(p.id, p.name, p.money, p.color, p.playOrder, p.position);
            i++;
        }
        player.Send(msg);
    }
    //同步位置协议
    public static void MsgSaizi(ClientState c, MsgBase msgBase)
    {
        MsgSaizi msg = (MsgSaizi)msgBase;
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
        if(room.battle == null)
        {
            return;
        }
        DFW_Battle battle = (DFW_Battle)room.battle;
        //广播
        msg.name = player.name;
        msg.bushu = new Random().Next(1, 7);
        msg.position = player.position + msg.bushu;
        if (msg.position / 41 >= 1)
        {
            msg.position %= 41;
            msg.position++;
            player.money += 1000;

            MsgUpdateMoney msgUp = new MsgUpdateMoney();
            msgUp.name = player.name;
            msgUp.money = player.money;
            room.Broadcast(msgUp);
        }
        int x = msg.position;
        if (x == 1 || x == 11 || x == 21)
        {
            //无事发生
        }
        else if (x == 6 || x == 16 || x == 26 || x == 36)
        {
            //车站
            msg.result = 1;
        }
        else if (x == 5 || x == 17 || x == 27 || x == 35)
        {
            //惊喜
            msg.result = 2;
            msg.tId = new Random().Next(0, battle.treasures.Count);
            battle.TreasureAction(msg.tId, player);
        }
        else if (x == 7 || x == 15 || x == 25 || x == 37)
        {
            //命运
            msg.result = 3;
            msg.fId = new Random().Next(0, battle.fates.Count);
            battle.FateAction(msg.fId, player);
        }
        else if (x == 31)
        {
            //坐牢
            msg.position = 0;
            msg.result = 4;
        }
        else
        {
            msg.result = 5;
            //房子
            House h = battle.GetHouse(x);
            if (h != null)
            {
                if (h.playerName != "")
                {
                    Player otherPlayer = PlayerManager.GetPlayer(room.GetPlayerId(h.playerName));
                    if (otherPlayer != null)
                    {
                        if (h.state == 0 && h.playerName != player.name && otherPlayer.position != 0)//有人买且不是自己的
                        {
                            player.money -= h.rent[h.level];
                            otherPlayer.money += h.rent[h.level];

                            MsgUpdateMoney msg1 = new MsgUpdateMoney();
                            msg1.name = player.name;
                            msg1.money = player.money;
                            room.Broadcast(msg1);

                            MsgUpdateMoney msg2 = new MsgUpdateMoney();
                            msg2.name = otherPlayer.name;
                            msg2.money = otherPlayer.money;
                            room.Broadcast(msg2);
                        }
                    }     
                }
            }
        }
        //更新数值
        player.position = msg.position;
        player.isSaozi = true;
        room.Broadcast(msg);
    }

    public static void MsgUpdateMoney(ClientState c, MsgBase msgBase)
    {
        MsgUpdateMoney msg = (MsgUpdateMoney)msgBase;
        Player player = c.player;
        if (player == null) return;
        //room
        Room room = RoomManager.GetRoom(player.roomId);
        if (room == null)
        {
            return;
        }
        //status
        if (room.status != Status.FIGHT)
        {
            return;
        }
        msg.name = player.name;
        msg.money += player.money;
        room.Broadcast(msg);
        //更新数据
        player.money = msg.money;
    }
    //结束回合协议
    public static void MsgSkip(ClientState c, MsgBase msgBase)
    {
        MsgSkip msg = (MsgSkip)msgBase;
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
        DFW_Battle battle = (DFW_Battle)room.battle;
        if (!player.isSaozi)
        {
            msg.result = 1;
            room.Broadcast(msg);
            return;
        }
        
        if (player.money < 0)
        {
            player.isPoCan = true;
            battle.cunHuo--;
            foreach (int i in player.property)
            {
                battle.houses[i].state = -1;
                battle.houses[i].playerName = "";
            }
            MsgPoCan msgPoCan = new MsgPoCan();
            msgPoCan.name = player.name;
            msgPoCan.hid = player.property.ToArray();
            room.Broadcast(msgPoCan);
        }
        if (battle.Judgment())
        {
            //某一方胜利，结束战斗
            room.status = Status.PREPARE;
            //发送Result
            MsgBattleDFWResult msgB = new MsgBattleDFWResult();
            //统计信息
            foreach (string id in room.playerIds.Keys)
            {
                Player r = PlayerManager.GetPlayer(id);
                if (!r.isPoCan)
                {
                    msgB.winner = r.name;
                    msgB.money = r.money;
                    msgB.property = r.property.Count;
                    room.Broadcast(msgB);
                    r.data.level++;
                    r.data.win++;
                    r.data.coin += 10;
                    
                }
                else
                {
                    r.data.level++;
                    r.data.lost++;
                    r.data.coin += 5;
                }
            }
            room.battle = null;
            BattleManager.RemoveBattle(battle.id);
            return;
        }
        //广播
        msg.curOrder = (battle.curOrder % room.playerIds.Count) + 1;
        Player p;
        bool b = false;
        while(!b)
        {
            foreach (string id in room.playerIds.Keys)
            {
                p = PlayerManager.GetPlayer(id);
                if (p != null)
                {
                    if (p.playOrder == msg.curOrder)
                    {
                        if (!p.isPoCan && p.position != 0)
                        {
                            b = true;
                            msg.name = p.name;
                        }
                        else if (!p.isPoCan && p.position == 0)
                        {
                            p.position = 11;
                        }
                        break;
                    }
                }
            }
            if(!b) msg.curOrder = (msg.curOrder % room.playerIds.Count) + 1;
        }
        
        //更新数值

        battle.curOrder = msg.curOrder;
        battle.time = 0;
        battle.count++;

        room.Broadcast(msg);
    }

    public static void MsgSend(ClientState c, MsgBase msgBase)
    {
        MsgSend msg = (MsgSend)msgBase;
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
        msg.name = player.name;
        room.Broadcast(msg);
    }

    public static void MsgBuy(ClientState c, MsgBase msgBase)
    {
        MsgBuy msg = (MsgBuy)msgBase;
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
        DFW_Battle battle = (DFW_Battle)room.battle;
        House h = battle.GetHouse(msg.houseId);
        if (h == null)
        {
            return;
        }
        if (player.money < h.price)
        {
            msg.result = 1;
            msg.name = player.name;
            room.Broadcast(msg);
            return;
        }
        if (h.playerName != "")
        {
            msg.result = 2;
            msg.name = player.name;
            room.Broadcast(msg);
            return;
        }
        player.money -= h.price;
        player.property.Add(msg.houseId);
        h.playerName = player.name;
        h.state = 0;

        msg.name = player.name;
        msg.color = player.color;
        msg.money = player.money;
        room.Broadcast(msg);
    }

    public static void MsgBuild(ClientState c, MsgBase msgBase)
    {
        MsgBuild msg = (MsgBuild)msgBase;
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
        DFW_Battle battle = (DFW_Battle)room.battle;
        House h = battle.GetHouse(msg.houseId);
        if (h == null)
        {
            return;
        }
        if (player.money < h.up)
        {
            msg.result = 1;
            msg.name = player.name;
            room.Broadcast(msg);
            return;
        }
        if (h.playerName != player.name)
        {
            msg.result = 2;
            msg.name = player.name;
            room.Broadcast(msg);
            return;
        }
        if (h.level >= h.maxLevel)
        {
            msg.result = 3;
            msg.name = player.name;
            room.Broadcast(msg);
            return;
        }
        player.money -= h.up;

        h.level++;

        msg.houseLevel = h.level;
        msg.maxLevel = h.maxLevel;
        msg.name = player.name;
        msg.money = player.money;
        room.Broadcast(msg);
    }

    public static void MsgSale(ClientState c, MsgBase msgBase)
    {
        MsgSale msg = (MsgSale)msgBase;
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
        DFW_Battle battle = (DFW_Battle)room.battle;
        House h = battle.GetHouse(msg.houseId);
        if (h == null)
        {
            return;
        }
        if (h.playerName != player.name)
        {
            msg.result = 1;
            msg.name = player.name;
            room.Broadcast(msg);
            return;
        }
        if (h.level <= 0)
        {
            msg.result = 2;
            msg.name = player.name;
            room.Broadcast(msg);
            return;
        }
        player.money += h.up / 2;
        h.level--;

        msg.houseLevel = h.level;
        msg.name = player.name;
        msg.money = player.money;
        room.Broadcast(msg);
    }

    public static void MsgPawn(ClientState c, MsgBase msgBase)
    {
        MsgPawn msg = (MsgPawn)msgBase;
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
        DFW_Battle battle = (DFW_Battle)room.battle;
        foreach (int houseId in msg.hid)
        {
            House h = battle.GetHouse(houseId);

            if (h == null)
            {
                return;
            }
            if (h.playerName != player.name)
            {
                msg.result = 1;
                msg.name = player.name;
                room.Broadcast(msg);
                return;
            }
            if (h.state != 0)
            {
                msg.result = 2;
                msg.name = player.name;
                room.Broadcast(msg);
                return;
            }
            player.money += h.price / 2;
            h.state = 1;
        }
        msg.name = player.name;
        msg.money = player.money;
        room.Broadcast(msg);
    }

    public static void MsgRansom(ClientState c, MsgBase msgBase)
    {
        MsgRansom msg = (MsgRansom)msgBase;
        Player player = c.player;
        int mon = 0;
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
        DFW_Battle battle = (DFW_Battle)room.battle;
        foreach (int houseId in msg.hid)
        {
            House h = battle.GetHouse(houseId);

            if (h == null)
            {
                return;
            }
            if (h.playerName != player.name)
            {
                msg.result = 1;
                msg.name = player.name;
                room.Broadcast(msg);
                return;
            }
            if (h.state != 1)
            {
                msg.result = 2;
                msg.name = player.name;
                room.Broadcast(msg);
                return;
            }
            mon += h.price / 2;
        }
        if (player.money < mon)
        {
            msg.result = 3;
            msg.name = player.name;
            room.Broadcast(msg);
            return;
        }
        foreach (int houseId in msg.hid)
        {
            House h = battle.GetHouse(houseId);

            if (h == null)
            {
                return;
            }
            h.state = 0;
        }
        player.money -= mon;

        msg.name = player.name;
        msg.color = player.color;
        msg.money = player.money;
        room.Broadcast(msg);
    }

    public static void MsgVehicle(ClientState c, MsgBase msgBase)
    {
        MsgVehicle msg = (MsgVehicle)msgBase;
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
        if(msg.pos == player.position)
        {
            msg.result = 1;
            msg.name = player.name;
            room.Broadcast(msg);
            return;
        }
        if(player.money < 500)
        {
            msg.result = 2;
            msg.name = player.name;
            room.Broadcast(msg);
            return;
        }
        player.position = msg.pos;
        player.money -= 500;
        MsgUpdateMoney msgUpdateMoney = new MsgUpdateMoney();
        msgUpdateMoney.name = player.name;
        msgUpdateMoney.money = player.money;
        room.Broadcast(msgUpdateMoney);
        msg.name = player.name;
        room.Broadcast(msg);
    }
}


