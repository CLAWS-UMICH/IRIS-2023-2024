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
    TextMeshPro heartRate, H2O, O2, heartRate1, H2O1, O21;

    void Start()
    {
        vitalsUpdateEvent = EventBus.Subscribe<VitalsUpdatedEvent>(onVitalsUpdate);
        fellowVitalsUpdateEvent = EventBus.Subscribe<FellowAstronautVitalsDataChangeEvent>(onFellowVitalsUpdate);

    }

    private void onVitalsUpdate(VitalsUpdatedEvent e)
    {
        heartRate.text = e.vitals.heart_rate.ToString();
        H2O.text = e.vitals.h2o_gas_pressure.ToString();
        O2.text = e.vitals.primary_oxygen.ToString();
    }

    private void onFellowVitalsUpdate(FellowAstronautVitalsDataChangeEvent e) {
        heartRate1.text = e.AstronautToChange.vitals.heart_rate.ToString();
    }
}
