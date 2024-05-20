using UnityEngine;
using TMPro;

public class VitalsSwitcherFinal : MonoBehaviour
{
    public GameObject suitsControl;
    public GameObject screen1;
    public GameObject screen2;
    public GameObject button;
    string buttonText;

    void Start()
    {
        suitsControl = GameObject.Find("SuitsControl");
        screen1 = suitsControl.transform.Find("SuitsControlScreen1").gameObject;
        screen2 = suitsControl.transform.Find("SuitsControlScreen2").gameObject;
        screen2.SetActive(false);

        button = GameObject.Find("SwitchVitalsButton");
        buttonText = button.transform.Find("IconAndText").transform.Find("TextMeshPro").GetComponent<TextMeshPro>().text;

        buttonText = "Astronaut 2";
    }

    public void AstronautToggle()
    {
        if (screen1.activeSelf)
        {
            screen1.SetActive(false);
            screen2.SetActive(true);
            buttonText = "Astronaut 1";

            // Replace with your actual event publishing logic
            Debug.Log("Switched to Astronaut 1");
        }
        else
        {
            screen1.SetActive(true);
            screen2.SetActive(false);
            buttonText = "Astronaut 2";

            // Replace with your actual event publishing logic
            Debug.Log("Switched to Astronaut 2");
        }
    }
}
