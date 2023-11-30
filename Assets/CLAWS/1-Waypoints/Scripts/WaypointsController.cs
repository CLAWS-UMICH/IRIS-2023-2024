using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaypointsController : MonoBehaviour
{
    public List<Waypoint> waypoints = new List<Waypoint>();

    [SerializeField] private GameObject waypointPrefab;

    private Subscription<WaypointsAddedEvent> waypointsAddedEvent;
    private Subscription<WaypointsDeletedEvent> waypointsDeletedEvent;
    private Subscription<WaypointsEditedEvent> waypointsEditedEvent;

    private Dictionary<int, Waypoint> waypointDict = new Dictionary<int, Waypoint>();

    private WebsocketDataHandler wd;

    // Start is called before the first frame update
    void Start()
    {
        wd = GetComponent<WebsocketDataHandler>();
        //dummy waypoints 
        Waypoint one = new Waypoint { 
            waypoint_id = 0,
            location = new Location(53, 24), 
            type = 0, 
            description = "help", 
            author = 0 };

        Waypoint two = new Waypoint
        {
            waypoint_id = 1,
            location = new Location(35, 10),
            type = 1,
            description = "bye",
            author = 1
        };

        Waypoint three = new Waypoint
        {
            waypoint_id = 3,
            location = new Location(29, 50),
            type = 3,
            description = "yes",
            author = 2
        };

        Waypoint four = new Waypoint
        {
            waypoint_id = 4,
            location = new Location(19, 20),
            type = 4,
            description = "no",
            author = 3
        };

        Waypoint five = new Waypoint
        {
            waypoint_id = 5,
            location = new Location(63, 20),
            type = 5,
            description = "no",
            author = 4
        };

        waypoints.Add(one);
        waypoints.Add(two);
        waypoints.Add(three);
        waypoints.Add(four);
        waypoints.Add(five);

        //
        waypointsAddedEvent = EventBus.Subscribe<WaypointsAddedEvent>(onWaypointsAdded);
        waypointsDeletedEvent = EventBus.Subscribe<WaypointsDeletedEvent>(onWaypointsDeleted);
        waypointsEditedEvent = EventBus.Subscribe<WaypointsEditedEvent>(onWaypointsEdited);
    }

    public void onWaypointsAdded(WaypointsAddedEvent e)
    {
        List<Waypoint> addedWaypoints = e.NewAddedWaypoints;
        foreach(Waypoint waypoint in addedWaypoints)
        {
            waypointDict.Add(waypoint.waypoint_id, waypoint);
            AstronautInstance.User.WaypointData.AllWaypoints.Add(waypoint);
        }
        wd.SendWaypointData();
    }

    public void onWaypointsDeleted(WaypointsDeletedEvent e)
    {
        List<Waypoint> deletedWaypoints = e.DeletedWaypoints;
        foreach(Waypoint waypoint in deletedWaypoints)
        {
            if (waypointDict.ContainsKey(waypoint.waypoint_id))
            {
                waypointDict.Remove(waypoint.waypoint_id);
            }
        }
        wd.SendWaypointData();
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
            }
        }
        wd.SendWaypointData();
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

    public void SpawnWaypoints()
    {
        foreach(Waypoint waypoint in waypoints)
        {
            Vector3 position = GPSUtils.GPSCoordsToAppPosition(waypoint.location);
            Instantiate(waypointPrefab, position, Quaternion.identity);
        }
    }

    public void SpawnWaypoints(int one)
    {
        foreach (Waypoint waypoint in waypoints)
        {
            Vector3 position = GPSUtils.GPSCoordsToAppPosition(waypoint.location);
            Instantiate(waypointPrefab, position, Quaternion.identity);
        }
    }

    public void CreateNew()
    {
        AstronautInstance.User.WaypointData.currentIndex += 1;
        Waypoint way = new Waypoint
        {
            waypoint_id = AstronautInstance.User.WaypointData.currentIndex,
            location = new Location(53, 24),
            type = 0,
            description = "help",
            author = 0
        };

        wd.SendWaypointData();
    }
}
