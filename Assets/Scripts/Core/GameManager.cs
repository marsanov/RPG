using System.Collections;
using System.Collections.Generic;
using RPG.Resources;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    private const string _playerID = "Character";
    public static Dictionary<string, Health> characters = new Dictionary<string, Health>();
    
    public static void RegisterCharacter(string netID, Health character)
    {
        string playerID = _playerID + netID;
        characters.Add(playerID, character);
        character.transform.name = playerID;
    }

    public static void UnregisterPlayer(string characterID)
    {
        characters.Remove(characterID);
    }

    public static Health GetPlayer(string characterID)
    {
        return characters[characterID];
    }
}
