
using System.Text.Json;
using MySql.Data.MySqlClient;

public class DbManager {
	public static MySqlConnection mysql;

	//连接mysql数据库
	public static bool Connect(string db, string ip, int port, string user, string pw)
	{
		//创建MySqlConnection对象
		mysql = new MySqlConnection();
		//连接参数
		string s = string.Format("Database={0};Data Source={1}; port={2};User Id={3}; Password={4}", 
			               db, ip, port, user, pw);
		mysql.ConnectionString = s;
		//连接
		try
		{
			mysql.Open();
			Console.WriteLine("[数据库]connect succ ");
			mysql.Close();
			return true;
		}
		catch (Exception e)
		{
			Console.WriteLine("[数据库]connect fail, " + e.Message);
			return false;
		}
	}

	//测试并重连
	private static void CheckAndReconnect(){
		try{
			if(mysql.State == System.Data.ConnectionState.Open)
			{
				mysql.Close();
			}
			
			if(mysql.Ping()){
				return;
			}
			mysql.Close();
			mysql.Open();
			//Console.WriteLine("[数据库] Reconnect!");
		}
		catch(Exception e){
			Console.WriteLine("[数据库] CheckAndReconnect fail " + e.Message);
		}
		
	}

	//判定安全字符串
	private static bool IsSafeString(string str)
	{
		return !(str.Contains('/') || str.Contains('\\') || str.Contains(';') || str.Contains(',') || str.Contains('(') || str.Contains(')') || str.Contains('{') || str.Contains('}'));
	}


	//是否存在该用户
	public static bool IsAccountExist(string id)
	{
		CheckAndReconnect();
		//防sql注入
		if (!DbManager.IsSafeString(id)){
			return false;
		}
		//sql语句
		string s = string.Format("select * from account where id='{0}';", id);  
		//查询
		try 
		{
			MySqlCommand cmd = new MySqlCommand (s, mysql); 
			MySqlDataReader dataReader = cmd.ExecuteReader (); 
			bool hasRows = dataReader.HasRows;
			dataReader.Close();
			return hasRows;
		}
		catch(Exception e)
		{
			Console.WriteLine("[数据库] IsSafeString err, " + e.Message);
			return false;
		}
	}

    //是否存在该名字
    public static bool IsNameExist(string name)
    {
        CheckAndReconnect();
        //防sql注入
        if (!DbManager.IsSafeString(name))
        {
            return false;
        }
        //sql语句
        string s = string.Format("select * from dfw_player where name='{0}';", name);
        //查询
        try
        {
            MySqlCommand cmd = new MySqlCommand(s, mysql);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            bool hasRows = dataReader.HasRows;
            dataReader.Close();
            return hasRows;
        }
        catch (Exception e)
        {
            Console.WriteLine("[数据库] IsSafeString err, " + e.Message);
            return false;
        }
    }

    //注册
    public static bool Register(string id, string pw, string mail)
	{
		CheckAndReconnect();
		//防sql注入
		if(!DbManager.IsSafeString(id)){
			Console.WriteLine("[数据库] Register fail, id not safe");
			return false;
		}
		if(!DbManager.IsSafeString(pw)){
			Console.WriteLine("[数据库] Register fail, pw not safe");
			return false;
		}
		//能否注册
		if (IsAccountExist(id)) 
		{
			Console.WriteLine("[数据库] Register fail, id exist");
			return false;
		}
		//写入数据库User表
		string sql = string.Format("insert into account set id ='{0}' ,pw ='{1}' ,mail ='{2}';", id, pw, mail);
		try
		{
			MySqlCommand cmd = new MySqlCommand(sql, mysql);
			cmd.ExecuteNonQuery();
			return true;
		}
		catch(Exception e)
		{
			Console.WriteLine("[数据库] Register fail " + e.Message);
			return false;
		}
	}

