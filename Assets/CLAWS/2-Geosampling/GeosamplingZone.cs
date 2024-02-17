using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GeosamplingZone : MonoBehaviour
{
    private Subscription<GeosampleModeStartedEvent> geosampleModeStartedEvent;
    private Subscription<GeosampleModeEndedEvent> geosampleModeEndedEvent;
    private Subscription<GeosamplesAddedEvent> geosampleAddedEvent;

    public static int numZones = 0;

    public bool isEntered = false;
    public Location location;
    public GeosampleZone Zone;

    public float offsetBelow;
    public float radius;

    public int zoneSamples = 0;

    public void Init()
    {
        // Creates a Geosample Zone at the user's current location 
        geosampleModeStartedEvent = EventBus.Subscribe<GeosampleModeStartedEvent>(OnGeosampleModeStarted);
        geosampleModeEndedEvent = EventBus.Subscribe<GeosampleModeEndedEvent>(OnGeosampleModeEnded);
        geosampleAddedEvent = EventBus.Subscribe<GeosamplesAddedEvent>(OnGeosampleAdded);

        transform.position = Camera.main.transform.position - new Vector3(0f, offsetBelow, 0f);
        location = GPSUtils.AppPositionToGPSCoords(transform.position);
        zoneSamples = 0;
        radius = 3;

        // Update backend
        Zone = new GeosampleZone();
        Zone.zone_id = (char)('A' + (char)(numZones++ % 27));
        Zone.radius = 3;
        Zone.location = location;
        Zone.ZoneGeosamplesIds = new();
        
        GeosamplingManager.SendData();
        
        StartCoroutine(TrackUserLocation());
    }

    private void OnGeosampleModeStarted(GeosampleModeStartedEvent e)
    {
        GetComponent<LineRender>().ShowBoundary();

        StartCoroutine(TrackUserLocation());
    }

    private void OnGeosampleModeEnded(GeosampleModeEndedEvent e)
    {
        GetComponent<LineRender>().HideBoundary();

        isEntered = false;
    }

    private void OnGeosampleAdded(GeosamplesAddedEvent e)
    {
        zoneSamples += 1;

        // TODO update zone object
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
        Debug.Log("Geosample Zone Entered: " + Zone.ToString());

        isEntered = true;

        // todo show geosamples
        // todo update current zone and upper left
    }

    private void OnZoneExited()
    {
        Debug.Log("Geosample Zone Exited: " + Zone.ToString());

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
