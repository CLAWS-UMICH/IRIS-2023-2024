using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// Messaging
[System.Serializable]
public class Messaging
{
    public List<Message> AllMessages = new List<Message>();
}

[System.Serializable]
public class Message
{
    public int id;
    public int sent_to;
    public string message;
    public int from;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Message otherMessage = (Message)obj;
        return id == otherMessage.id &&
               sent_to == otherMessage.sent_to &&
               message == otherMessage.message &&
               from == otherMessage.from;
    }
}

// Vitals
[System.Serializable]
public class Vitals
{
    public int room_id;
    public bool is_running;
    public bool is_paused;
    public float time;
    public string timer;              // hh:mm:ss
    public string started_at;         // hh:mm:ss
    public float primary_oxygen;
    public float secondary_oxygen;
    public float suit_pressure;
    public float sub_pressure;
    public float o2_pressure;
    public float o2_rate;
    public float h2o_gas_pressure;
    public float h2o_liquid_pressure;
    public float sop_pressure;
    public float sop_rate;
    public float heart_rate;
    public float fan_tachometer;
    public float battery_capacity;
    public float temperature;
    public string battery_time_left;   // hh:mm:ss
    public string o2_time_left;        // hh:mm:ss
    public string h2o_time_left;       // hh:mm:ss
    public float battery_percentage;
    public float battery_outputput;
    public float oxygen_primary_time;
    public float oxygen_secondary_time;
    public float water_capacity;
}

// Geosamples
[System.Serializable]
public class Geosamples
{
    public List<Geosample> AllGeosamples = new List<Geosample>();
}

[System.Serializable]
public class Geosample
{
    public int id;
    public SpecData spec_data;
    public string time;
    public Location location;
    public int author;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Geosample otherGeo = (Geosample)obj;
        return id == otherGeo.id &&
               spec_data.Equals(otherGeo.spec_data) &&
               time == otherGeo.time &&
               location.Equals(otherGeo.location) &&
               author == otherGeo.author;
    }
}

[System.Serializable]
public class SpecData
{
    public int rock_example_data;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SpecData otherSpecData = (SpecData)obj;
        return rock_example_data == otherSpecData.rock_example_data;
    }
}

// Waypoints
[System.Serializable]
public class Waypoints
{
    public List<Waypoint> AllWaypoints = new List<Waypoint>();
}

[System.Serializable]
public class Waypoint
{
    public int id;
    public Location location;
    public int type; // 0 = station, 1 = nav, 2 = geo, 3 = danger
    public string description;
    public int author;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Waypoint otherWay = (Waypoint)obj;
        return id == otherWay.id &&
               location.Equals(otherWay.location) &&
               type == otherWay.type &&
               description == otherWay.description &&
               author == otherWay.author;
    }
}

// Tasklists
[System.Serializable]
public class TaskList
{
    public List<TaskObj> AllTasks = new List<TaskObj>();
}

[System.Serializable]
public class TaskObj
{
    public int id;
    public int status; // 0 = InProgress, 1 = Completed
    public string title;
    public string description;
    public int shared_with;
    public int parentTask; // Used for subtasks. If it is a parent class, this equals -1, otherwise, parentTask = TaskObj's id of its parent Task

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        TaskObj otherTask = (TaskObj)obj;
        return id == otherTask.id &&
               title == otherTask.title &&
               description == otherTask.description &&
               status == otherTask.status &&
               shared_with == otherTask.shared_with &&
               parentTask == otherTask.parentTask;
    }
}

// Alerts
[System.Serializable]
public class Alerts
{
    public List<AlertObj> AllAlerts = new List<AlertObj>();
}

[System.Serializable]
public class AlertObj
{
    public int id;
    public int id_in_danger;
    public string vital;
    public float vital_val;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        AlertObj otherAlert = (AlertObj)obj;
        return id == otherAlert.id &&
               id_in_danger == otherAlert.id_in_danger &&
               vital == otherAlert.vital &&
               vital_val == otherAlert.vital_val;
    }
}

// Breadcrumbs

[System.Serializable]
public class AllBreadCrumbs
{
    public List<Breadcrumb> AllCrumbs = new List<Breadcrumb>();

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        AllBreadCrumbs otherCrumbs = (AllBreadCrumbs)obj;

        // Compare lists using the SequenceEqual method from System.Linq
        return AllCrumbs.SequenceEqual(otherCrumbs.AllCrumbs);
    }
}

[System.Serializable]
public class Breadcrumb
{
    public int id;
    public Location location;
    public int type; // 0: backtracking and 1: navigation

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Breadcrumb otherBread = (Breadcrumb)obj;
        return id == otherBread.id &&
               location.Equals(otherBread.location) &&
               type == otherBread.type;
    }
}

// Location
[System.Serializable]
public class Location
{
    public double latitude;
    public double longitude;
    public int id;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Location otherLoc = (Location)obj;
        return id == otherLoc.id &&
               latitude == otherLoc.latitude &&
               longitude == otherLoc.longitude;
    }

    public Vector3 GetCoordinatesAsVector3()
    {
        // Assuming Z-axis is zero in this context
        return new Vector3((float)latitude, 0f, (float)longitude);
    }
}

// Data of other Fellow Astronauts
[System.Serializable]
public class FellowAstronaut
{
    public int id;
    public Location location;
    public string color;
    public Vitals vitals;
    public bool navigating;
    public AllBreadCrumbs bread_crumbs;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        FellowAstronaut otherA = (FellowAstronaut)obj;
        return id == otherA.id &&
               location.Equals(otherA.location) &&
               color == otherA.color &&
               vitals.Equals(otherA.vitals) &&
               navigating == otherA.navigating &&
               bread_crumbs.Equals(otherA.bread_crumbs);
    }
}