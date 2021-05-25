using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToIntro : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(1);
    }
}
