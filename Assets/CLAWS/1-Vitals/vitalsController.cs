using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class vitalsController : MonoBehaviour
{

    private Subscription<VitalsUpdatedEvent> vitalsUpdateEvent;
    private Subscription<FellowAstronautVitalsDataChangeEvent> fellowVitalsUpdateEvent;
    public GameObject parent;
    public GameObject parentFellow;
    TextMeshPro heartRate, H2O, O2, O2timeLeft, O2rate, H2OtimeLeft, heartRate1, H2O1, O21, batteryText;

    void Start()
    {
        vitalsUpdateEvent = EventBus.Subscribe<VitalsUpdatedEvent>(onVitalsUpdate);
        fellowVitalsUpdateEvent = EventBus.Subscribe<FellowAstronautVitalsDataChangeEvent>(onFellowVitalsUpdate);
        heartRate = parent.transform.Find("HeartRate").gameObject.GetComponent<TextMeshPro>();
        H2O = parent.transform.Find("H2O").gameObject.GetComponent<TextMeshPro>();
        O2 = parent.transform.Find("O2").gameObject.GetComponent<TextMeshPro>();
        O2timeLeft = parent.transform.Find("O2TimeLeft").gameObject.GetComponent<TextMeshPro>();
        H2OtimeLeft = parent.transform.Find("H2OTimeLeft").gameObject.GetComponent<TextMeshPro>();
        O2rate = parent.transform.Find("O2Rate").gameObject.GetComponent<TextMeshPro>();
    }

    private void onVitalsUpdate(VitalsUpdatedEvent e)
    {
        heartRate.text = "HeartRate: " + e.vitals.heart_rate.ToString();
        H2O.text = "H2O: " + e.vitals.h2o_gas_pressure.ToString();
        O2.text = "O2: " + e.vitals.primary_oxygen.ToString();
        O2timeLeft.text = "O2 time remaining: " + e.vitals.o2_time_left.ToString();
        H2OtimeLeft.text = "H2O time remaining: " + e.vitals.h2o_time_left.ToString();
        O2rate.text = "O2 Rate: " + e.vitals.o2_rate.ToString();

        // Add warning screens + when rates are going to fast
    }

    private void onFellowVitalsUpdate(FellowAstronautVitalsDataChangeEvent e) {
        heartRate1.text = e.AstronautToChange.vitals.heart_rate.ToString();
    }
}
