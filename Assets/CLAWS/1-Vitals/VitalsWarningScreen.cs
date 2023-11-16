using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VitalsWarningScreen : MonoBehaviour
{
    private Subscription<VitalsUpdatedEvent> vitalsUpdateEvent;
    private Subscription<FellowAstronautVitalsDataChangeEvent> fellowVitalsUpdateEvent;
    public GameObject parent;
    //public GameObject parentFellow;
    TextMeshPro heartRate, warningText;

    // Start is called before the first frame update
    void Start()
    {
        parent.SetActive(false);
        //parentFellow.SetActive(false);
        vitalsUpdateEvent = EventBus.Subscribe<VitalsUpdatedEvent>(checkVitalsWarning);
        fellowVitalsUpdateEvent = EventBus.Subscribe<FellowAstronautVitalsDataChangeEvent>(checkFellowVitalsWarning);
        heartRate = parent.transform.Find("HeartRate").gameObject.GetComponent<TextMeshPro>();
        warningText = parent.transform.Find("WarningText").gameObject.GetComponent<TextMeshPro>();
    }

    private void checkVitalsWarning(VitalsUpdatedEvent e)
    {
        //int.TryParse(heartRate.text, out int heartRateResult);
        //int.Parse(heartRate.text);
        //parent.SetActive(true);
        Debug.Log("heartRate text = " + heartRate.text);
        Debug.Log("int:" + int.Parse(heartRate.text));
        if (int.Parse(heartRate.text) > 20){
            parent.SetActive(true);
            heartRate.text = e.vitals.heart_rate.ToString();
            warningText.text = "WARNING HERE";
        }
    }

    private void checkFellowVitalsWarning(FellowAstronautVitalsDataChangeEvent e)
    {

    }

}
