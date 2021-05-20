using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public List<string> levels = new List<string>();
    public List<MainScene> mainScenes = new List<MainScene>();

    bool waitToLoad;

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
public class MainScene
{
    public string levelId;
}
