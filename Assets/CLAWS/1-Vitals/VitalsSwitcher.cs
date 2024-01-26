using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add the missing semicolon here

public class VitalsSwitcher : MonoBehaviour
{
    public GameObject firstAst;
    public GameObject secondAst;
    public GameObject button;

    TextMeshPro buttonText;

    void Start()
    {
        firstAst.SetActive(false);
        secondAst.SetActive(false);
        button.SetActive(false);
        buttonText = button.transform.Find("Button").transform.Find("ButtonText").gameObject.GetComponent<TextMeshPro>();
        buttonText.text = "Astronaut 2";
    }

    public void CloseVitals()
    {
        firstAst.SetActive(false);
        secondAst.SetActive(false);
        button.SetActive(false);

        buttonText.text = "Astronaut 2";
        EventBus.Publish(new ScreenChangedEvent(Screens.Menu));
    }

    public void OpenVitals()
    {
        firstAst.SetActive(true);
        button.SetActive(true);
        secondAst.SetActive(false);

        buttonText.text = "Astronaut 2";

        EventBus.Publish(new ScreenChangedEvent(Screens.Vitals_1));

    }

    // Change AstrounantToggle to Awake
    public void AstronantToggle()
    {
        if (firstAst.activeSelf)
        {
            firstAst.SetActive(false);
            secondAst.SetActive(true);
            buttonText.text = "Astronaut 1";

            EventBus.Publish(new ScreenChangedEvent(Screens.Vitals_1));
        }
        else
        {
            firstAst.SetActive(true);
            secondAst.SetActive(false);
            buttonText.text = "Astronaut 2";

            EventBus.Publish(new ScreenChangedEvent(Screens.Vitals_2));
        }
    }
}
