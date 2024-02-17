using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosamplingManager : MonoBehaviour
{
    public static bool GeosamplingMode = false;

    public GameObject GeosamplingZonePrefab;
    public GameObject SingleGeosamplePrefab;
    

    [ContextMenu("func CreateZone")]
    public void CreateZone()
    {
        Vector3 _spawn = Camera.main.transform.position + Camera.main.transform.forward * 0.6f;
        GameObject _zoneObject =  Instantiate(GeosamplingZonePrefab, _spawn, Quaternion.identity);
        GeosamplingZone _zone = _zoneObject.GetComponent<GeosamplingZone>();

        _zone.Init();
    }

    [ContextMenu("func CreateGeosample")]
    public void CreateGeosample()
    {
        Vector3 _spawn = Camera.main.transform.position + Camera.main.transform.forward * 0.6f;
        GameObject _geosample = Instantiate(SingleGeosamplePrefab, _spawn, Quaternion.identity);
        SingleGeosampleScreen _screen = _geosample.transform.Find("Prefab_SingleGeosampleScreen").GetComponent<SingleGeosampleScreen>();

        _screen.Init();
    }


    public static void SendData()
    {
        GameObject.Find("Controller").GetComponent<WebsocketDataHandler>().SendGeosampleData();
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
