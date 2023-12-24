using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressAnimation : MonoBehaviour
{
    private Subscription<VitalsUpdatedEvent> vitalsUpdateEvent;
    private Subscription<FellowAstronautVitalsDataChangeEvent> fellowVitalsUpdateEvent;
    private float maxScale;

    public GameObject FullProgressBar;
    public GameObject EmptyProgressBar;
    public TextMeshPro value;
    public GameObject indicator;

    //adjust as needed
    public float maxVal;
    public float val;
    public float fill;

    //for indicator
    private Vector3 currentPosition;
    private Vector3 objectSize;

    private void Start()
    {
        vitalsUpdateEvent = EventBus.Subscribe<VitalsUpdatedEvent>(onVitalsUpdate);
        //fellowVitalsUpdateEvent = EventBus.Subscribe<FellowAstronautVitalsDataChangeEvent>(onFellowVitalsUpdate);
        FullProgressBar.SetActive(false);
        val = 0f;
        maxVal = 100f;
        maxScale = FullProgressBar.transform.localScale.x;

        ////for indicator
        //currentPosition = FullProgressBar.transform.position;
        //objectSize = FullProgressBar.transform.localScale;
    }

    //private void Update()
    //{
    //    //testing with heart rate
    //    value.text = val.ToString();

    //    // Limit fill within the range of 0 to current scale
    //    fill = Mathf.Clamp(val/maxVal * maxScale , 0f, maxScale);
    //    //Debug.Log(fill);

    //    // Adjust the scale of the fill bar based on the fill
    //    Vector3 newScale = FullProgressBar.transform.localScale;
    //    newScale.x = fill;
    //    FullProgressBar.transform.localScale = newScale;
    //    FullProgressBar.SetActive(true);
    //}

    // can change to Update if needed
    // doesn't show in Game Mode
    private void onVitalsUpdate(VitalsUpdatedEvent e)
    {
        //testing with heart rate
        val = e.vitals.heart_rate;
        value.text = val.ToString();

        // limit fill
        fill = Mathf.Clamp(val / maxVal * maxScale, 0f, maxScale);

        // adjust the scale of the fill bar based on the fill
        Vector3 newScale = FullProgressBar.transform.localScale;
        newScale.x = fill;

        //// move indicator fix this if needed
        //float rightEdge = currentPosition.x + (objectSize.x * 0.5f);
        //Debug.Log(rightEdge);
        //indicator.transform.position = new Vector3(rightEdge, currentPosition.y, currentPosition.z);

        // update everything
        FullProgressBar.transform.localScale = newScale;
        FullProgressBar.SetActive(true);
    }

}
