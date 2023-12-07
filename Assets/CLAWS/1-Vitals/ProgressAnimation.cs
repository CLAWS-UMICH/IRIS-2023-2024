using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressAnimation : MonoBehaviour
{
    public Image imageToFill;
    public GameObject vitalsMask;
    private Subscription<VitalsUpdatedEvent> vitalsUpdateEvent;
    private Subscription<FellowAstronautVitalsDataChangeEvent> fellowVitalsUpdateEvent;
    public float vitalsData;
    Transform quadTransform;
    public float maxDataValue;
    public float maxRotation;
    public float fillPercentage;

    // Start is called before the first frame update
    void Start()
    {
        fillPercentage = 0.5f;
        vitalsMask.SetActive(false);
        maxDataValue = 100f; // Maximum data value (0-100)
        maxRotation = 360f;
        quadTransform = vitalsMask.transform;
        vitalsData = 0;
        vitalsUpdateEvent = EventBus.Subscribe<VitalsUpdatedEvent>(onVitalsUpdate);
        fellowVitalsUpdateEvent = EventBus.Subscribe<FellowAstronautVitalsDataChangeEvent>(onFellowVitalsUpdate);
    }
    public void onVitalsUpdate(VitalsUpdatedEvent e)
    {
        // can't test if this is working since most recent pull messed things up :(
        // missing prefabs and other things????
        // float clampedProgress = Mathf.Clamp(vitalsData, 0f, 250f);
        // Does this really work though?

        vitalsData = e.vitals.heart_rate;
        vitalsMask.SetActive(true);
        float fillPercentage = Mathf.Clamp01(e.vitals.heart_rate / maxDataValue);
        float fillAngle = fillPercentage * maxRotation;
        vitalsMask.transform.localRotation = Quaternion.Euler(0f, 0f, -fillAngle);

    }
    public void onFellowVitalsUpdate(FellowAstronautVitalsDataChangeEvent e)
    {
        // Is this needed?
    }
}
