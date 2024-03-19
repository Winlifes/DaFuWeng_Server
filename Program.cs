namespace DaFuWeng_Server
{
    internal class Program
    {
        public static string version = "1.2.0";
        public static void Main(string[] args)
        {
            if (!DbManager.Connect("test", "127.0.0.1", 3306, "root", "123456"))
            {
                return;
            }

            NetManager.StartLoop(9888);
        }
    }
}
