using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.IO;
using System;

public class GeosamplingDB_Manager : MonoBehaviour
{
    public static GeosamplingDB_Manager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool isOpen = false;

    private void OnEnable()
    {
        isOpen = true;
        RenderZones();
    }
    private void OnDisable()
    {
        isOpen = false;
    }

    public static void OpenScreen()
    {
        Instance.gameObject.SetActive(true);
    }
    public static void CloseScreen()
    {
        Instance.gameObject.SetActive(false);
    }
    public static void Toggle()
    {
        if (Instance.gameObject.activeSelf)
        {
            Instance.gameObject.SetActive(false);
        }
        else
        {
            Instance.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        ZoneButtons = new();
        FunctionQueue = new();
        isOpen = true;

        RenderZones();
        gameObject.SetActive(false);
        defaultPhotoMaterial = outputQuad.GetComponent<Renderer>().material;

        void _renderOnEvent<T>(T e)
        {
            if (isOpen)
            {
                RenderZones();
            }
        } 

        EventBus.Subscribe<GeosampleCreatedEvent>(_renderOnEvent);
        EventBus.Subscribe<GeosamplesEditedEvent>(_renderOnEvent);
        EventBus.Subscribe<GeosampleSentEvent>(_renderOnEvent);
    }


    // data
    public List<GameObject> ZoneButtons;
    public List<GameObject> SampleButtons;

    public GameObject ZoneParent;
    public GameObject SampleParent;

    public GameObject ZoneButtonPrefab;
    public GameObject SampleButtonPrefab;

    public GeosamplingColor colors;
    public GeosamplingShapes shapes;

    // XRF things
    public GameObject XRFReadings;
    public List<TextMeshPro> XRFList = new List<TextMeshPro>();
    public GameObject HiddenSamplesScreen;
    public GameObject outputQuad;
    Material defaultPhotoMaterial;

    // rendering
    Queue<string> FunctionQueue;
    string currZoneLetter;


    [ContextMenu("func RenderZones")]
    public void RenderZones()
    {
        FunctionQueue.Enqueue("ClearSamples");
        FunctionQueue.Enqueue("ClearZones");
        FunctionQueue.Enqueue("RenderZones");
        HideSampleDetails();
    }

    public void RenderSamples(string zoneLetter)
    {
        currZoneLetter = zoneLetter;

        FunctionQueue.Enqueue("ClearSamples");
        FunctionQueue.Enqueue("RenderSamples");
    }

    private void _RenderZones()
    {
        foreach (var zone in AstronautInstance.User.GeosampleZonesData.AllGeosampleZones)
        {
            GameObject g = Instantiate(ZoneButtonPrefab, ZoneParent.transform);
            g.GetComponent<GeosamplingDB_ZoneButton>().SetZoneLetter(zone.zone_id.ToString());
            ZoneButtons.Add(g);
        }
        ZoneParent.GetComponent<ScrollHandler>().Fix();
    }

    private void _RenderSamples()
    {
        foreach (var sample in AstronautInstance.User.GeosampleData.AllGeosamples)
        {
            if (sample.zone_id.ToString() == currZoneLetter)
            {
                GameObject g = Instantiate(SampleButtonPrefab, SampleParent.transform);
                g.GetComponent<GeosamplingDB_SampleButton>().SetSample(sample);
                SampleButtons.Add(g);
            }
        }
        SampleParent.GetComponent<ScrollHandler>().Fix();
    }

    void _ClearList(List<GameObject> ToDelete)
    {
        foreach (GameObject g in ToDelete)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(g);
            }
            else
#endif
            {
                Destroy(g);
            }
        }
        ToDelete.Clear();
    }


    private void Update()
    {
        if (FunctionQueue.Count > 0)
        {
            string operation = FunctionQueue.Dequeue();

            switch (operation)
            {
                case "ClearZones":
                    _ClearList(ZoneButtons);
                    break;
                case "RenderZones":
                    _RenderZones();
                    break;
                case "ClearSamples":
                    _ClearList(SampleButtons);
                    break;
                case "RenderSamples":
                    _RenderSamples();
                    break;
                default:
                    Debug.LogError("Nothing in function queue");
                    break;
            }
        }
    }

    public void OnZoneClicked(string ZoneLetter_f)
    {
        RenderSamples(ZoneLetter_f);
    }

    public void OnSampleClicked(Geosample g)
    {
        // SHAPE
        GeosamplingShape.Shape shapeName = GeosamplingShape.Shape.None;
        switch (g.shape) { 
            case "Shape":
                shapeName = GeosamplingShape.Shape.None;
                break;
            case "Polygon":
                shapeName = GeosamplingShape.Shape.Polygon;
                break;
            case "Cube":
                shapeName = GeosamplingShape.Shape.Cube;
                break;
            case "Cylinder":
                shapeName = GeosamplingShape.Shape.Cylinder;
                break;
            case "Cone":
                shapeName = GeosamplingShape.Shape.Cone;
                break;
            case "Sphere":
                shapeName = GeosamplingShape.Shape.Sphere;
                break;
            case "Crystalline":
                shapeName = GeosamplingShape.Shape.Crystalline;
                break;
            case "Ellipsoid":
                shapeName = GeosamplingShape.Shape.Ellipsoid;
                break;
            case "Irregular":
                shapeName = GeosamplingShape.Shape.Irregular;
                break;
            default:
                Debug.LogError("Shape error");
                break;
        }
        shapes.SetShape(shapeName);

        // COLOR
        colors.SetColor(g.color);

        // XRF
        // Update XRF Readings
        if (g.eva_data != null)
        {
            XRFList[0].text = g.eva_data.data.SiO2.ToString();
            XRFList[1].text = g.eva_data.data.FeO.ToString();
            XRFList[2].text = g.eva_data.data.CaO.ToString();
            XRFList[3].text = g.eva_data.data.TiO2.ToString();
            XRFList[4].text = g.eva_data.data.MnO.ToString();
            XRFList[5].text = g.eva_data.data.K2O.ToString();
            XRFList[6].text = g.eva_data.data.Al2O3.ToString();
            XRFList[7].text = g.eva_data.data.MgO.ToString();
            XRFList[8].text = g.eva_data.data.P2O3.ToString();
        }

        XRFReadings.SetActive(true);
        HiddenSamplesScreen.SetActive(false);

        // NOTE
        // the current rednering system for photo jpgs only works for 
        // one astronaut, not multiplayer
        if (g.photo_jpg != null && g.photo_jpg != "")
        {
            ShowPhoto(g);
        }
        else
        {
            HidePhoto();
        }
    }

    public void HideSampleDetails()
    {
        HiddenSamplesScreen.SetActive(true);
        XRFReadings.SetActive(false);
        HidePhoto();
    }

    private void ShowPhoto(Geosample g)
    {
        if (Screenshot.SamplePictures.ContainsKey(g.geosample_id))
        {
            Renderer r = outputQuad.GetComponent<Renderer>();
            r.material = Screenshot.SamplePictures[g.geosample_id];
        }
    }
    private void HidePhoto()
    {
        if (defaultPhotoMaterial != null)
        {
            outputQuad.GetComponent<Renderer>().material = defaultPhotoMaterial;
        }
    }
}
