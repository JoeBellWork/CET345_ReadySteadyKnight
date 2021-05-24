using System.Collections;
using UnityEngine;

public class IntroSceneManager : MonoBehaviour
{
    public GameObject startText;
    float timer;
    bool loadingLevel;
    bool initialise;

    public int activeElement;
    public GameObject menuObj;
    public ButtonRef[] menuOptions;
    public AudioManagerScript audioManager;

    private void Start()
    {
        menuObj.SetActive(false);
    }

    public void Update()
    {
        if(audioManager == null)
        {
            audioManager = AudioManagerScript.getInstance();
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            audioManager.soundPlay("Effect_GameOver");
            StartCoroutine(QuitGame());
        }


        if (!initialise)
        {
            //flicker start text for asthetic and feedback
            timer += Time.deltaTime;
            if(timer > 0.6f)
            {
                timer = 0;
                startText.SetActive(!startText.activeInHierarchy);
            }

            //press space to start functionality. Closes initial text and offers player 1/2 options.
            if(Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Jump"))
            {
                audioManager.soundPlay("Effect_Win");
                initialise = true;
                startText.SetActive(false);
                menuObj.SetActive(true);
            }
        }
        else
        {
            if(!loadingLevel) // while game isnt loading a new level
            {
                //shows which option is selected
                menuOptions[activeElement].selected = true;

                // change which menu option is selected
                if(Input.GetKeyUp(KeyCode.UpArrow))
                {
                    audioManager.soundPlay("Effect_Click");
                    menuOptions[activeElement].selected = false;
                    if(activeElement > 0)
                    {
                        activeElement--;
                    }
                    else
                    {
                        activeElement = menuOptions.Length - 1;
                    }
                }

                if(Input.GetKeyUp(KeyCode.DownArrow))
                {
                    audioManager.soundPlay("Effect_Click");
                    menuOptions[activeElement].selected = false;
                    if(activeElement < menuOptions.Length - 1)
                    {
                        activeElement++;
                    }
                    else
                    {
                        activeElement = 0;
                    }
                }

                // if space button hit again
                if(Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Jump"))
                {
                    audioManager.soundPlay("Effect_Win");
                    //load next level
                    loadingLevel = true;
                    StartCoroutine(LoadLevel());
                    menuOptions[activeElement].transform.localScale *= 1.25f;
                }
            }
        }
    }

    // game mode to determin if second fighter is ai or second player input
    void HandleSelectionOption()
    {
        switch(activeElement)
        {
            case 0:
                CharacterManager.getInstance().numberOfUsers = 1;
                break;
            case 1:
                CharacterManager.getInstance().numberOfUsers = 2;
                CharacterManager.getInstance().players[1].playerType = PlayerBase.PlayerType.user;
                break;
        }
    }

    // loads level after character manager is set for fighter selection
    IEnumerator LoadLevel()
    {
        HandleSelectionOption();
        yield return new WaitForSeconds(0.6f);
        MySceneManager.GetInstance().RequestLevelOnLoad(MySceneManager.SceneType.main, "select");
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}
