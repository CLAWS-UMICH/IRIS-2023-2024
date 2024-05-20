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
    static int global_message_id = 0;

    public int message_id; // starting from 0 and going up 1
    public int sent_to; // Astronaut ID it was sent to
    public string message;
    public int from; // Astronaut ID it who sent the message

    public Message()
    {
        global_message_id++;
    }

    public Message(int init_sent_to, string init_message, int init_from)
    {
        message_id = global_message_id;
        sent_to = init_sent_to;
        message = init_message;
        from = init_from;

        global_message_id++;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Message otherMessage = (Message)obj;
        return message_id == otherMessage.message_id &&
               sent_to == otherMessage.sent_to &&
               message == otherMessage.message &&
               from == otherMessage.from;
    }
}

// Vitals
[System.Serializable]
public class Vitals
{
    public int eva_time;
    public double batt_time_left;
    public double oxy_pri_storage;
    public double oxy_sec_storage;
    public double oxy_pri_pressure;
    public double oxy_sec_pressure;
    public int oxy_time_left;
    public double heart_rate;
    public double oxy_consumption;
    public double co2_production;
    public double suit_pressure_oxy;
    public double suit_pressure_co2;
    public double suit_pressure_other;
    public double suit_pressure_total;
    public double fan_pri_rpm;
    public double fan_sec_rpm;
    public double helmet_pressure_co2;
    public double scrubber_a_co2_storage;
    public double scrubber_b_co2_storage;
    public double temperature;
    public double coolant_m;
    public double coolant_gas_pressure;
    public double coolant_liquid_pressure;

    public double batt_percentage;
    public double oxy_percentage;
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
    public int geosample_id;
    public char zone_id;
    public EvaData eva_data;
    public string time;
    public string color;
    public string shape;
    public bool starred;
    public string rock_type;
    public Location location;
    public int author;
    public string photo_jpg;
    public string description;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Geosample otherGeo = (Geosample)obj;
        return geosample_id == otherGeo.geosample_id &&
               zone_id == otherGeo.zone_id &&
               eva_data.Equals(otherGeo.eva_data) &&
               time == otherGeo.time &&
               color == otherGeo.color &&
               shape == otherGeo.shape &&
               starred == otherGeo.starred &&
               location.Equals(otherGeo.location) &&
               author == otherGeo.author && 
               photo_jpg == otherGeo.photo_jpg;
    }
}

[System.Serializable]
public class SPEC
{
    public EvaData eva1;
    public EvaData eva2;
}

[System.Serializable]
public class EvaData 
{
    public string name; // Name of rock
    public int id; // id of rock from NASA
    public DataDetails data; // data of rock
}

[System.Serializable]
public class DataDetails
{
    public double SiO2;
    public double TiO2;
    public double Al2O3;
    public double FeO;
    public double MnO;
    public double MgO;
    public double CaO;
    public double K2O;
    public double P2O3;
}

// Geosamples
[System.Serializable]
public class GeosampleZones
{
    public List<GeosampleZone> AllGeosampleZones = new List<GeosampleZone>();
}

[System.Serializable]
public class GeosampleZone
{
    public char zone_id;
    public List<int> ZoneGeosamplesIds = new();
    public Location location;
    public float radius;
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        GeosampleZone otherGeoZone = (GeosampleZone)obj;
        return ZoneGeosamplesIds.Equals(otherGeoZone.ZoneGeosamplesIds) &&
               zone_id == otherGeoZone.zone_id &&
               location.Equals(otherGeoZone.location) &&
               radius == otherGeoZone.radius;  
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
    public int currentIndex;
    public List<Waypoint> AllWaypoints = new List<Waypoint>();
}

[System.Serializable]
public class Waypoint
{
    public int waypoint_id; // starting from 0 and going up 1
    public string waypoint_letter;
    public Location location;
    public int type; // 0 = station, 1 = POI, 2 = geo, 3 = danger, 4 = Companions
    public string description;
    public int author;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Waypoint otherWay = (Waypoint)obj;
        return waypoint_id == otherWay.waypoint_id &&
               waypoint_letter == otherWay.waypoint_letter &&
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
    public int task_id; // starting from 0 and going up 1
    public int status; // 0 = Not Started, 1 = InProgress, 2 = Completed
    public string title;
    public string description; //Detailed description of task
    public bool isEmergency; //0 for not Emergency, 1 for Emergency
    public List<SingleAstronaut> astronauts; // All astronauts involved (including self)
    public List<SubtaskObj> subtasks;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        TaskObj otherTask = (TaskObj)obj;
        return task_id == otherTask.task_id &&
               title == otherTask.title &&
               astronauts.Equals(otherTask.astronauts) &&
               subtasks.Equals(otherTask.subtasks) &&
               status == otherTask.status &&
               isEmergency == otherTask.isEmergency;
    }

    public TaskObj(TaskObj other)
    {
        task_id = other.task_id;
        status = other.status;
        title = other.title;
        isEmergency = other.isEmergency;
        astronauts = new List<SingleAstronaut>(other.astronauts);
        subtasks = new List<SubtaskObj>(other.subtasks);
    }

    public TaskObj() { }

}

