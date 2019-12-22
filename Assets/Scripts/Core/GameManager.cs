using System.Collections;
using System.Collections.Generic;
using RPG.Resources;
using RPG.Stats;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string _playerID = "Player";
    private static Dictionary<string, Health> players = new Dictionary<string, Health>();

    public static void RegisterPlayer(string netID, Health player)
    {
        string playerID = _playerID + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
    }

    public static void UnregisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Health GetPlayer(string playerID)
    {
        return players[playerID];
    }
}
