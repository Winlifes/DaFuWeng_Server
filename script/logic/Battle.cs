public class Battle
{
    //对局id
    public int id;
    //当前对局所属的房间
    public Room room;
    //回合数
    public int count;
    //当前回合
    public int curOrder;
    //存活人数
    public int cunHuo;
    //聊天内容
    public string content;
    // 计时器
    public int time;

    public virtual void Start(Room r, int c)
    {
        count = 1;
        curOrder = 1;
        content = "";
        time = 0;
        room = r;
        cunHuo = c;
    }

    public virtual void Update()
    {

    }

    public virtual void Remove(Player player)
    {
        cunHuo--;
        
    }
}
