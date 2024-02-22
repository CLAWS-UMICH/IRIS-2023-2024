using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosampleVisibilityManager_Zone : MonoBehaviour
{
    private Subscription<GeosampleZoneEnteredEvent> geosampleZoneEnteredEvent;
    private Subscription<GeosampleZoneExitedEvent> geosampleZoneExitedEvent;

    public List<GameObject> OnZoneEntered_Enable;
    public List<GameObject> OnZoneEntered_Disable;
    public List<GameObject> OnZoneExited_Enable;
    public List<GameObject> OnZoneExited_Disable;

    public SingleGeosampleScreen Geo;

    private void Start()
    {
        geosampleZoneEnteredEvent = EventBus.Subscribe<GeosampleZoneEnteredEvent>(OnGeosampleZoneEntered);
        geosampleZoneExitedEvent = EventBus.Subscribe<GeosampleZoneExitedEvent>(OnGeosampleZoneExited);

        if (GeosamplingZone.CurrentZone == "")
        {
            OnGeosampleZoneExited("");
        }
        else
        {
            OnGeosampleZoneEntered(new(GeosamplingZone.CurrentZone));
        }
    }

    private void OnGeosampleZoneEntered(GeosampleZoneEnteredEvent e)
    {
        if (Geo.Sample.zone_id == e.Zone[0])
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
        }
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
}
