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
    public float time;

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

    public static int[] GetRandomArray(int Number, int minNum, int maxNum)
    {
        int j;
        int[] b = new int[Number];
        Random r = new Random();
        for (j = 0; j < Number; j++)
        {
            int i = r.Next(minNum, maxNum + 1);
            int num = 0;
            for (int k = 0; k < j; k++)
            {
                if (b[k] == i)
                {
                    num++;
                }
            }
            if (num == 0)
            {
                b[j] = i;
            }
            else
            {
                j--;
            }
        }
        return b;
    }
}
