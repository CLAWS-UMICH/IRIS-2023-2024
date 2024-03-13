using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeosampleZoneNotif : MonoBehaviour
{
    private Subscription<GeosampleZoneEnteredEvent> geosampleZoneEnteredEvent;
    private Subscription<GeosampleZoneExitedEvent> geosampleZoneExitedEvent;
    private Subscription<GeosampleCreatedEvent> geosampleAddedEvent;

    public List<GameObject> OnZoneEntered_Enable;
    public List<GameObject> OnZoneEntered_Disable;
    public List<GameObject> OnZoneExited_Enable;
    public List<GameObject> OnZoneExited_Disable;

    public TextMeshPro ZoneName;
    public TextMeshPro NumScanned;
    public TextMeshPro NumStarred;

    private void Start()
    {
        geosampleZoneEnteredEvent = EventBus.Subscribe<GeosampleZoneEnteredEvent>(OnGeosampleZoneEntered);
        geosampleZoneExitedEvent = EventBus.Subscribe<GeosampleZoneExitedEvent>(OnGeosampleZoneExited);
        geosampleAddedEvent = EventBus.Subscribe<GeosampleCreatedEvent>(OnGeosampleZoneEntered);

        if (GeosamplingZone.CurrentZone == "")
        {
            OnGeosampleZoneExited("");
        }
        else
        {
            OnGeosampleZoneEntered("");
        }
    }

    IEnumerator UpdateScreen()
    {
        yield return new WaitForSeconds(0.2f);

        GeosampleZone curr = GeosamplingZone.FindZone(GeosamplingZone.CurrentZone[0]);
        if (curr != null)
        {
            ZoneName.text = "Zone " + GeosamplingZone.CurrentZone;

            NumScanned.text = curr.ZoneGeosamplesIds.Count.ToString() + " scanned";

            int num_starred = 0;
            foreach (int id in curr.ZoneGeosamplesIds)
            {
                Geosample sample = GeosamplingManager.FindGeosample(id);
                if (sample != null && sample.starred)
                {
                    num_starred++;
                }
            }
            NumStarred.text = num_starred.ToString() + " starred";
        }
    }

    private void OnGeosampleZoneEntered<T>(T _)
    {
        foreach (GameObject g in OnZoneEntered_Enable)
        {
            if (g != null)
            {
                g.SetActive(true);
            }
        }
        foreach (GameObject g in OnZoneEntered_Disable)
        {
            if (g != null)
            {
                g.SetActive(false);
            }
        }

        StartCoroutine(UpdateScreen());
    }

    private void OnGeosampleZoneExited<T>(T _)
    {
        foreach (GameObject g in OnZoneExited_Enable)
        {
            if (g != null)
            {
                g.SetActive(true);
            }
        }
        foreach (GameObject g in OnZoneExited_Disable)
        {
            if (g != null)
            {
                g.SetActive(false);
            }
        }
    }

    void OnDestroy()
    {
        if (geosampleZoneEnteredEvent != null)
        {
            EventBus.Unsubscribe(geosampleZoneEnteredEvent);
        }
        if (geosampleZoneExitedEvent != null)
        {
            EventBus.Unsubscribe(geosampleZoneExitedEvent);
        }
        if (geosampleAddedEvent != null)
        {
            EventBus.Unsubscribe(geosampleAddedEvent);
        }
    }
}

