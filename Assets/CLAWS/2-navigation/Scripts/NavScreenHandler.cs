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
    GameObject parentScreen;
    GameObject stationScreen;
    GameObject POIScreen;
    GameObject geoScreen;
    GameObject dangerScreen;

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
    private Subscription<SelectButton> selectButtonEvent;
    TextMeshPro title;

    List<string> stationLetters = new List<string>();
    List<string> poiLetters = new List<string>();
    List<string> geoLetters = new List<string>();
    List<string> dangerLetters = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        selectButtonEvent = EventBus.Subscribe<SelectButton>(onButtonSelect);

        wayController = transform.parent.Find("WaypointController").GetComponent<WaypointsController>();

        parentScreen = transform.parent.Find("NavScreen").gameObject;

        stationScreen = parentScreen.transform.Find("StationScroll").gameObject;
        POIScreen = parentScreen.transform.Find("POIScroll").gameObject;
        geoScreen = parentScreen.transform.Find("GeoScroll").gameObject;
        dangerScreen = parentScreen.transform.Find("DangerScroll").gameObject;
        title = parentScreen.transform.Find("Title").GetComponent<TextMeshPro>();

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

        // Update to see ALL icons on map
        SwitchCameraCull(-1);
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

        // Update to see ONLY station icons on map (ASK UX IF THIS SHOULD BE ALL OR ONLY STATION)
        SwitchCameraCull(23);
    }

    public void CloseNavScreen()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.Menu));
        title.text = "Station";
        hasLocation = false;
        currentScreen = ScreenType.Station;
        parentScreen.SetActive(false);
        stationScreen.SetActive(true);
        POIScreen.SetActive(false);
        geoScreen.SetActive(false);
        parentScreen.SetActive(false);

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
        Pathfind();
    }

    public void Pathfind()
    {
        if (hasLocation)
        {
            EventBus.Publish(new StartPathfinding(loc));
            hasLocation = false;
            NavScreenMode();
            CloseNavScreen();
        } else
        {
            Debug.Log("Did not select where to pathfind!");
        }
    }

    public void NavScreenMode()
    {
        GameObject.Find("AllScreens").transform.Find("MainMenu").GetComponent<MenuState>().ClickIRISNavigation();
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


}
