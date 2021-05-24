using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScreenManager : MonoBehaviour
{
    public int numberOfPlayers = 1;
    public List<PlayerInterfaces> plInterfaces = new List<PlayerInterfaces>();
    public PotraitInfo[] potraitPrefabs; // entires as portraits
    public int maxX; // hardcoded X and Y limits for portraits
    public int maxY;
    PotraitInfo[,] charGrid; // grid for selection entires

    public GameObject potraitCanvas; // canvas that holds portraits

    bool loadLevel; // if level loading
    public bool bothPlayersSelected;
    CharacterManager charManager;
    public AudioManagerScript audioManager;
    private int i;

    // same instance getting scripts
    public static SelectScreenManager instance;
    public static SelectScreenManager getInstance()
    {
        return instance;
    }
    void Awake()
    {
        instance = this;
    }
    

    void Start()
    {
        // reference to character manager
        charManager = CharacterManager.getInstance();
        audioManager = AudioManagerScript.getInstance();
        numberOfPlayers = charManager.numberOfUsers;

        charManager.solo = (numberOfPlayers == 1);

        //create grid
        charGrid = new PotraitInfo[maxX, maxY];
        int x = 0;
        int y = 0;

        potraitPrefabs = potraitCanvas.GetComponentsInChildren<PotraitInfo>();

        // enter all portraits and assign grid
        for (int i = 0; i < potraitPrefabs.Length; i++)
        {
            potraitPrefabs[i].posX += x;
            potraitPrefabs[i].posY += y;
            charGrid[x, y] = potraitPrefabs[i];

            if(x < maxX - 1)
            {
                x++;
            }
            else
            {
                x = 0;
                y++;
            }
        }
    }

    void Update()
    {
        if(!loadLevel)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // leave game with escape button
            {
                audioManager.soundPlay("Effect_GameOver");
                StartCoroutine(QuitGame());
            }

            // allow users to select their fighter using a grouping system
            for (int i = 0; i < plInterfaces.Count; i++)
            {
                if(i < numberOfPlayers)
                {
                    if (Input.GetButtonUp("Fire2" + charManager.players[i].inputId))
                    {
                        audioManager.soundPlay("Effect_Jump");
                        plInterfaces[i].playerBase.hasCharacter = false;
                    }

                    if (!charManager.players[i].hasCharacter)
                    {
                        plInterfaces[i].playerBase = charManager.players[i];

                        HandleSelectorPosition(plInterfaces[i]);
                        HandleSelectScreenInput(plInterfaces[i], charManager.players[i].inputId);
                        HandleCharacterPreview(plInterfaces[i]);
                    }
                }
                else
                {
                    charManager.players[i].hasCharacter = true;
                }
            }
        }
        if(bothPlayersSelected)
        {
            StartCoroutine(LoadLevel());
            loadLevel = true;
        }
        else
        {
            if(charManager.players[0].hasCharacter && charManager.players[1].hasCharacter)
            {
                bothPlayersSelected = true;
            }
        }
    }

    void HandleSelectScreenInput(PlayerInterfaces pl, string playerId)
    {
        #region Grid Navigation
        /* To navigate grid, change active X and Y to select which entry is active
         * smooth input to reduce speed of selection if button held down          
         */

        float verticle = Input.GetAxis("Vertical" + playerId);
        if(verticle != 0)
        {
            if(!pl.hasInputOnce)
            {
                if(verticle > 0)
                {
                    pl.activeY = (pl.activeY > 0) ? pl.activeY - 1 : maxY - 1;
                }
                else
                {
                    pl.activeY = (pl.activeY < maxY - 1) ? pl.activeY + 1 : 0;
                }
                pl.hasInputOnce = true;
            }
        }

        float horizontal = Input.GetAxis("Horizontal" + playerId);
        if(horizontal != 0)
        {
            if(!pl.hasInputOnce)
            {
                if(horizontal > 0)
                {
                    pl.activeX = (pl.activeX > 0) ? pl.activeX - 1 : maxX - 1;                }
                else
                {
                    pl.activeX = (pl.activeX < maxX - 1) ? pl.activeX + 1 : 0;
                }
                pl.timerToReset = 0;
                pl.hasInputOnce = true;
            }
        }

        if(verticle == 0 && horizontal == 0)
        {
            pl.hasInputOnce = false;
        }

        if(pl.hasInputOnce)
        {
            pl.timerToReset += Time.deltaTime;
            if(pl.timerToReset > 0.8f)
            {
                pl.hasInputOnce = false;
                pl.timerToReset = 0;
            }
        }
        #endregion
        // space bar to select character
        if(Input.GetButtonUp("Fire1" + playerId))
        {
            audioManager.soundPlay("Effect_Hit");
            // characterReact
            pl.createdCharacter.GetComponentInChildren<Animator>().Play("Kick");
            // pass character to character manager
            pl.playerBase.playerPrefab = charManager.returnCharacterWithId(pl.activePotrait.characterId).prefab;
            pl.playerBase.hasCharacter = true;
        }
    }


    IEnumerator LoadLevel()
    {
        audioManager.soundPlay("Effect_Win");
        // AI get random character prefab
        for(int i = 0; i < charManager.players.Count; i++)
        {
            if(charManager.players[i].playerType == PlayerBase.PlayerType.ai)
            {
                if(charManager.players[i].playerPrefab == null)
                {
                    int ranValue = Random.Range(0, potraitPrefabs.Length);
                    charManager.players[i].playerPrefab =
                        charManager.returnCharacterWithId(potraitPrefabs[ranValue].characterId).prefab;
                }
            }
        }
        yield return new WaitForSeconds(2);


        // system to randomise between level 1 and 2 for fight level.
        i = Random.Range(0, 2);
        if(i == 1)
        {
            MySceneManager.GetInstance().RequestLevelOnLoad(MySceneManager.SceneType.prog, "level_1");
        }
        else
        {
            MySceneManager.GetInstance().RequestLevelOnLoad(MySceneManager.SceneType.prog, "level_2");
        }
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    void HandleSelectorPosition(PlayerInterfaces pl)
    {
        // activate character selector
        pl.selector.SetActive(true);
        //find active portrait
        pl.activePotrait = charGrid[pl.activeX, pl.activeY];
        // place selector over position
        Vector2 selectorPosition = pl.activePotrait.transform.localPosition;
        selectorPosition = selectorPosition + new Vector2(potraitCanvas.transform.transform.localPosition.x, potraitCanvas.transform.localPosition.y);
        pl.selector.transform.localPosition = selectorPosition;
    }


    // function for setting which fighter object is instanciated into scene to display selected fighter
    void HandleCharacterPreview(PlayerInterfaces pl)
    {
        if(pl.previewPotrait != pl.activePotrait)
        {
            if(pl.createdCharacter != null)
            {
                Destroy(pl.createdCharacter);
            }

            audioManager.soundPlay("Effect_Click");
            GameObject go = Instantiate(
                CharacterManager.getInstance().returnCharacterWithId(pl.activePotrait.characterId).prefab, pl.charVisPos.position, Quaternion.identity) as GameObject;

            pl.createdCharacter = go;
            pl.previewPotrait = pl.activePotrait;

            if(!string.Equals(pl.playerBase.playerId, charManager.players[0].playerId))
            {
                pl.createdCharacter.GetComponent<StateManager>().lookRight = false;
            }

        }
    }

}

[System.Serializable]
public class PlayerInterfaces
{
    public PotraitInfo activePotrait; // current active portrait for P1
    public PotraitInfo previewPotrait;
    public GameObject selector; // select indicator for P1
    public Transform charVisPos; // character visual position for p1
    public GameObject createdCharacter; // created character for P1

    public int activeX; // active X and Y for P1
    public int activeY;

    // smoothing output
    public bool hasInputOnce;
    public float timerToReset;
    public PlayerBase playerBase;
}

