using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Astronaut
{
    public int id;
    public string name;
    public string initial;
    public Messaging MessagingData;
    public Vitals VitalsData;
    public Geosamples GeosampleData;
    public GeosampleZones GeosampleZonesData;
    public Waypoints WaypointData;
    public TaskList TasklistData;
    public Alerts AlertData;
    public AllBreadCrumbs BreadCrumbData;
    public Location location;
    public bool currently_navigating;
    public bool inDanger;
    public string color;

    public FellowAstronaut FellowAstronautsData;
}
