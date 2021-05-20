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


    // function that finds the fighter that the 1st player is against. 
    // used for progression development and data.
    public PlayerBase ReturnOppositePlayer(PlayerBase pl)
    {
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

    // means of generating and finding static script
    public static CharacterManager instance;
    public static CharacterManager getInstance()
    {
        return instance;
    }

    // function that aids in level progress generation
    public int ReturnCharacterInt(GameObject prefab)
    {
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


    void Awake() // generate static object across scenes
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
[System.Serializable]
// inheritable class used to aid fighter model selection
public class CharacterBase
{
    public string charId;
    public GameObject prefab;
}

[System.Serializable]
// inheritable class used to control player data
public class PlayerBase
{
    public string playerId;
    public string inputId;
    public PlayerType playerType;
    public bool hasCharacter;
    public GameObject playerPrefab;
    public StateManager playerStates;
    public int score;


    // constant int with string value use to determin the secon fighter mode
    public enum PlayerType
    {
        user, //human
        ai, //computer
    }
}