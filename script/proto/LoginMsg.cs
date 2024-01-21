//注册
public class MsgRegister:MsgBase {
	public MsgRegister() {protoName = "MsgRegister";}
	//客户端发
	public string id { get; set; } = "";
	public string pw { get; set; } = "";
	public string mail { get; set; } = "";
	//服务端回（0-成功，1-失败）
	public int result { get; set; } = 0;
}

public class MsgVersion : MsgBase
{
    public MsgVersion() { protoName = "MsgVersion"; }
    //客户端发
    public string verion { get; set; } = "";
    //服务端回（0-成功，1-失败）
    public int result { get; set; } = 0;
}


//登陆
public class MsgLogin:MsgBase {
	public MsgLogin() {protoName = "MsgLogin";}
	//客户端发
	public string id { get; set; } = "";
	public string pw { get; set; } = "";
    //服务端回（0-成功，1-失败, 2-成功但未创建角色）
    public int result { get; set; } = 0;
}

public class MsgSetName : MsgBase
{
    public MsgSetName() { protoName = "MsgSetName"; }
    //客户端发
    public string id { get; set; } = "";
    public string name { get; set; } = "";
    //服务端回（0-成功，1-失败, 2-名字已存在）
    public int result { get; set; } = 0;
}


//踢下线（服务端推送）
public class MsgKick:MsgBase {
	public MsgKick() {protoName = "MsgKick";}
	//原因（0-其他人登陆同一账号）
	public int reason { get; set; } = 0;
}