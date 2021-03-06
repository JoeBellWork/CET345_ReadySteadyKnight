using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public bool solo;
    public int numberOfUsers;

    // a list with all players and player types
    public List<PlayerBase> players = new List<PlayerBase>();


    // list for character data, id and prefab
    public List<CharacterBase> characterList = new List<CharacterBase>();





    // find characters with ID
    public CharacterBase returnCharacterWithId(string id)
    {
        CharacterBase retVal = null;

        for (int i = 0; i < characterList.Count; i++)
        {
            if (string.Equals(characterList[i].charId, id))
            {
                retVal = characterList[i];
                break;
            }
        }

        return retVal;
    }

    // return player from character states
    public PlayerBase returnPlayerFromStates(StateManager states)
    {
        PlayerBase retVal = null;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerStates == states)
            {
                retVal = players[i];
                break;
            }
        }
        return retVal;
    }

    public PlayerBase ReturnOppositePlayer(PlayerBase pl)
    {
        // find opposite fighter
        PlayerBase retVal = null;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != pl)
            {
                retVal = players[i];
                break;
            }
        }
        return retVal;
    }

    public int ReturnCharacterInt(GameObject prefab)
    {
        // function that returns character int for prefab in list
        int retVal = 0;
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].prefab == prefab)
            {
                retVal = i;
                break;
            }
        }
        return retVal;
    }

    // set static
    public static CharacterManager instance;
    public static CharacterManager getInstance()
    {
        return instance;
    }
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
[System.Serializable]
public class CharacterBase
{
    // fighter details
    public string charId;
    public GameObject prefab;
}

[System.Serializable]
public class PlayerBase
{
    // base stats and varibles for assigning a fighter toa  character
    public string playerId;
    public string inputId;
    public PlayerType playerType;
    public bool hasCharacter;
    public GameObject playerPrefab;
    public StateManager playerStates;
    public int score;

    public enum PlayerType // enum varible to assign player type for input and behaviour
    {
        user, //human
        ai, //computer
    }
}

