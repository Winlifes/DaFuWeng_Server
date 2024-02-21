using System.Runtime.InteropServices;

public class BattleManager
{
    //最大id
    private static int maxId = 1;
    //对局列表
    public static Dictionary<int, Battle> battles = new Dictionary<int, Battle>();

    //创建对局
    public static Battle AddBattle(Room.BattleType type)
    {
        Battle battle = null;
        maxId++;
        string className = type.ToString() + "_Battle";
        Type t = Type.GetType(className);
        
        if(t != null)
        {
            battle = t.Assembly.CreateInstance(className) as Battle;
            
            if(battle != null)
            {
                battle.id = maxId;
                battles.Add(battle.id, battle);
            }
            
        }
        
        return battle;
    }

    //删除对局
    public static bool RemoveBattle(int id)
    {
        battles.Remove(id);
        return true;
    }

    //获取对局
    public static Battle GetBattle(int id)
    {
        if (battles.ContainsKey(id))
        {
            return battles[id];
        }
        return null;
    }

    //Update
    public static void Update()
    {
        foreach (Battle battle in battles.Values)
        {
            battle.Update();
        }
    }
}
