//对player类进行的封装（用于推送客户端）
//轮盘赌局内数据
public class LPD_GameData
{
    public string id { get; set; } = "";
    public int health { get; set; } = 0;
    public string name { get; set; } = "player";
    public int playOrder { get; set; } = 0;
    public List<int> items { get; set; } = new List<int>();//id
    public LPD_GameData(string id, int health, string name, int playOrder)
    {
        this.id = id;
        this.health = health;
        this.name = name;
        this.playOrder = playOrder;
    }
}

public class IM
{
    public string id { get; set; } = "";
    public int[] items { get; set; }

    public IM(string id,int[] items)
    {
        this.id = id;
        this.items = items;
    }
}

public class Item
{
    public int id { get; set; } = -1;
    public string itemName { get; set; } = "item";
    public string description { get; set; } = "description";
    public Item(int id,string itemName,string description)
    {
        this.id = id;
        this.itemName = itemName;
        this.description = description;
    }

}