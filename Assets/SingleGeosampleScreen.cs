using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;


public class SingleGeosampleScreen : MonoBehaviour
{
    public static int id_counter = 0;

    public Geosample Sample;
    public GeoSampleScreens CurrentScreen;

    public GameObject NameScreen;
    public GameObject ZoneScreen;
    public GameObject ColorScreen;
    public GameObject ShapeScreen;
    public GameObject VoiceNotesScreen;

    public GameObject TakeXRF;
    public GameObject WaitingXRF;
    public GameObject XRFReadings;
    //public TextMeshPro[] XRFList;
    public List<TextMeshPro> XRFList = new List<TextMeshPro>();
    public GameObject XRFCollider;
    public GameObject XRFBackPlate;

    private void Start()
    {
        NameScreen.SetActive(false);
        ZoneScreen.SetActive(false);
        ColorScreen.SetActive(false);
        ShapeScreen.SetActive(false);
        VoiceNotesScreen.SetActive(false);
        WaitingXRF.SetActive(false);
        XRFReadings.SetActive(false);
        CurrentScreen = GeoSampleScreens.None;
    }
    public void Init()
    {
        Sample = new();
        Sample.author = AstronautInstance.User.id;
        AstronautInstance.User.GeosampleData.AllGeosamples.Add(Sample);

        SetID();
        SetCoordinates();
        SetTime();
        SetSampleName("Sample " + Sample.geosample_id);

        // set zone if within a zone
    }
    public void Load(Geosample Sample_f)
    {
        Sample = Sample_f;
        // Set all relevant data 

        CurrentScreen = GeoSampleScreens.None;
        SetZone(((char)('A' + (char)(Sample.zone_id++ % 27))).ToString());
        SetSampleName("Sample " + Sample.geosample_id);

        // set zone if within a zone
    }

    [ContextMenu("func FakeXRFScanned()")]
    public void FakeXRFScanned()
    {
        DataDetails d = new DataDetails();
        // todo set d.blahblahblah to whatever you want
        d.Al2O3 = 99.99;
        d.SiO2 = 2.888;
        d.FeO = 30.19;
        d.CaO = 1.0;
        EventBus.Publish<XRFScanEvent>(new(d));

        
    }

    public enum GeoSampleScreens
    {
        None,
        Name,
        Zone,
        XRFScan,
        Shape,
        Color,
        VoiceNotes,
        TakePhoto
    }

    // ----------------- Buttons -----------------
    public void OnNameButtonPressed()
    {
        if (CurrentScreen != GeoSampleScreens.Name)
        {
            CloseCurrentScreen();

            CurrentScreen = GeoSampleScreens.Name;
            NameScreen.SetActive(true);
        }
        else
        {
            CloseCurrentScreen();
        }        
    }
    public void OnZoneButtonPressed()
    {
        if (CurrentScreen != GeoSampleScreens.Zone)
        {
            CloseCurrentScreen();

            CurrentScreen = GeoSampleScreens.Zone;
            ZoneScreen.SetActive(true);
        }
        else
        {
            CloseCurrentScreen();
        }
    }
    public void OnXRFButtonPressed()
    {
        if (CurrentScreen != GeoSampleScreens.XRFScan)
        {
            CloseCurrentScreen();

            CurrentScreen = GeoSampleScreens.XRFScan;
            TakeXRF.SetActive(false);

            // Wait for the event
            WaitingXRF.SetActive(true);
            EventBus.Subscribe<XRFScanEvent>(waitingForXRF);
        }
        else
        {
            CloseCurrentScreen();
        }
    }

    public void waitingForXRF(XRFScanEvent e)
    {
        XRFList[0].text = e.data.SiO2.ToString();
        XRFList[1].text = e.data.FeO.ToString();
        XRFList[2].text = e.data.CaO.ToString();
        XRFList[3].text = e.data.TiO2.ToString();
        XRFList[4].text = e.data.MnO.ToString();
        XRFList[5].text = e.data.K2O.ToString();
        XRFList[6].text = e.data.Al2O3.ToString();
        XRFList[7].text = e.data.MgO.ToString();
        XRFList[8].text = e.data.P2O3.ToString();

        WaitingXRF.SetActive(false);
        XRFReadings.SetActive(true);
        XRFBackPlate.SetActive(false);
        XRFCollider.SetActive(false);

        
        // Update XRF Readings
    }

