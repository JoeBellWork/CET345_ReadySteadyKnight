using System.Collections;
using UnityEngine;
using TMPro;

public class YouWinScene : MonoBehaviour
{
    // script created to display game over scene, plays game over music and then returns user to intro scene.
    private AudioManagerScript audioManager;
    private LevelManager levelManager;
    private CharacterManager charM;
    public  TextMeshProUGUI winText;
    private void Start()
    {
        levelManager = LevelManager.getInstance();
        charM = CharacterManager.getInstance();
        
        if(!charM.solo)
        {
            if (levelManager.winner == 0)
            {
                winText.text = "Well done, Fighter 1! \n You win!";
            }
            else
            {
                winText.text = "Well done, Fighter 2! \n You win!";
            }
        }
        else
        {
            winText.text = "Well done, Champion! \n You win!";
        }
        
        audioManager = AudioManagerScript.getInstance();
        audioManager.soundPlay("Effect_Win");
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3);
        MySceneManager.GetInstance().RequestLevelOnLoad(MySceneManager.SceneType.main, "intro");
    }
}
