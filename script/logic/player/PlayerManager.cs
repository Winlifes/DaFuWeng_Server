﻿using System;

public class PlayerManager
{
	//玩家列表
	static Dictionary<string, Player> players = new Dictionary<string, Player>();
	//玩家是否在线
	public static bool IsOnline(string id){
		return players.ContainsKey(id);
	}
	//获取玩家
	public static Player GetPlayer(string id){
		if(players.ContainsKey(id)){
			return players[id];
		}
		return null;
	}

    public static Player GetPlayer(int order)
    {
        foreach(Player p in players.Values)
		{
			if(p.playOrder == order) return p;
		}
        return null;
    }

    //添加玩家
    public static void AddPlayer(string id, Player player){
		players.Add(id, player);
	}
	//删除玩家
	public static void RemovePlayer(string id){
		players.Remove(id);
	}
}


