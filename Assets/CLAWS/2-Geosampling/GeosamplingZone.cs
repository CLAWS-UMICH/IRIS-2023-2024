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

    public static int NumZones = 0;
    public static string CurrentZone = "";

    public TextMeshPro label;
    public bool isEntered = false;
    public Location location;
    public GeosampleZone Zone;

    public float offsetBelow;
    public float radius;

    public int zoneSamples = 0;

    // Geosample Zone Notification
    private GameObject GeoSampleZoneNotif;
    private TextMeshPro GeoSampleZoneScanned;
    //private TextMeshPro GeoSampleZoneStarred;

    public void Init()
    {
        // Creates a Geosample Zone at the user's current location 
        geosampleModeStartedEvent = EventBus.Subscribe<GeosampleModeStartedEvent>(OnGeosampleModeStarted);
        geosampleModeEndedEvent = EventBus.Subscribe<GeosampleModeEndedEvent>(OnGeosampleModeEnded);
        geosampleAddedEvent = EventBus.Subscribe<GeosamplesAddedEvent>(OnGeosampleAdded);

        transform.position = Camera.main.transform.position - new Vector3(0f, offsetBelow, 0f);
        location = GPSUtils.AppPositionToGPSCoords(transform.position);
        GeoSampleLabel();
        zoneSamples = 0;
        radius = 3;

        // Update backend
        Zone = new GeosampleZone();
        Zone.zone_id = (char)('A' + (char)(NumZones++ % 27));
        Zone.radius = 3;
        Zone.location = location;
        Zone.ZoneGeosamplesIds = new();

        GeosamplingManager.SendData();  
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
        // Find Notif Game Object
        GeoSampleZoneScanned.text = zoneSamples.ToString();
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

        // todo show geosamples
        // todo update current zone and upper left
        // once zone entered find the notif + show available stats of zone
        // starred gameobject commented out until we can find the number of starred
        GeoSampleZoneNotif = GameObject.Find("ZoneGeoSampleInfo");
        GeoSampleZoneScanned = GeoSampleZoneNotif.transform.Find("SampleZoneScanned VALUE").gameObject.GetComponent<TextMeshPro>();
        GeoSampleZoneScanned.text = zoneSamples.ToString();
        // GeoSampleZoneStarred = GeoSampleZoneNotif.transform.Find("SampleZoneStarred VALUE").gameObject.GetComponent<TextMeshPro>();

        GeoSampleZoneNotif.SetActive(true);

    }

    private void OnZoneExited()
    {
        Debug.Log("Geosample Zone Exited: " + Zone.ToString());

        isEntered = false;
        CurrentZone = "";

        //todo hide current zone notification
        GeoSampleZoneNotif.SetActive(false);
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
