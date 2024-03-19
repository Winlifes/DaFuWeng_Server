using static Room;

public class DFW_Battle:Battle
{
    //初始资金
    private static int Mon = 2000;
    //房子
    public Dictionary<int, House> houses = new Dictionary<int, House>();
    // 宝箱
    public List<Treasure> treasures = new List<Treasure>();
    // 命运
    public List<Fate> fates = new List<Fate>();

    public override void Start(Room r, int c)
    {
        base.Start(r, c);
        HouseInit();
        TreasureInit();
        FateInit();
        ResetPlayers();
        SetPlayers();
    }

    public override void Update()
    {
        SkipJudge();
    }

    public override void Remove(Player player)
    {
        base.Remove(player);
        player.data.lost++;
    }
    private void StateInit()
    {

    }

    private void HouseInit()
    {
        houses.Clear();
        houses.Add(2, new House(2, "教廷", 3, 200, 100, new int[] { 100, 200, 400, 800 }));
        houses.Add(3, new House(3, "利比亚", 3, 50, 50, new int[] { 25, 50, 100, 200 }));
        houses.Add(4, new House(4, "苏丹", 3, 60, 50, new int[] { 30, 60, 120, 240 }));
        houses.Add(6, new House(6, "日本站", 0, 100, 0, new int[] { 50 }));
        houses.Add(8, new House(8, "土耳其", 3, 100, 50, new int[] { 50, 100, 200, 400 }));
        houses.Add(9, new House(9, "希腊", 3, 100, 50, new int[] { 50, 100, 200, 400 }));
        houses.Add(10, new House(10, "保加利亚", 3, 120, 80, new int[] { 60, 120, 240, 480 }));
        houses.Add(12, new House(12, "波兰", 3, 150, 80, new int[] { 75, 150, 300, 600 }));
        houses.Add(13, new House(13, "俄罗斯", 3, 250, 100, new int[] { 125, 250, 500, 800 }));
        houses.Add(14, new House(14, "乌克兰", 3, 180, 80, new int[] { 90, 180, 360, 720 }));
        houses.Add(16, new House(16, "西班牙站", 0, 100, 0, new int[] { 50 }));
        houses.Add(18, new House(18, "立陶宛", 3, 200, 100, new int[] { 100, 200, 400, 800 }));
        houses.Add(19, new House(19, "拉脱维亚", 3, 200, 100, new int[] { 100, 200, 400, 800 }));
        houses.Add(20, new House(20, "艾欧尼亚", 3, 220, 100, new int[] { 110, 220, 440, 800 }));
        houses.Add(22, new House(22, "挪威", 3, 220, 100, new int[] { 110, 220, 440, 800 }));
        houses.Add(23, new House(23, "瑞典", 3, 220, 100, new int[] { 110, 220, 440, 800 }));
        houses.Add(24, new House(24, "芬兰", 3, 240, 100, new int[] { 120, 240, 480, 800 }));
        houses.Add(26, new House(26, "美国站", 0, 100, 0, new int[] { 50 }));
        houses.Add(28, new House(28, "德国", 3, 280, 100, new int[] { 140, 280, 560, 1000 }));
        houses.Add(29, new House(29, "法国", 3, 260, 100, new int[] { 130, 260, 520, 1000 }));
        houses.Add(30, new House(30, "英国", 3, 300, 150, new int[] { 150, 300, 600, 1200 }));
        houses.Add(32, new House(32, "加拿大", 3, 300, 150, new int[] { 150, 300, 600, 1200 }));
        houses.Add(33, new House(33, "美国", 3, 300, 150, new int[] { 150, 300, 600, 1200 }));
        houses.Add(34, new House(34, "墨西哥", 3, 320, 150, new int[] { 160, 320, 640, 1200 }));
        houses.Add(36, new House(36, "英国站", 0, 100, 0, new int[] { 50 }));
        houses.Add(38, new House(38, "迪拜", 3, 360, 150, new int[] { 180, 360, 720, 1200 }));
        houses.Add(39, new House(39, "夏威夷", 3, 400, 200, new int[] { 200, 400, 800, 1600 }));
        houses.Add(40, new House(40, "黑子之家", 3, 999, 500, new int[] { 500, 1000, 1500, 2000 }));
    }

