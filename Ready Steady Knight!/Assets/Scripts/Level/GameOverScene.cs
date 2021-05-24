using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScene : MonoBehaviour
{
    // script created to display game over scene, plays game over music and then returns user to intro scene.
    private AudioManagerScript audioManager;
    private void Start()
    {
        audioManager = AudioManagerScript.getInstance();
        audioManager.soundPlay("Effect_GameOver");
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3);
        MySceneManager.GetInstance().RequestLevelOnLoad(MySceneManager.SceneType.main, "intro");
    }
}