    //改名字
    public static bool SetName(string id, string name)
    {
        CheckAndReconnect();
        //防sql注入
        if (!DbManager.IsSafeString(name))
        {
            Console.WriteLine("[数据库] SetName fail, name not safe");
            return false;
        }
        //能否注册
        if (IsNameExist(name))
        {
            Console.WriteLine("[数据库] SetName fail, name exist");
            return false;
        }
        //写入数据库dfw_player表
        string sql = string.Format("insert into dfw_player set id ='{0}' ,name ='{1}';", id, name);
        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("[数据库] Register fail " + e.Message);
            return false;
        }
    }


    //创建角色
    public static bool CreatePlayer(string id)
	{
		CheckAndReconnect();
		//防sql注入
		if(!DbManager.IsSafeString(id)){
			Console.WriteLine("[数据库] CreatePlayer fail, id not safe");
			return false;
		}
		//序列化
		PlayerData playerData = new PlayerData ();
		string data = JsonSerializer.Serialize(playerData); 
		//写入数据库
		string sql = string.Format ("update dfw_player set data ='{0}' where id = '{1}';", data, id);
		try 
		{
			MySqlCommand cmd = new MySqlCommand (sql, mysql);
			cmd.ExecuteNonQuery ();
			return true;
		} 
		catch (Exception e)
		{
			Console.WriteLine("[数据库] CreatePlayer err, " + e.Message);
			return false;
		}
	}


	//检测用户名密码
	public static bool CheckPassword(string id, string pw)
	{
		CheckAndReconnect();
		//防sql注入
		if(!DbManager.IsSafeString(id)){
			Console.WriteLine("[数据库] CheckPassword fail, id not safe");
			return false;
		}
		if(!DbManager.IsSafeString(pw)){
			Console.WriteLine("[数据库] CheckPassword fail, pw not safe");
			return false;
		}
		//查询
		string sql = string.Format("select * from account where id='{0}' and pw='{1}';", id, pw);  

		try 
		{
			MySqlCommand cmd = new MySqlCommand (sql, mysql);  
			MySqlDataReader dataReader = cmd.ExecuteReader();
			bool hasRows = dataReader.HasRows;
			dataReader.Close();
			return hasRows;
		}
		catch(Exception e)
		{
			Console.WriteLine("[数据库] CheckPassword err, " + e.Message);
			return false;
		}
	}


	//获取玩家数据
	public static PlayerData GetPlayerData(string id)
	{
		CheckAndReconnect();
		//防sql注入
		if(!DbManager.IsSafeString(id)){
			Console.WriteLine("[数据库] GetPlayerData fail, id not safe");
			return null;
		}

		//sql
		string sql = string.Format("select * from dfw_player where id ='{0}';", id);
		try
		{
			//查询
			MySqlCommand cmd = new MySqlCommand (sql, mysql); 
			MySqlDataReader dataReader = cmd.ExecuteReader(); 
			if(!dataReader.HasRows)
			{
				dataReader.Close();
				return null;
			}
			//读取
			dataReader.Read();
			string data = dataReader.GetString("data");
			//反序列化
			PlayerData playerData = JsonSerializer.Deserialize<PlayerData>(data);
			dataReader.Close();
			return playerData;
		}
		catch(Exception e)
		{
			Console.WriteLine("[数据库] GetPlayerData fail, " + e.Message);
			return null;
		}
	}

    public static string GetPlayerName(string id)
    {
        CheckAndReconnect();
        //防sql注入
        if (!DbManager.IsSafeString(id))
        {
            Console.WriteLine("[数据库] GetPlayerData fail, id not safe");
            return null;
        }

        //sql
        string sql = string.Format("select * from dfw_player where id ='{0}';", id);
        try
        {
            //查询
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            if (!dataReader.HasRows)
            {
                dataReader.Close();
                return null;
            }
            //读取
            dataReader.Read();
            string name = dataReader.GetString("name");
			return name;
        }
        catch (Exception e)
        {
            Console.WriteLine("[数据库] GetPlayerData fail, " + e.Message);
            return null;
        }
    }

    //保存角色
    public static bool UpdatePlayerData(string id, PlayerData playerData)
	{
		CheckAndReconnect();
		//序列化
		string data = JsonSerializer.Serialize(playerData); 
		//sql
		string sql = string.Format("update dfw_player set data='{0}' where id ='{1}';", data, id);
		//更新
		try 
		{
			MySqlCommand cmd = new MySqlCommand (sql, mysql);
			cmd.ExecuteNonQuery ();
			return true;
		} 
		catch (Exception e)
		{
			Console.WriteLine("[数据库] UpdatePlayerData err, " + e.Message);
			return false;
		}
	}
}


