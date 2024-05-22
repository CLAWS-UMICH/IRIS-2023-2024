using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosamplingManager : MonoBehaviour
{
    public static GeosamplingManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        EventBus.Subscribe<GeosamplesAddedEvent>(e =>
        {
            foreach (Geosample g in e.NewAddedGeosamples)
            {
                CreateLoadedGeosample(g);
            }
        });
    }


    public static bool GeosamplingMode = false;

    public GameObject GeosamplingZonePrefab;
    public GameObject SingleGeosamplePrefab;
    

    [ContextMenu("func CreateZone")]
    public static void CreateZone()
    {
        Vector3 _spawn = Camera.main.transform.position + Camera.main.transform.forward * 0.6f;
        GameObject _zoneObject =  Instantiate(Instance.GeosamplingZonePrefab, _spawn, Quaternion.identity);
        GeosamplingZone _zone = _zoneObject.GetComponent<GeosamplingZone>();

        _zone.Init();
    }

    [ContextMenu("func CreateGeosample")]
    public static void CreateGeosample()
    {
        Vector3 _spawn = Camera.main.transform.position + Camera.main.transform.forward * 0.6f;
        _spawn = new Vector3(_spawn.x, Camera.main.transform.position.y - 0.2f, _spawn.z);
        GameObject _geosample = Instantiate(Instance.SingleGeosamplePrefab, _spawn, Quaternion.identity);
        SingleGeosampleScreen _screen = _geosample.transform.Find("Prefab_SingleGeosampleScreen").GetComponent<SingleGeosampleScreen>();

        _screen.Init();
    }
    public static void CreateLoadedGeosample(Geosample sample)
    {
        Vector3 _spawn = GPSUtils.GPSCoordsToAppPosition(sample.location);
        _spawn = new Vector3(_spawn.x, Camera.main.transform.position.y - 0.2f, _spawn.z);
        GameObject _geosample = Instantiate(Instance.SingleGeosamplePrefab, _spawn, Quaternion.identity);
        SingleGeosampleScreen _screen = _geosample.transform.Find("Prefab_SingleGeosampleScreen").GetComponent<SingleGeosampleScreen>();

        _screen.Load(sample);
    }


    public static void SendData()
    {
        GameObject.Find("Controller").GetComponent<WebsocketDataHandler>().SendGeosampleData();
        EventBus.Publish<GeosampleSentEvent>(new());
    }


    public static Geosample FindGeosample(int sample_id)
    {
        foreach (Geosample g in AstronautInstance.User.GeosampleData.AllGeosamples)
        {
            if (g.geosample_id == sample_id)
            {
                return g;
            }
        }

        Debug.LogError("Geosample Not Found");
        return null;
    }
    
    [ContextMenu("func StartGeosamplingMode")]
    public static void StartGeosamplingMode()
    {
        GeosamplingMode = true;
        EventBus.Publish<GeosampleModeStartedEvent>(new());
        EventBus.Publish<ScreenChangedEvent>(new(Screens.Geo));
        EventBus.Publish<ModeChangedEvent>(new(Modes.Sampling));

        // show only geo icons
        Instance.SwitchCameraCull(25);

        GeosamplingDB_Manager.CloseScreen();
    }

    [ContextMenu("func EndGeosamplingMode")]
    public static void EndGeosamplingMode()
    {
        GeosamplingMode = false;
        EventBus.Publish<GeosampleModeEndedEvent>(new());
        EventBus.Publish<ScreenChangedEvent>(new(Screens.Menu));

        // show all icons
        Instance.SwitchCameraCull(-1);
    }



    [SerializeField] Camera mainMapCamera;
    [SerializeField] Camera miniMapCamera;

    public void SwitchCameraCull(int num)
    {
        int mainCullingMask = mainMapCamera.cullingMask;
        int miniCullingMask = miniMapCamera.cullingMask;
        // 23: Station, 24: Nav, 25: Geo, 26: Comp
        if (num == -1)
        {
            for (int i = 23; i < 27; i++)
            {
                mainCullingMask |= (1 << i);
                miniCullingMask |= (1 << i);
                mainMapCamera.cullingMask = mainCullingMask;
                miniMapCamera.cullingMask = miniCullingMask;
            }
        }
        else
        {
            for (int i = 23; i < 27; i++)
            {
                if (num == i)
                {
                    mainCullingMask |= (1 << i);
                    miniCullingMask |= (1 << i);
                    mainMapCamera.cullingMask = mainCullingMask;
                    miniMapCamera.cullingMask = miniCullingMask;
                }
                else
                {
                    mainCullingMask &= ~(1 << i);
                    miniCullingMask &= ~(1 << i);
                    mainMapCamera.cullingMask = mainCullingMask;
                    miniMapCamera.cullingMask = miniCullingMask;
                }
            }
        }
    }


}
