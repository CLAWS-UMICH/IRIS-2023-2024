using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIA_Button : MonoBehaviour
{
    [SerializeField] GameObject OpenButton;
    [SerializeField] GameObject CloseButton;

    void Start()
    {
        HideButton();
    }

    public void HideButton()
    {
        OpenButton.SetActive(false);
        CloseButton.SetActive(false);
    }

    public void ShowButton_Up()
    {
        OpenButton.SetActive(true);
        CloseButton.SetActive(false);
    }

    public void ShowButton_Down()
    {
        OpenButton.SetActive(false);
        CloseButton.SetActive(true);
    }
}
