using UnityEngine;
using TMPro;

public class VitalsSwitcherFinal : MonoBehaviour
{
    public GameObject suitsControl;
    public GameObject screen1;
    public GameObject screen2;
    public GameObject button;
    string buttonText;
    bool main;

    void Start()
    {
        suitsControl = GameObject.Find("Vitals");
        screen1 = suitsControl.transform.Find("SuitsControlScreen1").gameObject;
        screen2 = suitsControl.transform.Find("SuitsControlScreen2").gameObject;
        screen2.SetActive(false);
        screen1.SetActive(false);
        button.SetActive(false);

        main = true;

        button = GameObject.Find("SwitchVitalsButton").gameObject;
        buttonText = button.transform.Find("IconAndText").transform.Find("TextMeshPro").GetComponent<TextMeshPro>().text;
    }

    public void AstronautToggle()
    {
        if (main)
        {
            // Opened Main vtials
            if (AstronautInstance.User.id == 0)
            {
                screen2.SetActive(false);
                screen1.SetActive(true);
                buttonText = "Astronaut 2";
            } else
            {
                screen1.SetActive(false);
                screen2.SetActive(true);
                buttonText = "Astronaut 1";
            }


            EventBus.Publish<ScreenChangedEvent>(new ScreenChangedEvent(Screens.Vitals_Main));

            main = false;
        }
        else
        {
            // Opened fellow vitals
            if (AstronautInstance.User.id == 0)
            {
                screen1.SetActive(false);
                screen2.SetActive(true);
                buttonText = "Astronaut 1";
            }
            else
            {
                screen2.SetActive(false);
                screen1.SetActive(true);
                buttonText = "Astronaut 2";
            }

            EventBus.Publish<ScreenChangedEvent>(new ScreenChangedEvent(Screens.Vitals_Fellow));

            main = true;
        }
        button.SetActive(true);
    }

    public void Close()
    {
        EventBus.Publish<ScreenChangedEvent>(new ScreenChangedEvent(Screens.Menu));
        screen2.SetActive(false);
        screen1.SetActive(false);
        button.SetActive(false);
        main = true;
    }
}