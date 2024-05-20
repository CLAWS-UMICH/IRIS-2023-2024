using UnityEngine;
using TMPro;

public class VitalsSwitcherFinal : MonoBehaviour
{
    public GameObject suitsControl;
    public GameObject screen1;
    public GameObject screen2;
    public GameObject button;
    string buttonText;
    bool on;

    void Start()
    {
        suitsControl = GameObject.Find("Vitals");
        screen1 = suitsControl.transform.Find("SuitsControlScreen1").gameObject;
        screen2 = suitsControl.transform.Find("SuitsControlScreen2").gameObject;
        screen2.SetActive(false);
        screen1.SetActive(false);
        button.SetActive(false);
        on = true;

        button = GameObject.Find("SwitchVitalsButton");
        buttonText = button.transform.Find("IconAndText").transform.Find("TextMeshPro").GetComponent<TextMeshPro>().text;

        buttonText = "Astronaut 2";
    }

    public void AstronautToggle()
    {
        if (on)
        {
            screen1.SetActive(false);
            screen2.SetActive(true);
            buttonText = "Astronaut 1";

            // Replace with your actual event publishing logic
            Debug.Log("Switched to Astronaut 1");
            on = !on;
        }
        else
        {
            screen1.SetActive(true);
            screen2.SetActive(false);
            buttonText = "Astronaut 2";

            // Replace with your actual event publishing logic
            Debug.Log("Switched to Astronaut 2");
            on = !on;
        }
    }

    public void Close()
    {
        screen2.SetActive(false);
        screen1.SetActive(false);
        button.SetActive(false);
        on = true;
        buttonText = "Astronaut 2";
    }
}
