using UnityEngine;
using System.Collections.Generic;
using System;

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

    public void HandleInitialData(InitialData data)
    {
        try
        {
            AstronautInstance.User.id = data.id;

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
    public void HandleMessagingData(Messaging data, string use)
    {
        if (use == "GET")
        {
            if (debugMode) Debug.Log("(GET) WebsocketDataHandler.cs: Sending MESSAGING data");

            // Create a new CombinedData instance
            MessagingData combinedData = new MessagingData
            {
                id = AstronautInstance.User.id,
                type = "Messaging",
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
                type = "Vitals",
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

    public void HandleGeosamplesData(Geosamples data, string use)
    {
        if (use == "GET")
        {
            if (debugMode) Debug.Log("(GET) WebsocketDataHandler.cs: Sending GEOSAMPLE data");

            // Create a new CombinedData instance
            GeosamplesData combinedData = new GeosamplesData
            {
                id = AstronautInstance.User.id,
                type = "Geosamples",
                use = "PUT",
                data = AstronautInstance.User.GeosampleData
            };

            // Convert the vitals data to JSON format and send to WebSocket client
            string jsonData = JsonUtility.ToJson(combinedData);
            wsClient.SendJsonData(jsonData);
        }
        else if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating GEOSAMPLE data");

            // Get the current list of geosamples from the instance
            List<Geosample> currentGeosamples = AstronautInstance.User.GeosampleData.AllGeosamples;

            // Get the new list of geosamples from the data parameter
            List<Geosample> newGeosamples = data.AllGeosamples;

            List<Geosample> deletedGeosamples = new List<Geosample>();
            List<Geosample> editedGeosamples = new List<Geosample>();
            List<Geosample> newAddedGeosamples = new List<Geosample>();

            foreach (Geosample currentSample in currentGeosamples)
            {
                bool sampleFound = false;

                foreach (Geosample newSample in newGeosamples)
                {
                    if (currentSample.geosample_id == newSample.geosample_id)
                    {
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
            }

            if (editedGeosamples.Count > 0)
            {
                EventBus.Publish(new GeosamplesEditedEvent(editedGeosamples));
            }

            if (newAddedGeosamples.Count > 0)
            {
                EventBus.Publish(new GeosamplesAddedEvent(newAddedGeosamples));
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
                type = "Waypoints",
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
                type = "TaskList",
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
                type = "Alerts",
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
                type = "AllBreadCrumbs",
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
                type = "Location",
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
                        EventBus.Publish(new FellowAstronautVitalsDataChangeEvent(data));
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

            EventBus.Publish(new StartPathfinding(location));

        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    public void HandlePicData(string pic, string use)
    {
        if (use == "PUT")
        {
            if (debugMode) Debug.Log("(PUT) WebsocketDataHandler.cs: Updating PICTURE data");

            EventBus.Publish(new NewPicEvent(pic));

        }
        else
        {
            Debug.Log("Invalid use case from server");
        }
    }

    // Public functions for to call to send data
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
        HandleGeosamplesData(emptyGeosampleData, "GET");
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

}
