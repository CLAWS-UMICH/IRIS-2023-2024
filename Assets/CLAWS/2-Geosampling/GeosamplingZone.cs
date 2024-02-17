using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeosamplingZone : MonoBehaviour
{
    private Subscription<GeosampleModeStartedEvent> geosampleModeStartedEvent;
    private Subscription<GeosampleModeEndedEvent> geosampleModeEndedEvent;
    private Subscription<GeosamplesAddedEvent> geosampleAddedEvent;

    public TextMeshPro label;

    public bool isEntered = false;
    public Location location;

    public float offsetBelow;
    public float radius;

    public int sessionSamples = 0;

    public void Init()
    {
        // Creates a Geosample Zone at the user's current location 

        geosampleModeStartedEvent = EventBus.Subscribe<GeosampleModeStartedEvent>(OnGeosampleModeStarted);
        geosampleModeEndedEvent = EventBus.Subscribe<GeosampleModeEndedEvent>(OnGeosampleModeEnded);
        geosampleAddedEvent = EventBus.Subscribe<GeosamplesAddedEvent>(OnGeosampleAdded);

        transform.position = Camera.main.transform.position - new Vector3(0f, offsetBelow, 0f);
        location = GPSUtils.AppPositionToGPSCoords(transform.position);
        GeoSampleLabel();
        
        StartCoroutine(TrackUserLocation());
    }

    private void GeoSampleLabel()
    {
        // creating geosample zone textmeshpro
        int ascii = (int)'A';
        ascii += sessionSamples;
        string zoneLabel = "";
        while (ascii > 90)
        {
            zoneLabel += "Z";
            ascii -= 90;
        }
        zoneLabel = ((char)ascii).ToString();
        label.text = zoneLabel;
    }

    private void OnGeosampleModeStarted(GeosampleModeStartedEvent e)
    {
        // show perimeter
        sessionSamples = 0;
        StartCoroutine(TrackUserLocation());
    }

    private void OnGeosampleModeEnded(GeosampleModeEndedEvent e)
    {
        // hide perimeter

        isEntered = false;
    }

    private void OnGeosampleAdded(GeosamplesAddedEvent e)
    {
        sessionSamples += 1;
    }

    IEnumerator TrackUserLocation()
    {
        while (GeosamplingManager.GeosamplingMode)
        {
            if (isEntered)
            {
                if (Vector3.Distance(Camera.main.transform.position, transform.position) > radius)
                {
                    OnZoneExited();
                }
            }
            else
            {
                if (Vector3.Distance(Camera.main.transform.position, transform.position) < radius)
                {
                    OnZoneEntered();
                }
            }

            yield return new WaitForSeconds(2f);
        }
    }

    private void OnZoneEntered()
    {
        isEntered = true;

        // todo show geosamples
    }

    private void OnZoneExited()
    {
        isEntered = false;

        GeosamplingManager.EndGeosamplingMode();
    }








    private void OnDestroy()
    {
        if (geosampleModeStartedEvent != null)
        {
            EventBus.Unsubscribe(geosampleModeStartedEvent);
        }
        if (geosampleModeEndedEvent != null)
        {
            EventBus.Unsubscribe(geosampleModeEndedEvent);
        }
        if (geosampleAddedEvent != null)
        {
            EventBus.Unsubscribe(geosampleAddedEvent);
        }
    }
}
