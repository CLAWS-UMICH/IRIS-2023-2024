using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosamplingVisibilityManager : MonoBehaviour
{
    private Subscription<GeosampleModeStartedEvent> geosampleModeStartedEvent;
    private Subscription<GeosampleModeEndedEvent> geosampleModeEndedEvent;

    public List<GameObject> OnModeStart_Enable;
    public List<GameObject> OnModeStart_Disable;
    public List<GameObject> OnModeEnd_Enable;
    public List<GameObject> OnModeEnd_Disable;

    private void Start()
    {
        geosampleModeStartedEvent = EventBus.Subscribe<GeosampleModeStartedEvent>(OnGeosampleModeStarted);
        geosampleModeEndedEvent = EventBus.Subscribe<GeosampleModeEndedEvent>(OnGeosampleModeEnded);

        if (GeosamplingManager.GeosamplingMode == true)
        {
            OnGeosampleModeStarted(new());
        }
        else
        {
            OnGeosampleModeEnded(new());
        }
    }

    private void OnGeosampleModeStarted(GeosampleModeStartedEvent e)
    {
        foreach (GameObject g in OnModeStart_Enable)
        {
            if (g != null)
            {
                g.SetActive(true);
            }
        }
        foreach (GameObject g in OnModeStart_Disable)
        {
            if (g != null)
            {
                g.SetActive(false);
            }
        }
    }

    private void OnGeosampleModeEnded(GeosampleModeEndedEvent e)
    {
        foreach (GameObject g in OnModeEnd_Enable)
        {
            if (g != null)
            {
                g.SetActive(true);
            }
        }
        foreach (GameObject g in OnModeEnd_Disable)
        {
            if (g != null)
            {
                g.SetActive(false);
            }
        }
    }


    void OnDestroy()
    {
        if (geosampleModeStartedEvent != null)
        {
            EventBus.Unsubscribe(geosampleModeStartedEvent);
        }
        if (geosampleModeEndedEvent != null)
        {
            EventBus.Unsubscribe(geosampleModeEndedEvent);
        }
    }
}
