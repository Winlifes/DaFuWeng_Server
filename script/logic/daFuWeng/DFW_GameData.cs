//对player类进行的封装（用于推送客户端）
//大富翁局内数据
public class GameData
{
    public string id { get; set; } = "";  //玩家id
    public string name { get; set; } = "";
    public int money { get; set; } = 0;
    public int color { get; set; } = 0;
    public int playOrder { get; set; } = 0;
    public int position { get; set; } = 0;
    public bool isSaozi { get; set; } = false;
    public bool isPoCan { get; set; } = false;
    public bool isGuaJi { get; set; } = false;
    public List<int> property { get; set; } = new List<int>();
    public GameData(string id, string name, int money, int color, int order, int position)
    {
        this.id = id;
        this.name = name;
        this.money = money;
        this.color = color;
        this.playOrder = order;
        this.position = position;
        this.isSaozi = false;
        isPoCan = false;
        isGuaJi = false;
    }
}
// 房子
public class House
{
    public int id;
    public string name;
    public int price;
    public int level;
    public int maxLevel;
    public int up;
    public string playerName = "";
    /// <summary>
    /// 租金
    /// </summary>
    public int[] rent;
    public int state;
    public House(int id, string name, int maxLevel, int price, int up, int[] rent)
    {
        this.id = id;
        this.name = name;
        this.maxLevel = maxLevel;
        this.price = price;
        this.up = up;
        this.level = 0;
        this.rent = rent;
        this.state = -1;//默认为未买入
    }
}
// 惊喜
public class Treasure
{
    public int id;
    public string name;
    public string desc;
    public Treasure(int id, string name, string desc)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
    }
}
// 命运
public class Fate
{
    public int id;
    public string name;
    public string desc;
    public Fate(int id, string name, string desc)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
    }
}