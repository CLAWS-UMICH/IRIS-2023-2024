using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosamplingModeButton : MonoBehaviour
{
    public List<GameObject> VisibleObjects;

    void Start()
    {
        EventBus.Subscribe<GeosampleModeStartedEvent>(_ =>
        {
            SetVisibility(false);
        });
        EventBus.Subscribe<GeosampleModeEndedEvent>(_ =>
        {
            SetVisibility(true);
        });

        // initial state
        if (GeosamplingManager.GeosamplingMode == false)
        {
            SetVisibility(true);
        }
        else
        {
            SetVisibility(false);
        }
    }

    private void OnEnable()
    {
        // initial state
        if (GeosamplingManager.GeosamplingMode == false)
        {
            SetVisibility(true);
        }
        else
        {
            SetVisibility(false);
        }
    }

    private void SetVisibility(bool visibilty)
    {
        foreach (GameObject g in VisibleObjects)
        {
            g.SetActive(visibilty);
        }
    }

}
