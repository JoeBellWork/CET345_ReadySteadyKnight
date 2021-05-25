using UnityEngine;

public class ButtonRef : MonoBehaviour
{
    // rather than use standard unity button displays
    // this projct allos users to move a selction choice by seting th SelectIndicator game object on and off depnding on highlighted option.
    public GameObject selectIndicator;

    public bool selected;

    void Start()
    {
        selectIndicator.SetActive(false);
    }

    void Update()
    {
        selectIndicator.SetActive(selected);
    }
}
