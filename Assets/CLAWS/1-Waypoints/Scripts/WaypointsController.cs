using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaypointsController : MonoBehaviour
{
    [SerializeField] private GameObject dangerPrefab;
    [SerializeField] private GameObject geoPrefab;
    [SerializeField] private GameObject stationPrefab;
    [SerializeField] private GameObject navPrefab;



    private Subscription<WaypointsAddedEvent> waypointsAddedEvent;
    private Subscription<WaypointsDeletedEvent> waypointsDeletedEvent;
    private Subscription<WaypointsEditedEvent> waypointsEditedEvent; 
    private Subscription<WaypointToDelete> waypointToDeleteEvent;
    private Subscription<WaypointToAdd> waypointToAdd;

    private Dictionary<int, Waypoint> waypointDict = new Dictionary<int, Waypoint>();
    private Dictionary<int, GameObject> waypointObjDic = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> waypointButtonDic = new Dictionary<int, GameObject>();

    private TextMeshPro _danger_title, _danger_letter, _geo_title, _geo_letter, _nav_title, _nav_letter, _station_title, _station_letter;

    private WebsocketDataHandler wd;
    private NavScreenHandler screenHandler;

    // Start is called before the first frame update
    void Start()
    {
        wd = GameObject.Find("Controller").GetComponent<WebsocketDataHandler>();

        _danger_title = dangerPrefab.transform.Find("Title").gameObject.transform.Find("IconAndText").gameObject.transform.Find("TextMeshPro").gameObject.GetComponent<TextMeshPro>();
        _danger_letter = dangerPrefab.transform.Find("Quad").gameObject.transform.Find("Text").gameObject.transform.Find("TextMeshPro").gameObject.GetComponent<TextMeshPro>();

        _geo_title = geoPrefab.transform.Find("Title").gameObject.transform.Find("IconAndText").gameObject.transform.Find("TextMeshPro").gameObject.GetComponent<TextMeshPro>();
        _geo_letter = geoPrefab.transform.Find("Quad").gameObject.transform.Find("Text").gameObject.transform.Find("TextMeshPro").gameObject.GetComponent<TextMeshPro>();

        _nav_title = navPrefab.transform.Find("Title").gameObject.transform.Find("IconAndText").gameObject.transform.Find("TextMeshPro").gameObject.GetComponent<TextMeshPro>();
        _nav_letter = navPrefab.transform.Find("Quad").gameObject.transform.Find("Text").gameObject.transform.Find("TextMeshPro").gameObject.GetComponent<TextMeshPro>();

        _station_title = stationPrefab.transform.Find("Title").gameObject.transform.Find("IconAndText").gameObject.transform.Find("TextMeshPro").gameObject.GetComponent<TextMeshPro>();
        _station_letter = stationPrefab.transform.Find("Quad").gameObject.transform.Find("Text").gameObject.transform.Find("TextMeshPro").gameObject.GetComponent<TextMeshPro>();

        waypointsAddedEvent = EventBus.Subscribe<WaypointsAddedEvent>(onWaypointsAdded);
        waypointsDeletedEvent = EventBus.Subscribe<WaypointsDeletedEvent>(onWaypointsDeleted);
        waypointsEditedEvent = EventBus.Subscribe<WaypointsEditedEvent>(onWaypointsEdited);
        waypointToDeleteEvent = EventBus.Subscribe<WaypointToDelete>(onWaypointToDelete);
        waypointToAdd = EventBus.Subscribe<WaypointToAdd>(onWaypointToAdd);

        screenHandler = transform.parent.Find("NavController").gameObject.GetComponent<NavScreenHandler>();
    }

    public void onWaypointsAdded(WaypointsAddedEvent e)
    {
        List<Waypoint> addedWaypoints = e.NewAddedWaypoints;
        foreach(Waypoint waypoint in addedWaypoints)
        {
            waypointDict[waypoint.waypoint_id] = waypoint;
            AstronautInstance.User.WaypointData.AllWaypoints.Add(waypoint);
            SpawnWaypoint(waypoint);

            GameObject button = screenHandler.AddButton(waypoint);
            waypointButtonDic[waypoint.waypoint_id] = button;
        }
    }
    public void onWaypointToAdd(WaypointToAdd e)
    {
        CreateNew(e.location, e.type, e.description);
    }

    public void onWaypointsDeleted(WaypointsDeletedEvent e)
    {
        List<Waypoint> deletedWaypoints = e.DeletedWaypoints;
        foreach(Waypoint waypoint in deletedWaypoints)
        {
            if (waypointDict.ContainsKey(waypoint.waypoint_id))
            {
                waypointDict.Remove(waypoint.waypoint_id);

                GameObject gm = waypointObjDic[waypoint.waypoint_id];
                waypointObjDic.Remove(waypoint.waypoint_id);

                GameObject gmButton = waypointButtonDic[waypoint.waypoint_id];
                screenHandler.DeleteButton(gmButton, waypoint.type);
                waypointButtonDic.Remove(waypoint.waypoint_id);

                Destroy(gm);
            }
            
            AstronautInstance.User.WaypointData.AllWaypoints.Remove(waypoint);
        }
    }

    public void onWaypointToDelete(WaypointToDelete e)
    {
        int indexToDelete = e.waypointIndexToDelete;

        if (waypointDict.ContainsKey(indexToDelete))
        {
            Waypoint w = waypointDict[indexToDelete];
            waypointDict.Remove(indexToDelete);

            GameObject gm = waypointObjDic[indexToDelete];
            waypointObjDic.Remove(indexToDelete);

            GameObject gmButton = waypointButtonDic[indexToDelete];
            screenHandler.DeleteButton(gmButton, w.type);
            waypointButtonDic.Remove(indexToDelete);

            Destroy(gm);
            AstronautInstance.User.WaypointData.AllWaypoints.Remove(w);

            wd.SendWaypointData();
        }
    }

    public void onWaypointsEdited(WaypointsEditedEvent e)
    {
        List<Waypoint> editedWaypoints = e.EditedWaypoints;
        foreach(Waypoint waypoint in editedWaypoints)
        {
            if (waypointDict.ContainsKey(waypoint.waypoint_id))
            {
                waypointDict[waypoint.waypoint_id].location = waypoint.location;
                waypointDict[waypoint.waypoint_id].type = waypoint.type;
                waypointDict[waypoint.waypoint_id].description = waypoint.description;

                Waypoint newWaypoint = new Waypoint
                {
                    waypoint_id = waypoint.waypoint_id,
                    location = waypoint.location,
                    type = waypoint.type,
                    description = waypoint.description,
                    author = waypoint.author
                };

                AstronautInstance.User.WaypointData.AllWaypoints.Remove(waypoint);
                AstronautInstance.User.WaypointData.AllWaypoints.Add(newWaypoint);

                waypointObjDic[waypoint.waypoint_id].transform.Find("Location").gameObject.transform.Find("Lat").gameObject.GetComponent<TextMeshPro>().text = "Lat: " + waypoint.location.latitude.ToString();
                waypointObjDic[waypoint.waypoint_id].transform.Find("Location").gameObject.transform.Find("Long").gameObject.GetComponent<TextMeshPro>().text = "Long: " + waypoint.location.longitude.ToString();
                waypointObjDic[waypoint.waypoint_id].transform.Find("Type").gameObject.GetComponent<TextMeshPro>().text = "Type: " + waypoint.type.ToString();
                waypointObjDic[waypoint.waypoint_id].transform.Find("Description").gameObject.GetComponent<TextMeshPro>().text = "Desc: " + waypoint.description;

                waypointButtonDic[waypoint.waypoint_id].transform.Find("IconAndText").gameObject.transform.Find("Description").gameObject.GetComponent<TextMeshPro>().text = waypoint.description;
            }
        }
    }

    void OnDestroy()
    {
        // Unsubscribe when the script is destroyed
        if (waypointsDeletedEvent != null)
        {
            EventBus.Unsubscribe(waypointsDeletedEvent);
        }
        if (waypointsEditedEvent != null)
        {
            EventBus.Unsubscribe(waypointsEditedEvent);
        }
        if (waypointsAddedEvent != null)
        {
            EventBus.Unsubscribe(waypointsAddedEvent);
        }
    }

    public void SpawnWaypoint(Waypoint waypoint)
    {
        GameObject instantiatedObject = new GameObject();
        switch (waypoint.type)
        {
            case 0: // station
                _station_letter.text = waypoint.waypoint_letter.ToString();
                _station_title.text = waypoint.description.ToString();
                instantiatedObject = Instantiate(stationPrefab, GPSUtils.GPSCoordsToAppPosition(waypoint.location), Quaternion.identity);
                Debug.Log(GPSUtils.GPSCoordsToAppPosition(waypoint.location));
                instantiatedObject.transform.parent = this.gameObject.transform;
                waypointObjDic[waypoint.waypoint_id] = instantiatedObject;
                break;

            case 1: // nav
                _nav_letter.text = waypoint.waypoint_letter.ToString();
                _nav_title.text = waypoint.description.ToString();
                instantiatedObject = Instantiate(navPrefab, GPSUtils.GPSCoordsToAppPosition(waypoint.location), Quaternion.identity);
                instantiatedObject.transform.parent = this.gameObject.transform;
                waypointObjDic[waypoint.waypoint_id] = instantiatedObject;
                break;

            case 2: // geo
                _geo_letter.text = waypoint.waypoint_letter.ToString();
                _geo_title.text = waypoint.description.ToString();
                instantiatedObject = Instantiate(geoPrefab, GPSUtils.GPSCoordsToAppPosition(waypoint.location), Quaternion.identity);
                instantiatedObject.transform.parent = this.gameObject.transform;
                waypointObjDic[waypoint.waypoint_id] = instantiatedObject;
                break;

            case 3: // danger
                _danger_letter.text = waypoint.waypoint_letter.ToString();
                _danger_title.text = waypoint.description.ToString();
                instantiatedObject = Instantiate(dangerPrefab, GPSUtils.GPSCoordsToAppPosition(waypoint.location), Quaternion.identity);
                instantiatedObject.transform.parent = this.gameObject.transform;
                waypointObjDic[waypoint.waypoint_id] = instantiatedObject;
                break;

            default:

                break;
        }


    }


    public void CreateNew(Location loc, int type, string desc)
    {
        Waypoint way = new Waypoint
        {
            waypoint_id = AstronautInstance.User.WaypointData.currentIndex,
            waypoint_letter = getLetter(AstronautInstance.User.WaypointData.currentIndex),
            location = loc,
            type = type,
            description = desc,
            author = AstronautInstance.User.id
        };

        waypointDict[way.waypoint_id] = way;
        AstronautInstance.User.WaypointData.AllWaypoints.Add(way);
        SpawnWaypoint(way);

        GameObject button = screenHandler.AddButton(way);
        waypointButtonDic[way.waypoint_id] = button;

        AstronautInstance.User.WaypointData.currentIndex += 1;

        wd.SendWaypointData();
    }

    private string getLetter(int num)
    {
        string result = "";

        while (num >= 0)
        {
            result = (char)('A' + num % 26) + result;
            num = (num / 26) - 1;

            if (num < 0)
            {
                break;
            }
        }

        return result;
    }

    public Location getLocationOfButton(string letter)
    {
        int index = getNumGivenString(letter);
        if (waypointDict.ContainsKey(index))
        {
            return waypointDict[index].location;
        }

        return new Location();
    }

    private int getNumGivenString(string letter)
    {
        int result = 0;
        int length = letter.Length;

        for (int i = 0; i < length; i++)
        {
            char c = letter[i];
            result = result * 26 + (c - 'A' + 1);
        }

        return result - 1;
    }
}
