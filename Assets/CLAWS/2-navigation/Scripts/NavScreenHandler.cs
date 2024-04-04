using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public enum ScreenType
{
    Station,
    POI,
    GeoSample,
    Danger
}

public class NavScreenHandler : MonoBehaviour
{
    GameObject player;
    GameObject parentScreen;
    GameObject stationScreen;
    GameObject POIScreen;
    GameObject geoScreen;
    GameObject dangerScreen;
    GameObject pathfindingScreen;
    public GameObject cancelRouteBtn;

    GameObject confirmationScreen;
    private GameObject BatteryOld;
    private GameObject BatteryNew;

    private GameObject OxygenOld;
    private GameObject OxygenNew;


    Camera mainMapCamera;
    Camera miniMapCamera;

    Location loc;
    bool hasLocation;

    [SerializeField] GameObject buttonPrefab;

    ScrollHandler stationScrollHandler;
    ScrollHandler POIScrollHandler;
    ScrollHandler geoScrollHandler;
    ScrollHandler dangerScrollHandler;

    [SerializeField] ScreenType currentScreen;

    WaypointsController wayController;
    CreateWaypoint wayCreate;
    private Subscription<SelectButton> selectButtonEvent;
    TextMeshPro title;
    Pathfinding pf;

    TextMeshPro confirmation_title, confirmation_time, confirmation_dist, confirmation_bat_depletion, confirmation_oxy_depletion;

    List<string> stationLetters = new List<string>();
    List<string> poiLetters = new List<string>();
    List<string> geoLetters = new List<string>();
    List<string> dangerLetters = new List<string>();

    private float BATT_TIME_CAP = 21600;
    private float OXY_TIME_CAP = 10800;
    private float astronautWalkingSpeed = 55;

    private Vector3 destinationVector;
    // Start is called before the first frame update

    private Subscription<ModeChangedEvent> modeChangedSubscription;
    void Start()
    {
        player = GameObject.Find("Main Camera").gameObject;
        selectButtonEvent = EventBus.Subscribe<SelectButton>(onButtonSelect);

        wayController = transform.parent.Find("WaypointController").GetComponent<WaypointsController>();
        wayCreate = transform.parent.Find("WaypointController").GetComponent<CreateWaypoint>();
        pf = transform.GetComponent<Pathfinding>();

        parentScreen = transform.parent.Find("NavScreen").gameObject;

        stationScreen = parentScreen.transform.Find("StationScroll").gameObject;
        POIScreen = parentScreen.transform.Find("POIScroll").gameObject;
        geoScreen = parentScreen.transform.Find("GeoScroll").gameObject;
        dangerScreen = parentScreen.transform.Find("DangerScroll").gameObject;
        title = parentScreen.transform.Find("Title").GetComponent<TextMeshPro>();
        confirmationScreen = transform.parent.Find("NavConfirmation").gameObject;


        //pathfindingScreen = transform.parent.Find("PathfindingScreen").gameObject;

        mainMapCamera = GameObject.Find("MainMapCamera").GetComponent<Camera>();
        miniMapCamera = GameObject.Find("MinimapCamera").GetComponent<Camera>();

        stationScrollHandler = stationScreen.GetComponent<ScrollHandler>();
        POIScrollHandler = POIScreen.GetComponent<ScrollHandler>();
        dangerScrollHandler = dangerScreen.GetComponent<ScrollHandler>();
        geoScrollHandler = geoScreen.GetComponent<ScrollHandler>();

        title.text = "Station";
        hasLocation = false;
        currentScreen = ScreenType.Station;
        parentScreen.SetActive(false);
        stationScreen.SetActive(true);
        POIScreen.SetActive(false);
        geoScreen.SetActive(false);
        parentScreen.SetActive(false);
        confirmationScreen.SetActive(false);

        confirmation_title = confirmationScreen.transform.Find("Title").GetComponent<TextMeshPro>();
        confirmation_time = confirmationScreen.transform.Find("Info").Find("TimeMsg").GetComponent<TextMeshPro>();
        confirmation_dist = confirmationScreen.transform.Find("Info").Find("DistMsg").GetComponent<TextMeshPro>();
        confirmation_bat_depletion = confirmationScreen.transform.Find("Info").Find("BatteryMsg").GetComponent<TextMeshPro>();
        confirmation_oxy_depletion = confirmationScreen.transform.Find("Info").Find("OxyMsg").GetComponent<TextMeshPro>();
        BatteryOld = confirmationScreen.transform.Find("battery_bar").Find("BatteryPBOld").gameObject;
        BatteryNew = confirmationScreen.transform.Find("battery_bar").Find("BatteryPBNew").gameObject;

        OxygenOld = confirmationScreen.transform.Find("oxygen_bar").Find("OxygenPBOld").gameObject;
        OxygenNew = confirmationScreen.transform.Find("oxygen_bar").Find("OxygenPBNew").gameObject;
        //CloseScreenStart();

        // Update to see ALL icons on map
        SwitchCameraCull(-1);

        modeChangedSubscription = EventBus.Subscribe<ModeChangedEvent>(SwitchMode);
    }

