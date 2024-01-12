using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ScreenType
{
    Station,
    Navigation,
    GeoSample
}

public class WaypointScreenHandler : MonoBehaviour
{
    GameObject parentScreen;
    GameObject stationScreen;
    GameObject navScreen;
    GameObject geoScreen;

    [SerializeField] GameObject buttonPrefab;

    ScrollHandler stationScrollHandler;
    ScrollHandler navScrollHandler;
    ScrollHandler geoScrollHandler;

    ScreenType currentScreen;

    // Start is called before the first frame update
    void Start()
    {
        parentScreen = transform.parent.Find("WaypointScreen").gameObject;
        stationScreen = parentScreen.transform.Find("StationScroll").gameObject;
        navScreen = parentScreen.transform.Find("NavScroll").gameObject;
        geoScreen = parentScreen.transform.Find("GeoScroll").gameObject;
        currentScreen = ScreenType.Station;
        CloseWaypointScreen();

        stationScrollHandler = stationScreen.GetComponent<ScrollHandler>();
        navScrollHandler = navScreen.GetComponent<ScrollHandler>();
        geoScrollHandler = geoScreen.GetComponent<ScrollHandler>();
    }

    public void OpenWaypointScreen()
    {
        currentScreen = ScreenType.Station;
        stationScreen.SetActive(true);
        navScreen.SetActive(false);
        geoScreen.SetActive(false);
        parentScreen.SetActive(true);
    }

    public void CloseWaypointScreen()
    {
        currentScreen = ScreenType.Station;
        parentScreen.SetActive(false);
        stationScreen.SetActive(true);
        navScreen.SetActive(false);
        geoScreen.SetActive(false);
    }

    public void OpenStation()
    {
        currentScreen = ScreenType.Station;
        stationScreen.SetActive(true);
        navScreen.SetActive(false);
        geoScreen.SetActive(false);
    }

    public void OpenNav()
    {
        currentScreen = ScreenType.Navigation;
        navScreen.SetActive(true);
        stationScreen.SetActive(false);
        geoScreen.SetActive(false);
    }

    public void OpenGeo()
    {
        currentScreen = ScreenType.GeoSample;
        geoScreen.SetActive(true);
        stationScreen.SetActive(false);
        navScreen.SetActive(false);
    }

    public void ScrollUp()
    {
        switch (currentScreen)
        {
            case ScreenType.Station:
                stationScrollHandler.ScrollUpOrLeft();
                break;

            case ScreenType.Navigation:
                navScrollHandler.ScrollUpOrLeft();
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
                stationScrollHandler.Fix();
                break;

            case ScreenType.Navigation:
                navScrollHandler.ScrollDownOrRight();
                break;

            case ScreenType.GeoSample:
                geoScrollHandler.ScrollDownOrRight();
                break;

            default:
                break;
        }
    }

    public GameObject AddButton(Waypoint way)
    {
        GameObject go = new GameObject();
        buttonPrefab.transform.Find("IconAndText").gameObject.transform.Find("LetterText").gameObject.GetComponent<TextMeshPro>().text = way.waypoint_letter;
        buttonPrefab.transform.Find("IconAndText").gameObject.transform.Find("Description").gameObject.GetComponent<TextMeshPro>().text = way.description;
        switch (way.type)
        {
            case 0:
                go =stationScrollHandler.HandleAddingButton(buttonPrefab);
                break;

            case 1:
                go =navScrollHandler.HandleAddingButton(buttonPrefab);
                break;

            case 2:
                go = geoScrollHandler.HandleAddingButton(buttonPrefab);
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
                navScrollHandler.HandleButtonDeletion(button);
                break;

            case 2:
                geoScrollHandler.HandleButtonDeletion(button);
                break;

            default:
                break;
        }
    }
}
