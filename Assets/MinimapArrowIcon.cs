using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapArrowIcon : MonoBehaviour
{
    void Start()
    {
        // Geosample mode
        EventBus.Subscribe<GeosampleModeStartedEvent>(e => transform.localScale = Vector3.one * 2);
        EventBus.Subscribe<GeosampleModeEndedEvent>(e => transform.localScale = Vector3.one * 10);
    }

}
