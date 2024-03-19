using Org.BouncyCastle.Pqc.Crypto.Picnic;
using static Room;

public class LPD_Battle : Battle
{
    private static int health = 4;

    public int bullet = 0;

    public int bulletAttack = 1;

    public int[] bullets = new int[20];

    public List<Item> items = new List<Item>();
    public override void Start(Room r, int c)
    {
        base.Start(r, c);
        count = 0;
        ResetPlayers();
        SetPlayers();
        ItemInit();
    }

    public override void Update()
    {
        if(cunHuo <= 1)
        {
            End();
        }
        if(bullet <= 0)
        {
            NextRound();
        }
    }

    public override void Remove(Player player)
    {
        base.Remove(player);

    }

    private void ResetPlayers()
    {
        foreach (string id in room.playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.playOrder = 0;
            player.health = 0;
            player.items.Clear();
            player.lPD_States.Clear();
        }
    }

    private void SetPlayers()
    {
        int i = 0;
        int[] ints = GetRandomArray(room.playerIds.Count, 1, room.playerIds.Count);

        foreach (string id in room.playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.playOrder = ints[i];
            player.health = health; 
            i++;
        }
    }

    private void ItemInit()
    {
        items.Clear();
        items.Add(new Item(0, "咖啡","加一条命"));
        items.Add(new Item(1, "啤酒", "退出一颗子弹并强制射击"));
        items.Add(new Item(2, "手铐", "锁住对方一回合"));
        items.Add(new Item(3, "放大镜", "查看即将要打出的子弹"));
        items.Add(new Item(4, "锯子", "强化下一次射击"));
        items.Add(new Item(5, "防弹衣", "下一个对手回合抵挡一颗子弹"));
    }

    public bool ItemAction(int item,Player player)
    {
        switch(item)
        {
            case 0:
                player.health++;
                MsgAddHealth msg = new MsgAddHealth();
                msg.id = player.id;
                msg.health = player.health;
                room.Broadcast(msg);
                break;
            case 1:
                MsgViewBullet msg1 = new MsgViewBullet();
                msg1.bullet = bullets[bullet - 1];
                room.Broadcast(msg1);
                bullet--;
                break;
            case 2:
                foreach(string id in room.playerIds.Keys)
                {
                    if(id != player.id)
                    {
                        Player p = PlayerManager.GetPlayer(id);
                        if (p != null)
                        {
                            if (p.lPD_States.Contains(LPD_State.LOCKED))
                            {
                                return false;
                            }
                            p.lPD_States.Add(LPD_State.LOCKED);
                            MsgAddState msg2 = new MsgAddState();
                            msg2.id = p.id;
                            msg2.states = p.lPD_States.ToArray();
                            room.Broadcast(msg2);
                        }
                    }
                }
                
                break;
            case 3:
                MsgViewBullet msg3 = new MsgViewBullet();
                msg3.bullet = bullets[bullet - 1];
                room.Broadcast(msg3);
                break;
            case 4:
                if (player.lPD_States.Contains(LPD_State.UP))
                {
                    return false;
                }
                bulletAttack++;
                player.lPD_States.Add(LPD_State.UP);
                MsgAddState msg4 = new MsgAddState();
                msg4.id = player.id;
                msg4.states = player.lPD_States.ToArray();
                room.Broadcast(msg4);
                break;
            case 5:
                if (player.lPD_States.Contains(LPD_State.DEFENSE))
                {
                    return false;
                }
                player.lPD_States.Add(LPD_State.DEFENSE);
                MsgAddState msg5 = new MsgAddState();
                msg5.id = player.id;
                msg5.states = player.lPD_States.ToArray();
                room.Broadcast(msg5);
                break;
            default: break;
        }
        return true;
    }

    private void NextRound()
    {
        count++;
        bullet = 3 + count;
        int fakeBullet = 0,realBullet = 0;
        while(fakeBullet==0 || realBullet==0)
        {
            fakeBullet = 0;
            realBullet = 0;
            for (int i = 0; i < bullet; i++)
            {
                bullets[i] = new Random().Next(2);
                if (bullets[i] == 0)
                {
                    realBullet++;
                }
                else
                {
                    fakeBullet++;
                }
            }
        }
        
        MsgNextRound msg = new MsgNextRound();
        foreach (string id in room.playerIds.Keys)
        {
            Player p = PlayerManager.GetPlayer(id);
            if (p != null)
            {
                int a = new Random().Next(items.Count);
                p.items.Add(a);
                MsgItem m1 = new MsgItem();
                m1.id = id;
                m1.use = false;
                m1.item = a;
                room.Broadcast(m1);

                int b = new Random().Next(items.Count);
                p.items.Add(b);
                MsgItem m2 = new MsgItem();
                m2.id = id;
                m2.use = false;
                m2.item = b;
                room.Broadcast(m2);
            }
        }
        msg.round = count;
        msg.bullet = bullet;
        msg.fakeBullet = fakeBullet;
        msg.realBullet = realBullet;
        room.Broadcast(msg);
    }
    private void End()
    {
        //某一方胜利，结束战斗
        room.status = Status.PREPARE;
        //发送Result
        MsgBattleLPDResult msgB = new MsgBattleLPDResult();
        //统计信息
        foreach (string id in room.playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.health > 0)
            {
                msgB.winner = player.name;
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
    }
}