    private void TreasureInit()
    {
        treasures.Clear();
        treasures.Add(new Treasure(0, "今天生日", "每人给你￥100零花钱"));
        treasures.Add(new Treasure(1, "做慈善", "你给每人￥100救济金"));
    }

    private void FateInit()
    {
        fates.Clear();
        fates.Add(new Fate(0, "土地神到", "随机在地图上建造一栋房子"));
        fates.Add(new Fate(1, "龙卷风来袭", "随机带走地图上的一栋房子"));
    }



    public void SkipJudge()
    {
        time++;
        if (time >= 1800)
        {
            //寻找当前回合玩家
            foreach (string id in room.playerIds.Keys)
            {
                Player player = PlayerManager.GetPlayer(id);
                if (player.playOrder == curOrder)
                {
                    if (!player.isSaozi)//当前回合没塞
                    {
                        Room room = RoomManager.GetRoom(player.roomId);
                        if(room != null)
                        {
                            MsgSaizi m = new MsgSaizi();
                            player.Send(m);
                        }
                    }
                    player.isSaozi = false;
                    if (player.money < 0)//当前破产玩家数据刷新
                    {
                        player.isPoCan = true;
                        cunHuo--;
                        foreach (int i in player.property)
                        {
                            houses[i].state = -1;
                            houses[i].playerName = "";
                        }
                        //广播
                        MsgPoCan msgPo = new MsgPoCan();
                        msgPo.name = player.name;
                        room.Broadcast(msgPo);
                        player.property.Clear();
                    }
                }
            }

            //自动切下一个回合
            time = 0;
            //判断胜负
            if (Judgment())
            {
                //某一方胜利，结束战斗
                room.status = Status.PREPARE;
                //发送Result
                MsgBattleDFWResult msgB = new MsgBattleDFWResult();
                //统计信息
                foreach (string id in room.playerIds.Keys)
                {
                    Player player = PlayerManager.GetPlayer(id);
                    if (!player.isPoCan)
                    {
                        msgB.winner = player.name;
                        msgB.money = player.money;
                        msgB.property = player.property.Count;
                        room.Broadcast(msgB);
                        player.data.level++;
                        player.data.win++;
                        player.data.coin += 10;

                    }
                    else
                    {
                        player.data.level++;
                        player.data.lost++;
                        player.data.coin += 5;
                    }
                }
                room.battle = null;
                BattleManager.RemoveBattle(id);
                return;
            }

            curOrder = (curOrder % room.playerIds.Count) + 1;
            Player p;
            bool b = false;
            string n = "";
            while (!b)
            {
                foreach (string id in room.playerIds.Keys)
                {
                    p = PlayerManager.GetPlayer(id);
                    if (p != null)
                    {
                        if (p.playOrder == curOrder)
                        {
                            if (!p.isPoCan && p.position != 0)
                            {
                                b = true;
                                n = p.name;
                            }
                            else if (!p.isPoCan && p.position == 0)
                            {
                                p.position = 11;
                            }
                            break;
                        }
                    }
                }
                if (!b) curOrder = (curOrder % room.playerIds.Count) + 1;
            }
            MsgSkip msg = new MsgSkip();
            msg.name = n;
            msg.curOrder = curOrder;
            room.Broadcast(msg);

        }
    }

    public bool Judgment()
    {
        if (cunHuo <= 1)
        {
            return true;
        }
        return false;
    }

    private void ResetPlayers()
    {
        foreach (string id in room.playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.money = 0;
            player.color = 0;
            player.playOrder = 0;
            player.position = 0;
            player.isSaozi = false;
            player.isPoCan = false;
            player.isGuaJi = false;
            player.property.Clear();
        }
    }

