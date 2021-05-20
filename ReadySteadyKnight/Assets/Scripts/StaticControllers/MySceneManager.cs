using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
   public int progressionStages = 5;
    public List<string> levels = new List<string>();
    public List<MainScene> mainScenes = new List<MainScene>();

    bool waitToLoad;
    int progIndex = 0;
    public List<SoloProgression> progression = new List<SoloProgression>();
    CharacterManager charM;

    private void Start()
    {
        charM = CharacterManager.getInstance();
    }

    public void createProgression()
    {
        progression.Clear();

        List<int> usedCharacters = new List<int>();
        int playerInt = charM.ReturnCharacterInt(charM.players[0].playerPrefab);
        usedCharacters.Add(playerInt);

        if(progressionStages > charM.characterList.Count - 1)
        {
            progressionStages = charM.characterList.Count - 2;
        }

        for (int i = 0; i < progressionStages; i++)
        {
            SoloProgression s = new SoloProgression();
            int levelInt = Random.Range(0, levels.Count);
            s.levelID = levels[levelInt];

            int charInt = UniqueRandomInt(usedCharacters, 0, charM.characterList.Count);
            s.charId = charM.characterList[charInt].charId;
            usedCharacters.Add(charInt);
            progression.Add(s);
        }
    }

    public void LoadNextOnProgression()
    {
        string targetId = "";
        SceneType sceneType = SceneType.prog;
        if(progIndex > progression.Count - 1)
        {
            targetId = "intro";
            sceneType = SceneType.main;
            Debug.Log(progIndex);
        }
        else
        {
            targetId = progression[progIndex].levelID;
            charM.players[1].playerPrefab = charM.returnCharacterWithId(progression[progIndex].charId).prefab;
            progIndex++;
        }
        RequestLevelOnLoad(sceneType, targetId);
    }

    public void RequestLevelOnLoad(SceneType st, string level)
    {
        if(!waitToLoad)
        {
            string targetId = "";

            switch (st)
            {
                case SceneType.main:
                    targetId = ReturnMainScene(level).levelId;
                    break;
                case SceneType.prog:
                    targetId = level;
                    break;
            }
            StartCoroutine(LoadScene(level));
            waitToLoad = true;

        }
    }

    IEnumerator LoadScene(string levelSt)
    {
        yield return SceneManager.LoadSceneAsync(levelSt, LoadSceneMode.Single);
        waitToLoad = false;
    }

    MainScene ReturnMainScene(string level)
    {
        MainScene r = null;
        for (int i = 0; i < mainScenes.Count; i++)
        {
            if(mainScenes[i].levelId == level)
            {
                r = mainScenes[i];
                break;
            }
        }
        return r;
    }


    int UniqueRandomInt(List<int> l, int min, int max)
    {
        int retVal = Random.Range(min, max);
        while (l.Contains(retVal))
        {
            retVal = Random.Range(min, max);
        }
        return retVal;
    }

    public static MySceneManager instance;
    public static MySceneManager GetInstance()
    {
        return instance;
    }



    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public enum SceneType
    { 
        main,
        prog
    }
}

[System.Serializable]
public class SoloProgression
{
    public string charId;
    public string levelID;
}


[System.Serializable]
public class MainScene
{
    public string levelId;
}
