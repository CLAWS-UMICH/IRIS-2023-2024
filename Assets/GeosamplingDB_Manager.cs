using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        ZoneButtons = new();

        RenderZones();
    }

    // data
    public List<GameObject> ZoneButtons;
    public List<GameObject> SampleButtons;

    public GameObject ZoneParent;
    public GameObject SampleParent;

    public GameObject ZoneButtonPrefab;
    public GameObject SampleButtonPrefab;


    // rendering
    bool RenderFinished = true;
    Queue<string> FunctionQueue;
    string currZoneLetter;


    [ContextMenu("func RenderZones")]
    public void RenderZones()
    {
        RenderFinished = false;

        FunctionQueue.Enqueue("ClearZones");
        FunctionQueue.Enqueue("RenderZones");
    }

    public void RenderSamples(string zoneLetter)
    {
        RenderFinished = false;
        currZoneLetter = zoneLetter;
        Debug.Log("Rendering samples for zone " + zoneLetter);

        FunctionQueue.Enqueue("ClearSamples");
        FunctionQueue.Enqueue("RenderSamples");
    }

    private void _RenderZones()
    {
        foreach (var zone in AstronautInstance.User.GeosampleZonesData.AllGeosampleZones)
        {
            GameObject g = Instantiate(ZoneButtonPrefab, ZoneParent.transform);
            g.GetComponent<GeosamplingDB_ZoneButton>().SetZoneLetter(zone.zone_id.ToString());
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
        // sample clicked
    }
}