    private void SetPlayers()
    {
        int i = 0;
        int[] ints = GetRandomArray(room.playerIds.Count, 1, room.playerIds.Count);

        foreach (string id in room.playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.money = Mon;
            player.color = ints[i];
            player.playOrder = ints[i];
            player.position = 1;
            i++;
        }
    }

    public House GetHouse(int id)
    {
        if (houses.ContainsKey(id))
        {
            return houses[id];
        }
        return null;
    }

    public void TreasureAction(int num, Player player)
    {
        switch (num)
        {
            case 0:
                foreach (string id in room.playerIds.Keys)
                {
                    Player otherPlayer = PlayerManager.GetPlayer(id);
                    if (otherPlayer == null) return;
                    if (otherPlayer.name != player.name && !otherPlayer.isPoCan)
                    {
                        otherPlayer.money -= 100;
                        MsgUpdateMoney msgUpdateMoney = new MsgUpdateMoney();
                        msgUpdateMoney.name = otherPlayer.name;
                        msgUpdateMoney.money = otherPlayer.money;
                        room.Broadcast(msgUpdateMoney);
                        player.money += 100;
                    }
                }
                MsgUpdateMoney msgMoney = new MsgUpdateMoney();
                msgMoney.name = player.name;
                msgMoney.money = player.money;
                room.Broadcast(msgMoney);
                return;
            case 1:
                foreach (string id in room.playerIds.Keys)
                {
                    Player otherPlayer = PlayerManager.GetPlayer(id);
                    if (otherPlayer == null) return;
                    if (otherPlayer.name != player.name && !otherPlayer.isPoCan)
                    {
                        otherPlayer.money += 100;
                        MsgUpdateMoney msgUpdateMoney = new MsgUpdateMoney();
                        msgUpdateMoney.name = otherPlayer.name;
                        msgUpdateMoney.money = otherPlayer.money;
                        room.Broadcast(msgUpdateMoney);
                        player.money -= 100;
                    }
                }
                MsgUpdateMoney msgMoney1 = new MsgUpdateMoney();
                msgMoney1.name = player.name;
                msgMoney1.money = player.money;
                room.Broadcast(msgMoney1);
                return;
            default:
                return;
        }
    }

    public void FateAction(int num, Player player)
    {
        switch (num)
        {
            case 0:
                int index = new Random().Next(2, 41);
                int count = 0;
                House h;
                while (count < 20)
                {
                    if (houses.TryGetValue(index, out h))
                    {
                        if (h.level < h.maxLevel)
                        {
                            h.level++;
                            MsgBuild msg = new MsgBuild();
                            msg.houseId = h.id;
                            msg.houseLevel = h.level;
                            msg.result = -1;
                            room.Broadcast(msg);
                            return;
                        }
                    }
                    index = new Random().Next(2, 41);
                    count++;
                }
                if (count >= 20)
                {
                    Console.WriteLine(">20");
                    MsgUpdateMoney msgUpdateMoney = new MsgUpdateMoney();
                    msgUpdateMoney.name = player.name;
                    msgUpdateMoney.money = player.money + 200;
                    room.Broadcast(msgUpdateMoney);
                    player.money = msgUpdateMoney.money;
                }

                return;
            case 1:
                int index1 = new Random().Next(2, 41);
                int count1 = 0;
                House h1;
                while (count1 < 20)
                {
                    if (houses.TryGetValue(index1, out h1))
                    {
                        if (h1.level > 0)
                        {
                            h1.level--;
                            MsgSale msg = new MsgSale();
                            msg.houseId = h1.id;
                            msg.houseLevel = h1.level;
                            msg.result = -1;
                            room.Broadcast(msg);
                            return;
                        }
                    }
                    index1 = new Random().Next(2, 41);
                    count1++;
                }
                if (count1 >= 20)
                {
                    Console.WriteLine(">20");
                    MsgUpdateMoney msgUpdateMoney = new MsgUpdateMoney();
                    msgUpdateMoney.name = player.name;
                    msgUpdateMoney.money = player.money + 200;
                    room.Broadcast(msgUpdateMoney);
                    player.money = msgUpdateMoney.money;

                }
                return;
            default:
                return;
        }
    }
}