    public void OnShapeButtonPressed()
    {
        if (CurrentScreen != GeoSampleScreens.Shape)
        {
            CloseCurrentScreen();
            CurrentScreen = GeoSampleScreens.Shape;
            ShapeScreen.SetActive(true);
        }
        else
        {
            CloseCurrentScreen();
        }
    }
    public void OnColorButtonPressed()
    {
        if (CurrentScreen != GeoSampleScreens.Color)
        {
            CloseCurrentScreen();
            CurrentScreen = GeoSampleScreens.Color;
            ColorScreen.SetActive(true);
        }
        else
        {
            CloseCurrentScreen();
        }        
    }
    [SerializeField] GameObject PhotoScreen;
    public void OnPhotoButtonPressed()
    {
        if (CurrentScreen != GeoSampleScreens.TakePhoto)
        {
            CloseCurrentScreen();

            CurrentScreen = GeoSampleScreens.TakePhoto;
            PhotoScreen.SetActive(true);
        }
        else { 
            CloseCurrentScreen();
            PhotoScreen.SetActive(false);
        }
    }
    public void OnVEGAButtonPressed()
    {
        if (CurrentScreen != GeoSampleScreens.VoiceNotes)
        {
            CloseCurrentScreen();

            CurrentScreen = GeoSampleScreens.VoiceNotes;
            VoiceNotesScreen.SetActive(true);
        }
        else
        {
            CloseCurrentScreen();
        }
    }
    public void CloseCurrentScreen()
    {
        switch (CurrentScreen)
        {
            case GeoSampleScreens.None:
                break;
            case GeoSampleScreens.Name:
                NameScreen.SetActive(false);
                break;
            case GeoSampleScreens.Zone:
                ZoneScreen.SetActive(false);
                break;
            case GeoSampleScreens.XRFScan:
                WaitingXRF.SetActive(false);
                TakeXRF.SetActive(true);
                break;
            case GeoSampleScreens.Shape:
                ShapeScreen.SetActive(false);
                break;
            case GeoSampleScreens.Color:
                ColorScreen.SetActive(false);
                break;
            case GeoSampleScreens.VoiceNotes:
                VoiceNotesScreen.SetActive(false);
                break;
            case GeoSampleScreens.TakePhoto:
                // PhotoScreen.SetActive(false);
                break;
        }
        CurrentScreen = GeoSampleScreens.None;
    }


    // -------------- Setter Methods --------------
    public TextMeshPro Zone_tmp;
    public TextMeshPro ZoneLetter_tmp;
    public TextMeshPro ZoneNone_tmp;
    public GameObject Zone_icon;
    public TextMeshPro OtherZone_tmp;

    public TextMeshPro RockType_tmp;
    public TextMeshPro XRF_tmp;

    public GeosamplingShapes Shape_visual;
    public GeosamplingColor Color_visual;

    public TextMeshPro Name_tmp;

    public void SetSampleName(string name)
    {
        Name_tmp.text = name;

        // TODO update Sample.name
        GeosamplingManager.SendData();
    }
    public void SetID()
    {
        Sample.geosample_id = id_counter++;
    }
    public void SetZone(string letter)
    {
        letter = letter.Trim();
        ZoneLetter_tmp.gameObject.SetActive(true);
        ZoneLetter_tmp.text = letter;
        ZoneNone_tmp.text = "";
        Zone_icon.SetActive(true);
        OtherZone_tmp.gameObject.SetActive(true);
        OtherZone_tmp.text = "Zone " + letter;

        Sample.zone_id = letter[0];
        GeosamplingManager.SendData();
    }
    public void SetCoordinates()
    {
        // also automatic assignment
        // called on start function
        Sample.location = GPSUtils.AppPositionToGPSCoords(Camera.main.transform.position + (Camera.main.transform.forward * 0.5f));
    }
    public void SetTime()
    {
        // also automatic assignment
        // called on start function
        Sample.time = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
    }
    public void SetRockType(string name)
    {
        // automatic assignment
        // called after xrf scan
        RockType_tmp.text = name;

        // TODO update Sample.rockType
        GeosamplingManager.SendData();
    }
    [SerializeField]
    public void SetShape(GeosamplingShape shape_in)
    {
        Shape_visual.SetShape(shape_in.shape);

        switch (shape_in.shape)
        {
            case GeosamplingShape.Shape.None:
                Sample.shape = "Shape";
                break;
            case GeosamplingShape.Shape.Polygon:
                Sample.shape = "Polygon";
                break;
            case GeosamplingShape.Shape.Cube:
                Sample.shape = "Cube";
                break;
            case GeosamplingShape.Shape.Cylinder:
                Sample.shape = "Cylinder";
                break;
            case GeosamplingShape.Shape.Cone:
                Sample.shape = "Cone";
                break;
            case GeosamplingShape.Shape.Sphere:
                Sample.shape = "Sphere";
                break;
            case GeosamplingShape.Shape.Crystalline:
                Sample.shape = "Crystalline";
                break;
            case GeosamplingShape.Shape.Ellipsoid:
                Sample.shape = "Ellipsoid";
                break;
            case GeosamplingShape.Shape.Irregular:
                Sample.shape = "Irregular";
                break;
            default:
                Debug.LogError("Shape error");
                break;
        }

        GeosamplingManager.SendData();
        CloseCurrentScreen();
    }
    public void SetColor(string hex)
    {
        Color_visual.SetColor(hex);
        Sample.color = hex;
        GeosamplingManager.SendData();
    }
    public void SetPhoto()
    {
        // TODO show photo

        // TODO update Sample.photo
        GeosamplingManager.SendData();
    }
    public void SetNote()
    {
        // TODO update tmp

        // TODO update Sample.note
        GeosamplingManager.SendData();
    }


    // -------------- Screen Visuals --------------
    private void Update()
    {
        // Rotate to user
        transform.forward = transform.position - Camera.main.transform.position;
    }
    
}
