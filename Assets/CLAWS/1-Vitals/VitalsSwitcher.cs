using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add the missing semicolon here

public class VitalsSwitcher : MonoBehaviour
{
    public GameObject firstAst;
    public GameObject button;

    TextMeshPro buttonText;

    void Start()
    {
        firstAst.SetActive(true);
        buttonText = button.transform.Find("ButtonText").gameObject.GetComponent<TextMeshPro>();
        buttonText.text = "Astronant 1";
    }

    // Change AstrounantToggle to Awake
    public void AstronantToggle()
    {
        if (firstAst.activeSelf)
        {
            firstAst.SetActive(false);
            buttonText.text = "Astronant 2";
        }
        else
        {
            firstAst.SetActive(true);
            buttonText.text = "Astronant 1";
        }
    }
}
