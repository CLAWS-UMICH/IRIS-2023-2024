using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosamplingManager : MonoBehaviour
{
    public static bool GeosamplingMode = false;

    public GameObject GeosamplingZonePrefab;
    

    [ContextMenu("func CreateZone")]
    public void CreateZone()
    {
        // create the zone prefab
        GameObject _zoneObject =  Instantiate(GeosamplingZonePrefab);
        GeosamplingZone _zone = _zoneObject.GetComponent<GeosamplingZone>();

        _zone.Init();
    }




    [ContextMenu("func StartGeosamplingMode")]
    public static void StartGeosamplingMode()
    {
        GeosamplingMode = true;
        EventBus.Publish<GeosampleModeStartedEvent>(new());
    }

    [ContextMenu("func EndGeosamplingMode")]
    public static void EndGeosamplingMode()
    {
        GeosamplingMode = false;
        EventBus.Publish<GeosampleModeEndedEvent>(new());
    }

}
