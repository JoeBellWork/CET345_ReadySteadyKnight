using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    WaitForSeconds oneSec;
    public Transform[] spawnPosition;
    CameraManager camM;
    CharacterManager charM;
    LevelUI levelUi;
    public int maxTurns = 2;
    int currentTurn = 1;

    public bool countdown;
    public int maxTurnTimer = 60;
    int currentTimer;
    float internalTimer;

    public static LevelManager instance;
    public static LevelManager getInstance()
    {
        return instance;
    }


    //initialise the scripts, static controls and texts
    void Start()
    {
        charM = CharacterManager.getInstance();
        camM = CameraManager.GetInstance();
        levelUi = LevelUI.GetInstance();
        oneSec = new WaitForSeconds(1);
        levelUi.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUi.AnnouncerTextLine2.gameObject.SetActive(false);
        StartCoroutine("StartGame");
    }


    // set player orientation to control direction
    public void FixedUpdate()
    {
        // player orientation
        if (charM.players[0].playerStates.transform.position.x <
            charM.players[1].playerStates.transform.position.x)
        {
            charM.players[0].playerStates.lookRight = true;
            charM.players[1].playerStates.lookRight = false;
        }
        else
        {
            charM.players[0].playerStates.lookRight = false;
            charM.players[1].playerStates.lookRight = true;
        }
    }

    //update timer in update
    public void Update()
    {
        if (countdown)
        {
            HandleTurnTimer();
        }
    }


    // void that controls the turn timer so that the fight counts down
    void HandleTurnTimer()
    {
        levelUi.LevelTimer.text = currentTimer.ToString();
        internalTimer += Time.deltaTime;

        if (internalTimer > 1)
        {
            currentTimer--;
            internalTimer = 0;
        }

        if (currentTimer <= 0)
        {
            EndTurnFunction(true);
            countdown = false;
        }
    }


    // initialse IEnumerator
    IEnumerator StartGame()
    {
        yield return CreatePlayers();
        yield return InitTurn();
    }



    // create the numer of players set
    IEnumerator CreatePlayers()
    {
        for (int i = 0; i < charM.players.Count; i++)
        {
            GameObject go = Instantiate(charM.players[i].playerPrefab, spawnPosition[i].position, Quaternion.identity) as GameObject;
            charM.players[i].playerStates = go.GetComponent<StateManager>();
            charM.players[i].playerStates.healthSlider = levelUi.healthSliders[i];
            camM.players.Add(go.transform);
        }
        yield return null;
    }


    // initialise turns
    IEnumerator InitTurn()
    {
        levelUi.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUi.AnnouncerTextLine2.gameObject.SetActive(false);
        currentTimer = maxTurnTimer;
        countdown = false;
        yield return InitPlayers();
        yield return EnableControl();
    }


    // initialise players
    IEnumerator InitPlayers()
    {
        for (int i = 0; i < charM.players.Count; i++)
        {
            charM.players[i].playerStates.health = 100;
            charM.players[i].playerStates.handleAnim.anim.Play("Locomotion");
            charM.players[i].playerStates.transform.position = spawnPosition[i].position;
        }
        yield return null;
    }


    // enable player input 
    IEnumerator EnableControl()
    {
        levelUi.AnnouncerTextLine1.gameObject.SetActive(true);
        levelUi.AnnouncerTextLine1.text = "Turn" + currentTurn;
        levelUi.AnnouncerTextLine1.color = Color.white;
        yield return oneSec;
        yield return oneSec;

        levelUi.AnnouncerTextLine1.text = "3";
        levelUi.AnnouncerTextLine1.color = Color.green;
        yield return oneSec;

        levelUi.AnnouncerTextLine1.text = "2";
        levelUi.AnnouncerTextLine1.color = Color.yellow;
        yield return oneSec;

        levelUi.AnnouncerTextLine1.text = "1";
        levelUi.AnnouncerTextLine1.color = Color.red;
        yield return oneSec;

        levelUi.AnnouncerTextLine1.text = "FIGHT";
        levelUi.AnnouncerTextLine1.color = Color.red;
        yield return oneSec;


        for (int i = 0; i < charM.players.Count; i++)
        {
            if (charM.players[i].playerType == PlayerBase.PlayerType.user)
            {
                InputHandler ih = charM.players[i].playerStates.gameObject.GetComponent<InputHandler>();
                ih.playerInput = charM.players[i].inputId;
                ih.enabled = true;
            }

            if (charM.players[i].playerType == PlayerBase.PlayerType.ai)
            {
                AICharacter ai = charM.players[i].playerStates.gameObject.GetComponent<AICharacter>();
                ai.enabled = true;

                ai.enStates = charM.ReturnOppositePlayer(charM.players[i]).playerStates;
            }
        }

        yield return oneSec;
        levelUi.AnnouncerTextLine1.gameObject.SetActive(false);
        countdown = true;
    }


    // Disable user input after fight
    void DisableControl()
    {
        for (int i = 0; i < charM.players.Count; i++)
        {
            charM.players[i].playerStates.ResetStateInputs();

            if (charM.players[i].playerType == PlayerBase.PlayerType.user)
            {
                charM.players[i].playerStates.GetComponent<InputHandler>().enabled = false;
            }
        }
    }


    //used to end a fight and run IEnumerator below for similar functionality
    public void EndTurnFunction(bool timeOut = false)
    {
        countdown = false;
        levelUi.LevelTimer.text = maxTurnTimer.ToString();

        if (timeOut)
        {
            levelUi.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUi.AnnouncerTextLine1.text = "Out of Time!";
            levelUi.AnnouncerTextLine1.color = Color.cyan;
        }
        else
        {
            levelUi.AnnouncerTextLine1.gameObject.SetActive(true);
            levelUi.AnnouncerTextLine1.text = "K.O.";
            levelUi.AnnouncerTextLine1.color = Color.red;
        }

        DisableControl();
        StartCoroutine(EndTurn());
    }


    // Large Ienumerator that ends a fight once the tierm has ran out or if the fighter has lost health. used to reset fighters and states or move to other scnes
    IEnumerator EndTurn()
    {
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        PlayerBase vPlayer = FindWinningPlayer();

        if (vPlayer == null)
        {
            levelUi.AnnouncerTextLine1.text = "Draw";
            levelUi.AnnouncerTextLine1.color = Color.blue;
        }
        else
        {
            levelUi.AnnouncerTextLine1.text = vPlayer.playerId + " Wins!";
            levelUi.AnnouncerTextLine1.color = Color.red;
        }
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        if (vPlayer != null)
        {
            if (vPlayer.playerStates.health == 100)
            {
                levelUi.AnnouncerTextLine2.gameObject.SetActive(true);
                levelUi.AnnouncerTextLine2.text = "Flawless Victory!";
            }
        }
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        currentTurn++;
        bool matchOver = isMatchOver();
        if (!matchOver)
        {
            StartCoroutine("InitTurn");
        }
        else
        {
            for (int i = 0; i < charM.players.Count; i++)
            {
                charM.players[i].score = 0;
                charM.players[i].hasCharacter = false;
            }

            if (charM.solo)
            {
                if (vPlayer == charM.players[0])
                {
                    MySceneManager.GetInstance().LoadNextOnProgression();
                }
                else
                {
                    MySceneManager.GetInstance().RequestLevelOnLoad(MySceneManager.SceneType.main, "gameOver");
                }
            }
            else
            {
                MySceneManager.GetInstance().RequestLevelOnLoad(MySceneManager.SceneType.main, "select");
            }
        }
    }


    // checks to see if match is over IE if wins exides 1
    bool isMatchOver()
    {
        bool retVal = false;

        for (int i = 0; i < charM.players.Count; i++)
        {
            if (charM.players[i].score >= maxTurns)
            {
                retVal = true;
                break;
            }
        }
        return retVal;
    }


    // function that determins which player won the match
    PlayerBase FindWinningPlayer()
    {
        PlayerBase retVal = null;
        StateManager targetPlayer = null;

        if (charM.players[0].playerStates.health != charM.players[1].playerStates.health)
        {
            if (charM.players[0].playerStates.health < charM.players[1].playerStates.health)
            {
                charM.players[1].score++;
                targetPlayer = charM.players[1].playerStates;
                levelUi.AddWinIndicator(1);
            }
            else
            {
                charM.players[0].score++;
                targetPlayer = charM.players[0].playerStates;
                levelUi.AddWinIndicator(0);
            }
            retVal = charM.returnPlayerFromStates(targetPlayer);
        }
        return retVal;
    }

    void Awake()
    {
        instance = this;
    }
}
