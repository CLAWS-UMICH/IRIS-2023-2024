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

        stationScrollHandler = stationScreen.GetComponent<ScrollHandler>();
        POIScrollHandler = POIScreen.GetComponent<ScrollHandler>();
        dangerScrollHandler = dangerScreen.GetComponent<ScrollHandler>();
        geoScrollHandler = geoScreen.GetComponent<ScrollHandler>();

        hasLocation = false;
        currentScreen = ScreenType.Station;
        parentScreen.SetActive(false);
        stationScreen.SetActive(true);
        POIScreen.SetActive(false);
        geoScreen.SetActive(false);
        parentScreen.SetActive(false);
    }



    public void OpenNavScreen()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.SelectStationWaypoint));
        hasLocation = false;
        currentScreen = ScreenType.Station;
        stationScreen.SetActive(true);
        POIScreen.SetActive(false);
        geoScreen.SetActive(false);
        parentScreen.SetActive(true);

        // TODO: Update to see ONLY station icons on map
    }

    public void CloseNavScreen()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.Menu));
        hasLocation = false;
        currentScreen = ScreenType.Station;
        parentScreen.SetActive(false);
        stationScreen.SetActive(true);
        POIScreen.SetActive(false);
        geoScreen.SetActive(false);
        parentScreen.SetActive(false);

        // TODO: Update to see ONLY station icons on map
    }


    public void OpenStation()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.SelectStationWaypoint));
        hasLocation = false;
        currentScreen = ScreenType.Station;
        POIScreen.SetActive(false);
        geoScreen.SetActive(false);
        dangerScreen.SetActive(false);
        stationScreen.SetActive(true);

        // TODO: Update to see ONLY station icons on map
    }

    public void OpenPOI()
    {
        // TODO: Update Screen changed event
        hasLocation = false;
        currentScreen = ScreenType.GeoSample;
        geoScreen.SetActive(false);
        dangerScreen.SetActive(false);
        stationScreen.SetActive(false);
        POIScreen.SetActive(true);

        // TODO: Update to see ONLY POI icons on map
    }

    public void OpenGeo()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.SelectGeoWaypoint));
        hasLocation = false;
        currentScreen = ScreenType.GeoSample;
        POIScreen.SetActive(false);
        dangerScreen.SetActive(false);
        stationScreen.SetActive(false);
        geoScreen.SetActive(true);

        // TODO: Update to see ONLY geo icons on map
    }

    public void OpenDanger()
    {
        // TODO: Update Screen changed event
        hasLocation = false;
        currentScreen = ScreenType.Danger;
        POIScreen.SetActive(false);
        geoScreen.SetActive(true);
        stationScreen.SetActive(false);
        dangerScreen.SetActive(true);

        // TODO: Update to see ONLY danger icons on map
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
                break;

            case 1:
                go = POIScrollHandler.HandleAddingButton(buttonPrefab);
                break;

            case 2:
                go = geoScrollHandler.HandleAddingButton(buttonPrefab);
                break;

            case 3:
                go = dangerScrollHandler.HandleAddingButton(buttonPrefab);
                break;

            default:
                break;
        }

        return go;
    }

    public void DeleteButton(GameObject button, int type)
    {
        switch (type)
        {
            case 0:
                stationScrollHandler.HandleButtonDeletion(button);
                break;

            case 1:
                POIScrollHandler.HandleButtonDeletion(button);
                break;

            case 2:
                geoScrollHandler.HandleButtonDeletion(button);
                break;

            case 3:
                dangerScrollHandler.HandleButtonDeletion(button);
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


}
