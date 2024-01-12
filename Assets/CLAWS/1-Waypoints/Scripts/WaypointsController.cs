using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaypointsController : MonoBehaviour
{
    [SerializeField] private GameObject waypointPrefab;

    private Subscription<WaypointsAddedEvent> waypointsAddedEvent;
    private Subscription<WaypointsDeletedEvent> waypointsDeletedEvent;
    private Subscription<WaypointsEditedEvent> waypointsEditedEvent;

    private Dictionary<int, Waypoint> waypointDict = new Dictionary<int, Waypoint>();
    private Dictionary<int, GameObject> waypointObjDic = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> waypointButtonDic = new Dictionary<int, GameObject>();

    private TextMeshPro _id, _letter, _lat, _lon, _type, _desc, _author;

    private WebsocketDataHandler wd;
    private WaypointScreenHandler screenHandler;

    // Start is called before the first frame update
    void Start()
    {
        wd = GameObject.Find("Controller").GetComponent<WebsocketDataHandler>();

        _id = waypointPrefab.transform.Find("ID").gameObject.GetComponent<TextMeshPro>();
        _letter = waypointPrefab.transform.Find("Letter").gameObject.GetComponent<TextMeshPro>();
        _lat = waypointPrefab.transform.Find("Location").gameObject.transform.Find("Lat").gameObject.GetComponent<TextMeshPro>();
        _lon = waypointPrefab.transform.Find("Location").gameObject.transform.Find("Long").gameObject.GetComponent<TextMeshPro>();
        _type = waypointPrefab.transform.Find("Type").gameObject.GetComponent<TextMeshPro>();
        _desc = waypointPrefab.transform.Find("Description").gameObject.GetComponent<TextMeshPro>();
        _author = waypointPrefab.transform.Find("Author").gameObject.GetComponent<TextMeshPro>();

        waypointsAddedEvent = EventBus.Subscribe<WaypointsAddedEvent>(onWaypointsAdded);
        waypointsDeletedEvent = EventBus.Subscribe<WaypointsDeletedEvent>(onWaypointsDeleted);
        waypointsEditedEvent = EventBus.Subscribe<WaypointsEditedEvent>(onWaypointsEdited);

        screenHandler = gameObject.GetComponent<WaypointScreenHandler>();
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

    public void onWaypointsDeleted(WaypointsDeletedEvent e)
    {
        Debug.Log("deleted");
        List<Waypoint> deletedWaypoints = e.DeletedWaypoints;
        foreach(Waypoint waypoint in deletedWaypoints)
        {
            if (waypointDict.ContainsKey(waypoint.waypoint_id))
            {
                waypointDict.Remove(waypoint.waypoint_id);
                GameObject gm = waypointObjDic[waypoint.waypoint_id];
                waypointObjDic.Remove(waypoint.waypoint_id);

                screenHandler.DeleteButton(waypointButtonDic[waypoint.waypoint_id], waypoint.type);
                waypointButtonDic.Remove(waypoint.waypoint_id);
                Destroy(gm);
            }

            AstronautInstance.User.WaypointData.AllWaypoints.Remove(waypoint);
        }
    }

    public void onWaypointsEdited(WaypointsEditedEvent e)
    {
        Debug.Log("edited");
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
        _id.text = "ID: " + waypoint.waypoint_id.ToString();
        _letter.text = "ID: " + waypoint.waypoint_letter.ToString();
        _lat.text = "Lat: " + waypoint.location.latitude.ToString();
        _lon.text = "Lon: " + waypoint.location.longitude.ToString();
        _type.text = "Type: " + waypoint.type.ToString();
        _desc.text = "Desc: " + waypoint.description;
        _author.text = "Author: " + waypoint.author.ToString();

        GameObject instantiatedObject = Instantiate(waypointPrefab, GPSUtils.GPSCoordsToAppPosition(waypoint.location), Quaternion.identity);
        instantiatedObject.transform.parent = this.gameObject.transform;
        waypointObjDic[waypoint.waypoint_id] = instantiatedObject;


    }


    public void CreateNew()
    {
        Waypoint way = new Waypoint
        {
            waypoint_id = AstronautInstance.User.WaypointData.currentIndex,
            waypoint_letter = getLetter(AstronautInstance.User.WaypointData.currentIndex),
            location = new Location(29.5647012, -95.081375),
            type = 0,
            description = "help",
            author = AstronautInstance.User.id
        };

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
}
