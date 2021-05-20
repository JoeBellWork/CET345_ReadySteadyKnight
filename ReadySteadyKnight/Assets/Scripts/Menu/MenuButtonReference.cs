using UnityEngine;

[System.Serializable]
public class MenuButtonReference : MonoBehaviour
{
    public GameObject selectIndicator;
    public bool selected;

    private void Start()
    {
        selectIndicator.SetActive(false);
    }

    private void Update()
    {
        selectIndicator.SetActive(selected);
    }
}
