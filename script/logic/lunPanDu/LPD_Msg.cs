//进入战场（服务端推送）
public class MsgEnterLPDBattle : MsgBase
{
    public MsgEnterLPDBattle() { protoName = "MsgEnterLPDBattle"; }
    //服务端回
    public LPD_GameData[] gameDatas { get; set; }
    public int result { get; set; } = 0;
}

public class MsgShot : MsgBase
{
    public MsgShot() { protoName = "MsgShot"; }

    public string id { get; set; } = "";
    public string beHitName { get; set; } = "";
    public int health { get; set; } = 0;
    public bool isHIt { get; set; } = false;
    public int curOrder { get; set; } = 0;
    public int result { get; set; } = 0;
}

public class MsgNextRound:MsgBase
{
    public MsgNextRound() { protoName = "MsgNextRound"; }

    public int round { get; set; } = 0;
    public int bullet { get; set; } = 0;
    public int fakeBullet { get; set; } = 0;
    public int realBullet { get; set; } = 0;
    public int result { get; set; } = 0;
}

public class MsgItem : MsgBase
{
    public MsgItem() { protoName = "MsgItem"; }
    public string id { get; set; } = "";
    public int item { get; set; } = 0;
    public bool use { get; set; } = false;
    public int result { get; set; } = 0;

}

public class MsgAddHealth : MsgBase
{
    public MsgAddHealth() { protoName = "MsgAddHealth"; }
    public string id { get; set; } = "";
    public int health { get; set; } = 0;
    public int result { get; set; } = 0;

}

public class MsgViewBullet : MsgBase
{
    public MsgViewBullet() { protoName = "MsgViewBullet"; }
    public int bullet { get; set; } = -1;
    public int result { get; set; } = 0;

}

public class MsgAddState : MsgBase
{
    public MsgAddState() { protoName = "MsgAddState"; }

    public string id { get; set; } = "";
    public LPD_State[] states { get; set; }
    public int result { get; set; } = 0;

}

public enum LPD_State
{
    NONE,
    LOCKED,
    DEFENSE,
    UP
}
//战斗结果（服务端推送）
public class MsgBattleLPDResult : MsgBase
{
    public MsgBattleLPDResult() { protoName = "MsgBattleLPDResult"; }
    //服务端回
    public string winner { get; set; } = "";	 //获胜的阵营
}
