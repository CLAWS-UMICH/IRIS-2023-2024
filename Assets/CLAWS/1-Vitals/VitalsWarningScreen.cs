using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 Something is happening here
 */

/* 
 * Things to do
 * If there are multiple warinings have something similar to the Task lisk Controller?
 * How will make warnings disappear (after vitals return to normal disappear after x seconds)
 */

public class VitalsWarningScreen : MonoBehaviour
{
    private Subscription<VitalsUpdatedEvent> vitalsUpdateEvent;
    private Subscription<FellowAstronautVitalsDataChangeEvent> fellowVitalsUpdateEvent;
    public GameObject parent;
    //public GameObject parentFellow;
    TextMeshPro heartRate, warningText;
    //private warningTime;

    // Start is called before the first frame update
    void Start()
    {
        parent.SetActive(false);
        //parentFellow.SetActive(false);

        // Subscribe to the Events
        vitalsUpdateEvent = EventBus.Subscribe<VitalsUpdatedEvent>(checkVitalsWarning);
        fellowVitalsUpdateEvent = EventBus.Subscribe<FellowAstronautVitalsDataChangeEvent>(checkFellowVitalsWarning);


        // Making the Objects
        //heartRate = parent.transform.Find("HeartRate").gameObject.GetComponent<TextMeshPro>();
        warningText = parent.transform.Find("WarningText").gameObject.GetComponent<TextMeshPro>();
    }

    private void checkVitalsWarning(VitalsUpdatedEvent e)
    {
        // DEBUGGING
        //int.TryParse(heartRate.text, out int heartRateResult);
        //int.Parse(heartRate.text);
        //parent.SetActive(true);
        //Debug.Log("heartRate = " + e.vitals.heart_rate);
        //Debug.Log("heartRate text = " + heartRate.text);
        //Debug.Log("int:" + int.Parse(heartRate.text));

        // Heart Rate
        if (e.vitals.heart_rate < 50){
            parent.SetActive(true);
            warningText.text = "Heart Rate:" + e.vitals.heart_rate.ToString();
            //warningText.text = "TEST WARNING HERE";
        }

        // O2


    }

    private void checkFellowVitalsWarning(FellowAstronautVitalsDataChangeEvent e)
    {

    }

}
