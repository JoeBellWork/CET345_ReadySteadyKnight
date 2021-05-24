using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    // script set to single instance objct, carried between scenes, controls the traversal between levels.
    public List<string> levels = new List<string>();
    public List<MainScene> mainScenes = new List<MainScene>();

    bool waitToLoad;

    // uses switch bool system to discover what level to move between
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

    IEnumerator LoadScene(string levelSt) // specific IEnumerator that loads the scene passed and turns off bool to allow levels to be switched again.
    {
        yield return SceneManager.LoadSceneAsync(levelSt, LoadSceneMode.Single);
        waitToLoad = false;
    }

    MainScene ReturnMainScene(string level)
    {
        // returns specific main levels to allow traversal between game over and intro scenes.
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
    
    // static controls
    public static MySceneManager instance;
    public static MySceneManager GetInstance()
    {
        return instance;
    }

    // generate single instance across scenes
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // enum for scene types to allow specific selection
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
