using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;

/*
Name: Brian Schneider
Description: This script handles data communication through websockets, including sending and receiving various types of data.
 */

public class WebsocketDataHandler : MonoBehaviour
{
    private WebSocketClient wsClient;
    [SerializeField] private bool debugMode = false;

    public void Start()
    {
        wsClient = GetComponent<WebSocketClient>();
    }

    public void HandleInitialData(InitialData data, string use)
    {
        if (use == "GET")
        {
            if (debugMode) Debug.Log("(GET) WebsocketDataHandler.cs: Sending INITIAL data");

            // Create a new CombinedData instance
            InitialData combinedData = new InitialData
            {
                type = "INITIAL",
                use = "PUT",
                color = data.color,
                name = data.name,
                id = data.id
            };

            // Convert the combined data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);

            wsClient.SendJsonData(jsonData);
        } 
        else
        {
            try
            {
                AstronautInstance.User.id = data.id;
                AstronautInstance.User.color = data.color;
                AstronautInstance.User.name = data.name;

                if (data.name.Length > 1)
                {
                    AstronautInstance.User.initial = data.name[0].ToString();
                }

                // Create a new CombinedData instance
                InitialData combinedData = new InitialData
                {
                    id = AstronautInstance.User.id,
                    type = "INITIAL",
                    data = "SUCCESS"
                };

                // Convert the combined data to JSON format and send to WebSocket client
                string jsonData = JsonUtility.ToJson(combinedData);

                wsClient.SendJsonData(jsonData);
            }
            catch (Exception ex)
            {
                Debug.LogError("An exception occurred: " + ex.Message);
                try
                {
                    // Create a new CombinedData instance
                    InitialData combinedData = new InitialData
                    {
                        id = AstronautInstance.User.id,
                        type = "INITIAL",
                        data = "FAILURE"
                    };

                    // Convert the combined data to JSON format and send to WebSocket client
                    string jsonData = JsonUtility.ToJson(combinedData);

                    wsClient.SendJsonData(jsonData);
                }
                catch (Exception ex1)
                {
                    Debug.LogError("An exception occurred: " + ex1.Message);
                }
            }
        }
    }
    public void HandleMessagingData(Messaging data, string use)
    {
        if (use == "GET")
        {
            if (debugMode) Debug.Log("(GET) WebsocketDataHandler.cs: Sending MESSAGING data");

            // Create a new CombinedData instance
            MessagingData combinedData = new MessagingData
            {
                id = AstronautInstance.User.id,
                type = "MESSAGING",
                use = "PUT",
                data = AstronautInstance.User.MessagingData
            };

            // Convert the combined data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);

            wsClient.SendJsonData(jsonData);
        } 
        else if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating MESSAGING data");

            // Get the current list of messages from the instance
            List<Message> currentMessages = AstronautInstance.User.MessagingData.AllMessages;

            // Get the new list of messages from the data parameter
            List<Message> newMessages = data.AllMessages;

            List<Message> deletedMessages = new List<Message>();
            List<Message> editedMessages = new List<Message>();
            List<Message> newAddedMessages = new List<Message>();

            foreach (Message currentMessage in currentMessages)
            {
                bool messageFound = false;

                foreach (Message newMessage in newMessages)
                {
                    if (currentMessage.message_id == newMessage.message_id)
                    {
                        messageFound = true;
                        if (!currentMessage.Equals(newMessage))
                        {
                            editedMessages.Add(newMessage);
                        }
                        break;
                    }
                }

                if (!messageFound)
                {
                    deletedMessages.Add(currentMessage);
                }
            }

            foreach (Message newMessage in newMessages)
            {
                bool isNew = true;

                foreach (Message currentMessage in currentMessages)
                {
                    if (currentMessage.message_id == newMessage.message_id)
                    {
                        isNew = false;
                        break;
                    }
                }

                if (isNew)
                {
                    newAddedMessages.Add(newMessage);
                }
            }

            // Publish events for each scenario
            if (deletedMessages.Count > 0)
            {
                EventBus.Publish(new MessagesDeletedEvent(deletedMessages));
            }

            if (editedMessages.Count > 0)
            {
                EventBus.Publish(new MessagesEditedEvent(editedMessages));
            }

            if (newAddedMessages.Count > 0)
            {
                EventBus.Publish(new MessagesAddedEvent(newAddedMessages));
            }

            // Update the list of messages with the new data
            AstronautInstance.User.MessagingData.AllMessages = data.AllMessages;
        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandleVitalsData(Vitals data, string use)
    {
        if (use == "GET")
        {
            if (debugMode) Debug.Log("(GET) WebsocketDataHandler.cs: Sending VITALS data");

            // Create a new CombinedData instance
            VitalsData combinedData = new VitalsData
            {
                id = AstronautInstance.User.id,
                type = "VITALS",
                use = "PUT",
                data = AstronautInstance.User.VitalsData,
            };

            // Convert the vitals data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);
            wsClient.SendJsonData(jsonData);
        }
        else if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating VITALS data");

            EventBus.Publish(new VitalsUpdatedEvent(data));

            // Update the list of geosamples with the new data
            AstronautInstance.User.VitalsData = data;
        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandleGeosamplesData(Geosamples data, GeosampleZones zoneData, string use)
    {
        if (use == "GET")
        {
            if (debugMode) Debug.Log("(GET) WebsocketDataHandler.cs: Sending GEOSAMPLE data");

            // Create a new CombinedData instance
            GeosamplesData combinedData = new GeosamplesData
            {
                id = AstronautInstance.User.id,
                type = "GEOSAMPLES",
                use = "PUT",
                data = AstronautInstance.User.GeosampleData, 
                zones = AstronautInstance.User.GeosampleZonesData
            };

            // Convert the vitals data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);
            wsClient.SendJsonData(jsonData);
        }
        else if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating GEOSAMPLE data");

            // TODO - Add events for each scenario
         
            // Get the current list of geosamples from the instance
            List<Geosample> currentGeosamples = AstronautInstance.User.GeosampleData.AllGeosamples;
            List<GeosampleZone> currentGeosampleZones = AstronautInstance.User.GeosampleZonesData.AllGeosampleZones;

            // Get the new list of geosamples from the data parameter
            List<Geosample> newGeosamples = data.AllGeosamples;

            List<Geosample> deletedGeosamples = new List<Geosample>();
            List<GeosampleZone> deletedGeosampleZones = new List<GeosampleZone>();

            List<Geosample> editedGeosamples = new List<Geosample>();

            List<Geosample> newAddedGeosamples = new List<Geosample>();

            //go through all zones, check samples exist for each

            foreach (Geosample sample in newGeosamples)
            {
                Debug.Log(sample.ToString());
            }

            foreach (GeosampleZone currentZone in currentGeosampleZones)
            {
                bool sampleFound = false;

                if (currentZone.ZoneGeosamplesIds.Count == 0)
                {
                    deletedGeosampleZones.Add(currentZone);
                }
            }

            foreach (Geosample currentSample in currentGeosamples)
            {
                bool sampleFound = false;
                if (currentSample == null)
                {
                    Debug.Log("bleh");
                }

                foreach (Geosample newSample in newGeosamples)
                {
                    if (currentSample.geosample_id == newSample.geosample_id)
                    {
                        if (currentSample == null)
                        {
                            Debug.Log("blah " + currentSample.ToString() + " / " + newSample.ToString());
                        }

                        sampleFound = true;
                        if (!currentSample.Equals(newSample))
                        {
                            editedGeosamples.Add(newSample);
                        }
                        break;
                    }
                }

                if (!sampleFound)
                {
                    deletedGeosamples.Add(currentSample);
                }
            }

            foreach (Geosample newSample in newGeosamples)
            {
                bool isNew = true;

                foreach (Geosample currentSample in currentGeosamples)
                {
                    if (currentSample.geosample_id == newSample.geosample_id)
                    {
                        isNew = false;
                        break;
                    }
                }

                if (isNew)
                {
                    newAddedGeosamples.Add(newSample);
                }
            }

            // Publish events for each scenario
            if (deletedGeosamples.Count > 0)
            {
                EventBus.Publish(new GeosamplesDeletedEvent(deletedGeosamples));
                Debug.Log("deleted: " + deletedGeosamples.Count.ToString());
            }

            if (editedGeosamples.Count > 0)
            {
                EventBus.Publish(new GeosamplesEditedEvent(editedGeosamples));
                Debug.Log("edited: " + editedGeosamples.Count.ToString());
            }

            if (newAddedGeosamples.Count > 0)
            {
                EventBus.Publish(new GeosamplesAddedEvent(newAddedGeosamples));
                Debug.Log("new: " + newAddedGeosamples.Count.ToString());
            }

            if (deletedGeosampleZones.Count > 0)
            {
                EventBus.Publish(new GeosampleZoneDeletedEvent(deletedGeosampleZones));
            }

            // Update the list of geosamples with the new data
            AstronautInstance.User.GeosampleData.AllGeosamples = data.AllGeosamples;
        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandleWaypointsData(Waypoints data, string use)
    {
        if (use == "GET")
        {
            if (debugMode) Debug.Log("(GET) WebsocketDataHandler.cs: Sending WAYPOINTS data");

            // Create a new CombinedData instance
            WaypointsData combinedData = new WaypointsData
            {
                id = AstronautInstance.User.id,
                type = "WAYPOINTS",
                use = "PUT",
                data = AstronautInstance.User.WaypointData
            };

            // Convert the vitals data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);
            wsClient.SendJsonData(jsonData);
        }
        else if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating WAYPOINTS data");

            // Get the current list of waypoints from the instance
            List<Waypoint> currentWaypoints = AstronautInstance.User.WaypointData.AllWaypoints;

            // Get the new list of waypoints from the data parameter
            List<Waypoint> newWaypoints = data.AllWaypoints;

            List<Waypoint> deletedWaypoints = new List<Waypoint>();
            List<Waypoint> editedWaypoints = new List<Waypoint>();
            List<Waypoint> newAddedWaypoints = new List<Waypoint>();

            foreach (Waypoint currentWaypoint in currentWaypoints)
            {
                bool waypointFound = false;

                foreach (Waypoint newWaypoint in newWaypoints)
                {
                    if (currentWaypoint.waypoint_id == newWaypoint.waypoint_id)
                    {
                        waypointFound = true;
                        if (!currentWaypoint.Equals(newWaypoint))
                        {
                            editedWaypoints.Add(newWaypoint);
                        }
                        break;
                    }
                }

                if (!waypointFound)
                {
                    deletedWaypoints.Add(currentWaypoint);
                }
            }

            foreach (Waypoint newWaypoint in newWaypoints)
            {
                bool isNew = true;

                foreach (Waypoint currentWaypoint in currentWaypoints)
                {
                    if (currentWaypoint.waypoint_id == newWaypoint.waypoint_id)
                    {
                        isNew = false;
                        break;
                    }
                }

                if (isNew)
                {
                    newAddedWaypoints.Add(newWaypoint);
                }
            }

            // Publish events for each scenario
            if (deletedWaypoints.Count > 0)
            {
                EventBus.Publish(new WaypointsDeletedEvent(deletedWaypoints));
            }

            if (editedWaypoints.Count > 0)
            {
                EventBus.Publish(new WaypointsEditedEvent(editedWaypoints));
            }

            if (newAddedWaypoints.Count > 0)
            {
                EventBus.Publish(new WaypointsAddedEvent(newAddedWaypoints));
            }

            // Update the list of waypoints with the new data
            AstronautInstance.User.WaypointData.AllWaypoints = data.AllWaypoints;
            AstronautInstance.User.WaypointData.currentIndex = data.currentIndex;
        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandleTaskListData(TaskList data, string use)
    {
        if (use == "GET")
        {
            if (debugMode) Debug.Log("(GET) WebsocketDataHandler.cs: Sending TASKLIST data");

            // Create a new CombinedData instance
            TaskListData combinedData = new TaskListData
            {
                id = AstronautInstance.User.id,
                type = "TASKLIST",
                use = "PUT",
                data = AstronautInstance.User.TasklistData
            };

            // Convert the vitals data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);
            wsClient.SendJsonData(jsonData);
        }
        else if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating TASKLIST data");

            // Get the current list of tasks from the instance
            List<TaskObj> currentTasks = AstronautInstance.User.TasklistData.AllTasks;

            // Get the new list of tasks from the data parameter
            List<TaskObj> newTasks = data.AllTasks;

            List<TaskObj> deletedTasks = new List<TaskObj>();
            List<TaskObj> editedTasks = new List<TaskObj>();
            List<TaskObj> newAddedTasks = new List<TaskObj>();
            List<TaskObj> taskFinished = new List<TaskObj>();

            foreach (TaskObj currentTask in currentTasks)
            {
                bool taskFound = false;

                foreach (TaskObj newTask in newTasks)
                {
                    if (currentTask.task_id == newTask.task_id)
                    {
                        taskFound = true;
                        if (!currentTask.Equals(newTask))
                        {
                            editedTasks.Add(newTask);

                            if (newTask.isEmergency)
                            {
                                EventBus.Publish<CreateAlert>(new CreateAlert(AlertEnum.TaskList_EmergencyTask, "Emergency Task Updated", newTask.title));
                            }
                        }
                        break;
                    }
                }

                if (!taskFound)
                {
                    deletedTasks.Add(currentTask);
                }
            }

            foreach (TaskObj newTask in newTasks)
            {
                bool isNew = true;

                foreach (TaskObj currentTask in currentTasks)
                {
                    if (currentTask.task_id == newTask.task_id)
                    {
                        isNew = false;
                        break;
                    }
                }

                if (isNew)
                {
                    newAddedTasks.Add(newTask);

                    if (!newTask.isEmergency)
                    {
                        EventBus.Publish<CreateAlert>(new CreateAlert(AlertEnum.TaskList_NewTask, "New Task Added", newTask.title));
                    }
                    else
                    {
                        EventBus.Publish<CreateAlert>(new CreateAlert(AlertEnum.TaskList_EmergencyTask, "New Emergency Task", newTask.title));
                    }
                }
            }

            // Publish events for each scenario
            if (deletedTasks.Count > 0)
            {
                EventBus.Publish(new TasksDeletedEvent(deletedTasks));
            }

            if (editedTasks.Count > 0)
            {
                EventBus.Publish(new TasksEditedEvent(editedTasks));
            }

            if (newAddedTasks.Count > 0)
            {
                EventBus.Publish(new TasksAddedEvent(newAddedTasks));
            }

            // Update the list of tasks with the new data
            AstronautInstance.User.TasklistData.AllTasks = data.AllTasks;

        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandleAlertsData(Alerts data, string use)
    {
        if (use == "GET")
        {
            if (debugMode) Debug.Log("(GET) WebsocketDataHandler.cs: Sending ALERTS data");

            // Create a new CombinedData instance
            AlertsData combinedData = new AlertsData
            {
                id = AstronautInstance.User.id,
                type = "ALERTS",
                use = "PUT",
                data = AstronautInstance.User.AlertData
            };

            // Convert the vitals data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);

            wsClient.SendJsonData(jsonData);
        }
        else if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating ALERTS data");

            // Convert the alerts data to JSON format and send it to WebSocket
            AstronautInstance.User.AlertData.AllAlerts = data.AllAlerts;
        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandleAllBreadCrumbsData(AllBreadCrumbs data, string use)
    {
        if (use == "GET")
        {
            if (debugMode) Debug.Log("(GET) WebsocketDataHandler.cs: Sending BREADCRUMBS data");

            // Create a new CombinedData instance
            AllBreadCrumbsData combinedData = new AllBreadCrumbsData
            {
                id = AstronautInstance.User.id,
                type = "ALLBREADCRUMBS",
                use = "PUT",
                data = AstronautInstance.User.BreadCrumbData
            };

            // Convert the vitals data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);
            wsClient.SendJsonData(jsonData);
        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandleLocationData(Location data, string use)
    {
        if (use == "GET")
        {
            if (debugMode) Debug.Log("(GET) WebsocketDataHandler.cs: Sending LOCATION data");

            // Create a new CombinedData instance
            LocationData combinedData = new LocationData
            {
                id = AstronautInstance.User.id,
                type = "LOCATION",
                use = "PUT",
                data = AstronautInstance.User.location
            };

            // Convert the vitals data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);

            wsClient.SendJsonData(jsonData);
        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandleMultiplayerData(FellowAstronaut data, string use, int id, List<string> changes)
    {
        if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating FELLOWASTRONAUT data");

            foreach (string change in changes)
            {
                switch (change)
                {
                    case "location":
                        EventBus.Publish(new FellowAstronautLocationDataChangeEvent(data));
                        break;
                    case "vitals":
                        EventBus.Publish(new FellowAstronautVitalsDataChangeEvent(data.vitals));
                        break;
                    case "breadcrumbs":
                        EventBus.Publish(new FellowAstronautBreadcrumbsDataChangeEvent(data));
                        break;
                    case "navigating":
                        EventBus.Publish(new FellowAstronautNavigatingDataChangeEvent(data));
                        break;
                    default:
                        if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: UNKNOWN TYPE IN FELLOWASTRONAUT data");
                        break;
                }

            }

            // Update the fellow astronaut's data with the new data
            AstronautInstance.User.FellowAstronautsData = data;

        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandleNavData(Location location, string use)
    {
        if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating NAVIGATION data");

            EventBus.Publish(new WebNavEvent(location));

        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandlePicData(string pic, string title, int height, int width, string use)
    {
        if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating PICTURE data");

            EventBus.Publish(new NewPicEvent(pic, title, height, width));

        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandleHighlightData(int id, string use)
    {
        if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating HIGHLIGHT data");

            EventBus.Publish(new LLMCHighlight(id));

        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandleKill()
    {
        if (debugMode) Debug.Log("WebsocketDataHandler.cs: Killing Astronaut " + AstronautInstance.User.id);

        // Create a new CombinedData instance
        KillData combinedData = new KillData
        {
            id = AstronautInstance.User.id,
            type = "KILL",
        };

        // Convert the vitals data to JSON format and send to WebSocket client
        string jsonData = JsonUtility.ToJson(combinedData);

        wsClient.SendJsonData(jsonData);
    }

    public void HandleAudioData(VegaAudio _data, string use)
    {

        if (use == "GET")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Sending Audio");

            // Create a new CombinedData instance
            AudioData combinedData = new AudioData
            {
                id = AstronautInstance.User.id,
                type = "AUDIO",
                use = "PUT",
                data = _data
            };

            // Convert the vitals data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);

            wsClient.SendJsonData(jsonData);

        }
        else if (use == "PUT")
        {
            EventBus.Publish(new SpeechToText(_data.text_from_VEGA));
        } 
        else
        {
            Debug.Log("Invalid use case from server");
        }

    }

    public void HandleOrocessedAudioData(VegaAudio _data, string use)
    {

        if (use == "GET")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Sending Audio");

            // Create a new CombinedData instance
            AudioData combinedData = new AudioData
            {
                id = AstronautInstance.User.id,
                type = "AUDIO",
                use = "PUT",
                data = _data
            };

            // Convert the vitals data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);

            wsClient.SendJsonData(jsonData);

        }
        else if (use == "PUT")
        {
            EventBus.Publish(new SpeechToText(_data.text_from_VEGA));
            //EventBus.Publish(new VEGACommand(_data.));
        }
        else
        {
            Debug.Log("Invalid use case from server");
        }

    }
    public void HandleUIAData(UIAImage _data, string use)
    {

        if (use == "GET")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Sending UIA");

            // Create a new CombinedData instance
            UIAData combinedData = new UIAData
            {
                id = AstronautInstance.User.id,
                type = "UIAIMAGE",
                use = "PUT",
                data = _data
            };

            // Convert the vitals data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);
            Debug.Log(jsonData);

            wsClient.SendJsonData(jsonData);

        }
        else if (use == "PUT")
        {
            // EventBus.Publish(new SpeechToText(_data.points));
            GameObject.Find("UIA").GetComponent<imageCapture>().processUIAwebsocket(_data.position, _data.rotation, _data.points);
        }
        else
        {
            Debug.Log("Invalid use case from server");
        }


    }

    // Public functions for to call to send data
    public void SendInitialData(string color, string name, int _id)
    {
        InitialData emptyInitials = new InitialData();
        emptyInitials.use = "PUT";
        emptyInitials.type = "INITIAL";
        emptyInitials.color = color;
        emptyInitials.name = name;
        emptyInitials.id = _id;
        HandleInitialData(emptyInitials, "GET");
    }

    public void SendMessageData()
    {
        Messaging emptyMessagingData = new Messaging();
        HandleMessagingData(emptyMessagingData, "GET");
    }

    public void SendVitalsData()
    {
        Vitals emptyVitalsData = new Vitals();
        HandleVitalsData(emptyVitalsData, "GET");
    }

    public void SendGeosampleData()
    {
        Geosamples emptyGeosampleData = new Geosamples();
        GeosampleZones emptyGeosampleZoneData = new GeosampleZones();
        HandleGeosamplesData(emptyGeosampleData, emptyGeosampleZoneData, "GET");
    }

    public void SendWaypointData()
    {
        Waypoints emptyWaypointData = new Waypoints();
        HandleWaypointsData(emptyWaypointData, "GET");
    }

    public void SendTasklistData()
    {
        TaskList emptyTasklistData = new TaskList();
        HandleTaskListData(emptyTasklistData, "GET");
    }

    public void SendAlertsData()
    {
        Alerts emptyAlertsData = new Alerts();
        HandleAlertsData(emptyAlertsData, "GET");
    }
    
    public void SendBreadCrumbData()
    {
        AllBreadCrumbs emptyBreadCrumbData = new AllBreadCrumbs();
        HandleAllBreadCrumbsData(emptyBreadCrumbData, "GET");
    }

    public void SendLocationData()
    {
        Location emptyLocationData = new Location();
        HandleLocationData(emptyLocationData, "GET");
    }

    public void SendKill()
    {
        Location emptyLocationData = new Location();
        HandleKill();
    }

    public void SendAudio(VegaAudio data)
    {
        HandleAudioData(data, "GET");
    }

    public void SendUIA(UIAImage data)
    {
        HandleUIAData(data, "GET");
    }
}