[System.Serializable]
public class SingleAstronaut
{
    public int astronaut_id;
    public bool ready;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SingleAstronaut otherTask = (SingleAstronaut)obj;
        return astronaut_id == otherTask.astronaut_id &&
               ready == otherTask.ready;
    }
}

[System.Serializable]
public class SubtaskObj
{
    public int subtask_id; // starting from 0 and going up 1
    public int status; // 0 = InProgress, 1 = Completed
    public string title;
    public string description;
    public List<int> astronautIDs;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SubtaskObj otherTask = (SubtaskObj)obj;
        return subtask_id == otherTask.subtask_id &&
               title == otherTask.title &&
               status == otherTask.status &&
               description == otherTask.description &&
               astronautIDs.Equals(otherTask.astronautIDs);
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
    public int alert_id; // starting from 0 and going up 1 
    public int astronaut_in_danger; // ID who is in danger
    public string vital; // vital that is in danger
    public float vital_val; // that vital's value

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        AlertObj otherAlert = (AlertObj)obj;
        return alert_id == otherAlert.alert_id &&
               astronaut_in_danger == otherAlert.astronaut_in_danger &&
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
    public int crumb_id;
    public Location location;
    public int type; // 0: backtracking and 1: navigation

    public Breadcrumb(int crumbId, Location location, int type)
    {
        this.crumb_id = crumbId;
        this.location = location;
        this.type = type;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Breadcrumb otherBread = (Breadcrumb)obj;
        return crumb_id == otherBread.crumb_id &&
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

    public Location() { }

    public Location(double lat, double lon)
    {
        latitude = lat;
        longitude = lon;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Location otherLoc = (Location)obj;
        return latitude == otherLoc.latitude &&
               longitude == otherLoc.longitude;
    }
}

// Data of other Fellow Astronauts
[System.Serializable]
public class FellowAstronaut
{
    public int astronaut_id;
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
        return astronaut_id == otherA.astronaut_id &&
               location.Equals(otherA.location) &&
               color == otherA.color &&
               vitals.Equals(otherA.vitals) &&
               navigating == otherA.navigating &&
               bread_crumbs.Equals(otherA.bread_crumbs);
    }
}

// TSS
[System.Serializable]
public class COMM
{
    public CommDetails comm;
}

[System.Serializable]
public class CommDetails
{
    public bool comm_tower;
}

[System.Serializable]
public class DCU
{
    public DCUData dcu;
}

[System.Serializable]
public class DCUData
{
    public EvaDetails eva1;
    public EvaDetails eva2;
}

[System.Serializable]
public class IMU
{
    public IMUEVAs imu;
}

[System.Serializable]
public class IMUEVAs
{
    public IMUData eva1;
    public IMUData eva2;
}

[System.Serializable]
public class IMUData
{
    public double posx;
    public double posy;
    public double heading;
}


[System.Serializable]
public class EvaDetails
{
    public bool batt;
    public bool oxy;
    public bool comm;
    public bool fan;
    public bool pump;
    public bool co2;
}

[System.Serializable]
public class ROVER
{
    public RoverDetails rover;
}

[System.Serializable]
public class RoverDetails
{
    public double posx;
    public double posy;
    public int qr_id;
}

[System.Serializable]
public class TELEMETRY
{
    public TelemetryDetails telemetry;
}

[System.Serializable]
public class TelemetryDetails
{
    public int eva_time;
    public EvaTelemetryDetails eva1;
    public EvaTelemetryDetails eva2;
}

[System.Serializable]
public class EvaTelemetryDetails
{
    public double batt_time_left;
    public double oxy_pri_storage;
    public double oxy_sec_storage;
    public double oxy_pri_pressure;
    public double oxy_sec_pressure;
    public int oxy_time_left;
    public double heart_rate;
    public double oxy_consumption;
    public double co2_production;
    public double suit_pressure_oxy;
    public double suit_pressure_co2;
    public double suit_pressure_other;
    public double suit_pressure_total;
    public double fan_pri_rpm;
    public double fan_sec_rpm;
    public double helmet_pressure_co2;
    public double scrubber_a_co2_storage;
    public double scrubber_b_co2_storage;
    public double temperature;
    public double coolant_m;
    public double coolant_gas_pressure;
    public double coolant_liquid_pressure;
}

[System.Serializable]
public class UIA
{
    public UiDetails uia;
}

[System.Serializable]
public class UiDetails
{
    public bool eva1_power;
    public bool eva1_oxy;
    public bool eva1_water_supply;
    public bool eva1_water_waste;
    public bool eva2_power;
    public bool eva2_oxy;
    public bool eva2_water_supply;
    public bool eva2_water_waste;
    public bool oxy_vent;
    public bool depress;
}