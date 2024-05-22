using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GeosamplingZone : MonoBehaviour
{
    private Subscription<GeosampleModeStartedEvent> geosampleModeStartedEvent;
    private Subscription<GeosampleModeEndedEvent> geosampleModeEndedEvent;
    private Subscription<GeosamplesAddedEvent> geosampleAddedEvent;
    private Subscription<GeosampleZoneDeletedEvent> geosamplezoneDeletedEvent;

    public static int NumZones = 0;
    public static string CurrentZone = "";

    public static GeosampleZone FindZone(char zone_id)
    {
        foreach (GeosampleZone zone in AstronautInstance.User.GeosampleZonesData.AllGeosampleZones) {
            if (zone.zone_id == zone_id)
            {
                return zone;
            }
        }

        Debug.LogError("Geosample Zone Not Found");
        return null;
    }

    public TextMeshPro label;
    public bool isEntered = false;
    public Location location;
    public GeosampleZone Zone;

    public float offsetBelow;
    public float radius;

    public int zoneSamples = 0;

    // Geosample Zone Notification
    // need to set the Zone Notification on default not shown either in unity or in code
    private GameObject GeoSampleZoneNotif;
    private TextMeshPro GeoSampleZoneScanned;
    //private TextMeshPro GeoSampleZoneStarred;

    public void Init()
    {
        // Creates a Geosample Zone at the user's current location 
        geosampleModeStartedEvent = EventBus.Subscribe<GeosampleModeStartedEvent>(OnGeosampleModeStarted);
        geosampleModeEndedEvent = EventBus.Subscribe<GeosampleModeEndedEvent>(OnGeosampleModeEnded);
        geosampleAddedEvent = EventBus.Subscribe<GeosamplesAddedEvent>(OnGeosampleAdded);
        geosamplezoneDeletedEvent = EventBus.Subscribe<GeosampleZoneDeletedEvent>(OnGeosamplezoneDeleted);


        transform.position = Camera.main.transform.position - new Vector3(0f, offsetBelow, 0f);
        location = GPSUtils.AppPositionToGPSCoords(transform.position);
        zoneSamples = 0;
        radius = 3;
        GeoSampleLabel();

        // Update backend
        Zone = new GeosampleZone();
        Zone.zone_id = (char)('A' + (char)(NumZones++ % 27));
        Zone.radius = 3;
        Zone.location = location;
        Zone.ZoneGeosamplesIds = new();

        AstronautInstance.User.GeosampleZonesData.AllGeosampleZones.Add(Zone);

        GeosamplingManager.SendData();
        StartCoroutine(TrackUserLocation());
    }

    public void Load(GeosampleZone Zone_f)
    {
        geosampleModeStartedEvent = EventBus.Subscribe<GeosampleModeStartedEvent>(OnGeosampleModeStarted);
        geosampleModeEndedEvent = EventBus.Subscribe<GeosampleModeEndedEvent>(OnGeosampleModeEnded);
        geosampleAddedEvent = EventBus.Subscribe<GeosamplesAddedEvent>(OnGeosampleAdded);

        transform.position = GPSUtils.GPSCoordsToAppPosition(Zone_f.location) + new Vector3(0, Camera.main.transform.position.y - offsetBelow, 0);
        location = Zone_f.location;
        zoneSamples = Zone_f.ZoneGeosamplesIds.Count;
        radius = Zone_f.radius;
        GeoSampleLabel();

        StartCoroutine(TrackUserLocation());
    }

    private void GeoSampleLabel()
    {
        // creating geosample zone textmeshpro
        string text = ((char)((int)'A' + (zoneSamples % 27))).ToString();
        label.text = text;
    } 

    private void OnGeosampleModeStarted(GeosampleModeStartedEvent e)
    {
        // show perimeter
        transform.Find("BoundaryCircle").GetComponent<LineRender>().ShowBoundary();
        StartCoroutine(TrackUserLocation());
    }

    private void OnGeosampleModeEnded(GeosampleModeEndedEvent e)
    {
        transform.Find("BoundaryCircle").GetComponent<LineRender>().HideBoundary();
        isEntered = false;
    }

    private void OnGeosampleAdded(GeosamplesAddedEvent e)
    {
        zoneSamples += 1;

        // TODO update zone object
        // Find Notif Game Object
        GeoSampleZoneScanned.text = zoneSamples.ToString();
    }

    private void OnGeosamplezoneDeleted(GeosampleZoneDeletedEvent e)
    {
        foreach (GeosampleZone geosamplezone in e.DeletedGeoSampleZones)
        {
            if (geosamplezone.zone_id == Zone.zone_id)
            {
                Destroy(gameObject);
            }
        }
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
        CurrentZone = Zone.zone_id.ToString();

        EventBus.Publish<GeosampleZoneEnteredEvent>(new(Zone.zone_id.ToString()));
    }

    private void OnZoneExited()
    {
        Debug.Log("Geosample Zone Exited: " + Zone.ToString());

        isEntered = false;
        CurrentZone = "";

        EventBus.Publish<GeosampleZoneExitedEvent>(new(Zone.zone_id.ToString()));
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
        if (geosamplezoneDeletedEvent != null)
        {
            EventBus.Unsubscribe(geosamplezoneDeletedEvent);
        }
    }
}
