using UnityEngine;
using TMPro;

public class VitalsSwitcherFinal : MonoBehaviour
{
    private GameObject screen1;
    private GameObject screen2;
    private GameObject button;
    TextMeshPro buttonNumber;
    TextMeshPro buttonAstro;
    bool main;

    void Start()
    {
        screen1 = transform.Find("SuitsControlScreen1").gameObject;
        screen2 = transform.Find("SuitsControlScreen2").gameObject;
        button = transform.Find("SwitchVitalsButton").gameObject;
        screen2.SetActive(false);
        screen1.SetActive(false);
        button.SetActive(false);

        main = true;

        buttonNumber = button.transform.Find("Number").GetComponent<TextMeshPro>();
        buttonAstro = button.transform.Find("Astronaut").GetComponent<TextMeshPro>();
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
                buttonNumber.text = "2";
                buttonAstro.text = "Astronaut 2";
            } 
            else
            {
                screen1.SetActive(false);
                screen2.SetActive(true);
                buttonNumber.text = "1";
                buttonAstro.text = "Astronaut 1";
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
                buttonNumber.text = "1";
                buttonAstro.text = "Astronaut 1";
            }
            else
            {
                screen2.SetActive(false);
                screen1.SetActive(true);
                buttonNumber.text = "2";
                buttonAstro.text = "Astronaut 2";
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