    public void CloseScreenStart()
    {
        gameObject.SetActive(false);
    }

    private void SwitchMode(ModeChangedEvent e)
    {
        if (e.Mode != Modes.Navigation)
        {
            CancelPathfindConfirmation();
        }
    }

    public void OpenNavScreen()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.SelectStationNav));
        title.text = "Station";
        hasLocation = false;
        currentScreen = ScreenType.Station;
        stationScreen.SetActive(true);
        POIScreen.SetActive(false);
        geoScreen.SetActive(false);
        parentScreen.SetActive(true);
        confirmationScreen.SetActive(false);

        // Update to see ONLY station icons on map (ASK UX IF THIS SHOULD BE ALL OR ONLY STATION)
        SwitchCameraCull(23);
    }

    public void CloseNavScreen()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.Menu));
        title.text = "Station";
        hasLocation = false;
        currentScreen = ScreenType.Station;
        stationScreen.SetActive(true);
        POIScreen.SetActive(false);
        geoScreen.SetActive(false);
        parentScreen.SetActive(false);
        confirmationScreen.SetActive(false);

        // Update to see ALL icons on map
        SwitchCameraCull(-1);
    }


    public void OpenStation()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.SelectStationNav));
        title.text = "Station";
        hasLocation = false;
        currentScreen = ScreenType.Station;
        POIScreen.SetActive(false);
        geoScreen.SetActive(false);
        dangerScreen.SetActive(false);
        stationScreen.SetActive(true);
        confirmationScreen.SetActive(false);

        // Update to see ONLY station icons on map
        SwitchCameraCull(23);
    }

    public void OpenPOI()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.SelectPOINav));
        title.text = "Points of Interest";
        hasLocation = false;
        currentScreen = ScreenType.GeoSample;
        geoScreen.SetActive(false);
        dangerScreen.SetActive(false);
        stationScreen.SetActive(false);
        confirmationScreen.SetActive(false);
        POIScreen.SetActive(true);

        // Update to see ONLY POI icons on map
        SwitchCameraCull(24);
    }

    public void OpenGeo()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.SelectGeoNav));
        title.text = "Geosamples";
        hasLocation = false;
        currentScreen = ScreenType.GeoSample;
        POIScreen.SetActive(false);
        dangerScreen.SetActive(false);
        stationScreen.SetActive(false);
        confirmationScreen.SetActive(false);
        geoScreen.SetActive(true);

        // Update to see ONLY geo icons on map
        SwitchCameraCull(25);
    }

    public void OpenDanger()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.SelectDangerNav));
        title.text = "Danger";
        hasLocation = false;
        currentScreen = ScreenType.Danger;
        POIScreen.SetActive(false);
        geoScreen.SetActive(false);
        stationScreen.SetActive(false);
        confirmationScreen.SetActive(false);
        dangerScreen.SetActive(true);

        // Update to see ONLY danger icons on map
        SwitchCameraCull(26);
    }

    public void ScrollUp()
    {
        switch (currentScreen)
        {
            case ScreenType.Station:
                stationScrollHandler.ScrollUpOrLeft();
                break;

            case ScreenType.POI:
                POIScrollHandler.ScrollUpOrLeft();
                break;

            case ScreenType.Danger:
                dangerScrollHandler.ScrollUpOrLeft();
                break;

            case ScreenType.GeoSample:
                geoScrollHandler.ScrollUpOrLeft();
                break;

            default:
                break;
        }
    }

    public void ScrollDown()
    {
        switch (currentScreen)
        {
            case ScreenType.Station:
                stationScrollHandler.ScrollDownOrRight();
                break;

            case ScreenType.POI:
                POIScrollHandler.ScrollDownOrRight();
                break;

            case ScreenType.Danger:
                dangerScrollHandler.ScrollDownOrRight();
                break;

            case ScreenType.GeoSample:
                geoScrollHandler.ScrollDownOrRight();
                break;

            default:
                break;
        }
    }

    // 0 = station, 1 = nav, 2 = geo, 3 = danger
    public GameObject AddButton(Waypoint way)
    {
        GameObject go = new GameObject();
        buttonPrefab.transform.Find("IconAndText").gameObject.transform.Find("LetterText").gameObject.GetComponent<TextMeshPro>().text = way.waypoint_letter;
        buttonPrefab.transform.Find("IconAndText").gameObject.transform.Find("Description").gameObject.GetComponent<TextMeshPro>().text = way.description;
        switch (way.type)
        {
            case 0:
                go = stationScrollHandler.HandleAddingButton(buttonPrefab);
                stationLetters.Add(way.waypoint_letter);
                break;

            case 1:
                go = POIScrollHandler.HandleAddingButton(buttonPrefab);
                poiLetters.Add(way.waypoint_letter);
                break;

            case 2:
                go = geoScrollHandler.HandleAddingButton(buttonPrefab);
                geoLetters.Add(way.waypoint_letter);
                break;

            case 3:
                go = dangerScrollHandler.HandleAddingButton(buttonPrefab);
                dangerLetters.Add(way.waypoint_letter);
                break;

            default:
                break;
        }

        return go;
    }

    public void DeleteButton(GameObject button, int type)
    {
        string letter = button.transform.Find("IconAndText").gameObject.transform.Find("LetterText").gameObject.GetComponent<TextMeshPro>().text;
        switch (type)
        {
            case 0:
                stationScrollHandler.HandleButtonDeletion(button);
                stationLetters.Remove(letter);
                break;

            case 1:
                POIScrollHandler.HandleButtonDeletion(button);
                poiLetters.Remove(letter);
                break;

            case 2:
                geoScrollHandler.HandleButtonDeletion(button);
                geoLetters.Remove(letter);
                break;

            case 3:
                dangerScrollHandler.HandleButtonDeletion(button);
                dangerLetters.Remove(letter);
                break;

            default:
                break;
        }
    }

    public void onButtonSelect(SelectButton e)
    {
        hasLocation = true;
        loc = wayController.getLocationOfButton(e.letter);
        InitializePathfind(e.letter);


    }

    private void EditConfirmationDetails(string letter)
    {
        double distance = pf.getTotalDistance();
        double totalTime = distance / astronautWalkingSpeed;

        /*double initialBatTime = AstronautInstance.User.VitalsData.batt_time_left;
        double initialOxyTime = AstronautInstance.User.VitalsData.oxy_time_left;
        double initialBatPerc = AstronautInstance.User.VitalsData.batt_percentage;
        double initialOxyPerc = AstronautInstance.User.VitalsData.oxy_percentage;*/
        double initialBatTime = 21000;
        double initialOxyTime = 10000;
        double initialBatPerc = initialBatTime / BATT_TIME_CAP * 100;
        double initialOxyPerc = initialOxyTime / OXY_TIME_CAP * 100;

        double newBatTime = initialBatTime - (totalTime * 60);
        double newOxyTime = 10000 - (totalTime * 60);
        double newBatPerc = newBatTime / BATT_TIME_CAP * 100;
        double newOxyPerc = newOxyTime / OXY_TIME_CAP * 100;

        confirmation_title.text = "Navigate to Point: " + letter;

        // TODO: Update values (make them up)
        confirmation_dist.text = distance.ToString("0") + "m";

        string formattedTime;
        if (totalTime < 1.0)
        {
            // If less than 1 minute, convert to seconds with no decimal places
            double totalSeconds = totalTime * 60;
            formattedTime = totalSeconds.ToString("0") + "s";
        }
        else
        {
            // If 1 minute or more, display the time in minutes with no decimal places
            formattedTime = totalTime.ToString("0") + "min";
        }
        confirmation_time.text = formattedTime;

        // TODO: Update progress bars
        // TODO: Update progress bars text values
        string batInitialPercentage = initialBatPerc.ToString("0") + "%";
        string batNewPercentage = newBatPerc.ToString("0") + "%";
        confirmation_bat_depletion.text = $"Battery Depletion: ({batInitialPercentage} to {batNewPercentage})";
        BatteryOld.GetComponent<progress_bar_nav>().Update_Progress_bar((float)initialBatPerc);
        BatteryNew.GetComponent<progress_bar_nav>().Update_Progress_bar((float)newBatPerc);


        string oxyInitialPercentage = initialOxyPerc.ToString("0") + "%";
        string oxyNewPercentage = newOxyPerc.ToString("0") + "%";
        confirmation_oxy_depletion.text = $"Oxygen Depletion: ({oxyInitialPercentage} to {oxyNewPercentage})";
        OxygenOld.GetComponent<progress_bar_nav>().Update_Progress_bar((float)initialOxyPerc);
        OxygenNew.GetComponent<progress_bar_nav>().Update_Progress_bar((float)newOxyPerc);
    }

    public void InitializePathfind(string letter)
    {
        if (hasLocation)
        {
            EventBus.Publish(new ScreenChangedEvent(Screens.NavConfirmation));
            EventBus.Publish(new StartPathfinding(loc));
            destinationVector = GPSUtils.GPSCoordsToAppPosition(loc);
  
            hasLocation = false;

            // Edit confirmation screen details
            EditConfirmationDetails(letter);

            // Show confirmation screen
            parentScreen.SetActive(false);
            confirmationScreen.SetActive(true);

        }
        else
        {
            Debug.Log("Did not select where to pathfind!");
        }
    }

    public void ConfirmPathFind()
    {
        NavScreenMode();
        CloseNavScreen();
        AstronautInstance.User.currently_navigating = true;
        StartCoroutine(_CheckDistance());
    }

    IEnumerator _CheckDistance()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            float distance = Vector3.Distance(destinationVector, player.transform.position);
            if (distance <= 2)
            {
                // Cancel Navigation
                ExitNavScreenMode();

                // TODO: Maybe tell web we got to the destination

                yield break;
            }
        }
    }

    public void CancelPathfindConfirmation()
    {
        // delete breadcrumbs
        pf.destroyCurrentBreadCrumbs();

        CloseNavScreen();
        AstronautInstance.User.currently_navigating = false;
    }

    public void ClosePathfinding()
    {
        pf.destroyCurrentBreadCrumbs();
        pathfindingScreen.SetActive(false);
        OpenNavScreen();
        AstronautInstance.User.currently_navigating = false;
    }

    public void CancelRoute()
    {
        AstronautInstance.User.currently_navigating = false;
        ExitNavScreenMode();
    }

    public void NavScreenMode()
    {
        EventBus.Publish(new ModeChangedEvent(Modes.Navigation));
    }

    public void ExitNavScreenMode()
    {
        CancelPathfindConfirmation();
        EventBus.Publish(new ModeChangedEvent(Modes.Normal));
    }

    public void SwitchCameraCull(int num)
    {
        int mainCullingMask = mainMapCamera.cullingMask;
        int miniCullingMask = miniMapCamera.cullingMask;
        // 23: Station, 24: Nav, 25: Geo, 26: Danger
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
                } else
                {
                    mainCullingMask &= ~(1 << i);
                    miniCullingMask &= ~(1 << i);
                    mainMapCamera.cullingMask = mainCullingMask;
                    miniMapCamera.cullingMask = miniCullingMask;
                }
            }
        }
    }

    public void openWaypointCreation()
    {
        CloseNavScreen();
        wayCreate.OpenCreateWaypointScreen();
    }

}
