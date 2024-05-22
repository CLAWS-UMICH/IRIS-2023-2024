using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;


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
    public GameObject StarredIcon;
    public TextMeshPro GeoSampleIDLabel;

    public GameObject TakeXRF;
    public GameObject WaitingXRF;
    public GameObject XRFReadings;
    //public TextMeshPro[] XRFList;
    public List<TextMeshPro> XRFList = new List<TextMeshPro>();
    public GameObject XRFCollider;
    public GameObject XRFBackPlate;
    public TextMeshPro Description;
    public GameObject VegaHighlight;
    public bool XRFScanned;

    private void Start()
    {
        NameScreen.SetActive(false);
        ZoneScreen.SetActive(false);
        ColorScreen.SetActive(false);
        ShapeScreen.SetActive(false);
        VoiceNotesScreen.SetActive(false);
        WaitingXRF.SetActive(false);
        XRFReadings.SetActive(false);
        StarredIcon.SetActive(false);
        XRFScanned = false;
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
        SetStar();
        SetZoneId();
        SetSampleName("Sample " + Sample.geosample_id);
        SetGeoSampleMiniMapIcon(Sample.geosample_id);
        //StarredIcon = transform.Find("GeoSampleMiniMapIcon").transform.Find("FavoritedGeoSampleIcon").gameObject;


        if (GeosamplingZone.CurrentZone != "")
        {
            SetZone(GeosamplingZone.CurrentZone);
            Debug.Log("Automatically setting geosample zone");
        }
        else
        {
            // create a zone right now
            GeosamplingManager.CreateZone();

            IEnumerator _SetZone()
            {
                yield return new WaitForSeconds(0.1f);
                SetZone(GeosamplingZone.CurrentZone);
                Debug.Log("Automatically creating a geosample zone and setting current zone");
            }

            StartCoroutine(_SetZone());
        }

        EventBus.Publish<GeosampleCreatedEvent>(new());

        EventBus.Subscribe<GeosamplesEditedEvent>(e =>
        {
            foreach (var sample in e.EditedGeosamples)
            {
                if (sample.geosample_id == Sample.geosample_id)
                {
                    Load(sample);
                    Debug.Log("updating a geosample");
                }
            }
        });
    }
    public void Load(Geosample Sample_f)
    {
        Sample = Sample_f;

        CurrentScreen = GeoSampleScreens.None;
        SetZone(((char)('A' + (char)(Sample.zone_id++ % 27))).ToString());
        SetSampleName("Sample " + Sample.geosample_id);
        SetDescription(Sample.description);
        SetStar();
        SetZoneId();
        SetColor(Sample_f.color);
        SetShape(Sample_f.shape);

        Debug.Log("geosample loaded");
    }

    [ContextMenu("func FakeXRFScanned()")]
    public void FakeXRFScanned()
    {
        DataDetails d = new DataDetails();
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
            XRFReadings.SetActive(false);
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
        // Update XRF Readings
        XRFList[0].text = e.data.SiO2.ToString();
        XRFList[1].text = e.data.FeO.ToString();
        XRFList[2].text = e.data.CaO.ToString();
        XRFList[3].text = e.data.TiO2.ToString();
        XRFList[4].text = e.data.MnO.ToString();
        XRFList[5].text = e.data.K2O.ToString();
        XRFList[6].text = e.data.Al2O3.ToString();
        XRFList[7].text = e.data.MgO.ToString();
        XRFList[8].text = e.data.P2O3.ToString();

        // Show Readings
        XRFScanned = true;
        EventBus.Publish<PlayAudio>(new PlayAudio("XRF_Scan"));
        WaitingXRF.SetActive(false);
        XRFReadings.SetActive(true);
  
        
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

    Subscription<SpeechToText> _TranscriptionSubscription;

    public void OnStartTranscription()
    {
        if (_TranscriptionSubscription == null)
        {
            _TranscriptionSubscription = EventBus.Subscribe<SpeechToText>(OnTranscriptionFinished);
        }

        EventBus.Publish<StartTranscription>(new());
        VegaHighlight.SetActive(true);
        Debug.Log("starting geo transcription");
    }
    public void OnTranscriptionFinished(SpeechToText e)
    {
        Debug.Log("received transcription for geo: " + e.text);
        SetDescription(e.text);
        VegaHighlight.SetActive(false);

        EventBus.Unsubscribe(_TranscriptionSubscription);
        _TranscriptionSubscription = null;
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
                if (XRFScanned == true)
                {
                    XRFReadings.SetActive(false);
                    WaitingXRF.SetActive(true);
                }
                else
                {
                    TakeXRF.SetActive(true);
                }
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
    public TextMeshPro Zone_id_tmp;

    public TextMeshPro RockType_tmp;
    public TextMeshPro XRF_tmp;

    public GeosamplingShapes Shape_visual;
    public GeosamplingColor Color_visual;

    public TextMeshPro Name_tmp;

    public void SetSampleName(string name)
    {
        Name_tmp.text = name;

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
        var zone = GeosamplingZone.FindZone(letter[0]);
        if (zone != null)
        {
            zone.ZoneGeosamplesIds.Add(Sample.geosample_id);
        }

        SetZoneId();
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
        Sample.rock_type = name;
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
    public void SetShape(string shape_in)
    {
        GeosamplingShape.Shape shape = GeosamplingShape.Shape.None;
        switch (shape_in)
        {
            case "Polygon":
                shape = GeosamplingShape.Shape.Polygon;
                break;
            case "Cube":
                shape = GeosamplingShape.Shape.Cube;
                break;
            case "Cylinder":
                shape = GeosamplingShape.Shape.Cylinder;
                break;
            case "Cone":
                shape = GeosamplingShape.Shape.Cone;
                break;
            case "Sphere":
                shape = GeosamplingShape.Shape.Sphere;
                break;
            case "Crystalline":
                shape = GeosamplingShape.Shape.Crystalline;
                break;
            case "Ellipsoid":
                shape = GeosamplingShape.Shape.Ellipsoid;
                break;
            case "Irregular":
                shape = GeosamplingShape.Shape.Irregular;
                break;
            default:
                Debug.Log("no shape?");
                break;
        }

        Debug.Log("setting shape to " + shape.ToString());
        Shape_visual.SetShape(shape);

        GeosamplingManager.SendData();
        CloseCurrentScreen();
    }
    public void SetColor(string hex)
    {
        Color_visual.SetColor(hex);
        Sample.color = hex;
        GeosamplingManager.SendData();
    }
    public void SetPhoto(string jpg)
    {
        Sample.photo_jpg = jpg;
        GeosamplingManager.SendData();
    }
    public void SetDescription(string desc)
    {
        desc = desc.Trim();
        Sample.description = desc;
        Description.text = desc;
        GeosamplingManager.SendData();
    }
    public void SetNote()
    {
        // TODO update tmp

        // TODO update Sample.note
        GeosamplingManager.SendData();
    }
    public void SetStar()
    {
        StarredIcon.SetActive(Sample.starred);
    }
    public void SetZoneId()
    {
        Zone_id_tmp.text = Sample.zone_id.ToString() + Sample.geosample_id.ToString();
    }

    // -------------- Screen Visuals --------------
    private void Update()
    {
        // Rotate to user
        transform.forward = transform.position - Camera.main.transform.position;
    }
    private void SetGeoSampleMiniMapIcon(int geoSampleID)
    {
        // Create geosample zone textmeshpro
        string text = geoSampleID.ToString();
        GeoSampleIDLabel.text = text;
    }

}
