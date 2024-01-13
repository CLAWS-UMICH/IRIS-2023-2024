using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using PimDeWitte.UnityMainThreadDispatcher;

public class WebSocketClient : MonoBehaviour
{
    private WebSocket ws;
    private AstronautInstance astroInstance;
    private WebsocketDataHandler dataHandler;
    [SerializeField] string webSocketUrl = "ws://localhost:8080";

    private void Start()
    {
        astroInstance = GetComponent<AstronautInstance>();
        dataHandler = GetComponent<WebsocketDataHandler>();
        ws = new WebSocket(webSocketUrl);
        ws.OnMessage += OnWebSocketMessage;
        ws.Connect();
    }

    public void ReConnect()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
        ws = new WebSocket(webSocketUrl);
        ws.OnMessage += OnWebSocketMessage;
        ws.Connect();
    }

    public void ReConnect(string connectionString)
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
        webSocketUrl = connectionString;
        ws = new WebSocket(webSocketUrl);
        ws.OnMessage += OnWebSocketMessage;
        ws.Connect();
    }

    private void OnDestroy()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }

    private void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        if (e.Data != null)
        {
            try
            {
                UnityMainThreadDispatcher.Instance().Enqueue(ThisWillBeExecutedOnTheMainThread(e.Data));
            }
            catch (Exception ex)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => Debug.LogError("Error handling JSON message: " + ex.Message));
            }
        }
        else
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => Debug.LogWarning("Received empty JSON data."));
        }
    }

    public IEnumerator ThisWillBeExecutedOnTheMainThread(string jsonData)
    {
        //Debug.Log("This is executed from the main thread");
        HandleJsonMessage(jsonData);
        yield return null;
    }


    public void HandleJsonMessage(string jsonData)
    {
        // Deserialize the JSON into JsonMessage class
        JsonMessage jsonMessage = JsonUtility.FromJson<JsonMessage>(jsonData);

        // Determine the type of data
        string messageType = jsonMessage.type;
        string messageUse = jsonMessage.use;
        int messageID = jsonMessage.id;

        // Ignore connection that is not initial or does not match astronaut's ID or not -1
        if (messageType != "INITIAL" && messageID != AstronautInstance.User.id && messageID != -1)
        {
            return;
        }

        switch (messageType)
        {
            case "INITIAL":
                InitialData initialData = JsonUtility.FromJson<InitialData>(jsonData);
                dataHandler.HandleInitialData(initialData);
                break;
            case "Messaging":
                MessagingData messageData = JsonUtility.FromJson<MessagingData>(jsonData);
                dataHandler.HandleMessagingData(messageData.data, messageUse);
                break;
            case "Vitals":
                VitalsData vitalsData = JsonUtility.FromJson<VitalsData>(jsonData);
                dataHandler.HandleVitalsData(vitalsData.data, messageUse);
                break;
            case "Geosamples":
                GeosamplesData geoData = JsonUtility.FromJson<GeosamplesData>(jsonData);
                dataHandler.HandleGeosamplesData(geoData.data, messageUse);
                break;
            case "Waypoints":
                WaypointsData waypointsData = JsonUtility.FromJson<WaypointsData>(jsonData);
                dataHandler.HandleWaypointsData(waypointsData.data, messageUse);
                break;
            case "TaskList":
                TaskListData taskListData = JsonUtility.FromJson<TaskListData>(jsonData);
                dataHandler.HandleTaskListData(taskListData.data, messageUse);
                break;
            case "Alerts":
                AlertsData alertsData = JsonUtility.FromJson<AlertsData>(jsonData);
                dataHandler.HandleAlertsData(alertsData.data, messageUse);
                break;
            case "AllBreadCrumbs":
                AllBreadCrumbsData breadcrumbsData = JsonUtility.FromJson<AllBreadCrumbsData>(jsonData);
                dataHandler.HandleAllBreadCrumbsData(breadcrumbsData.data, messageUse);
                break;
            case "Location":
                LocationData locationData = JsonUtility.FromJson<LocationData>(jsonData);
                dataHandler.HandleLocationData(locationData.data, messageUse);
                break;
            case "Multiplayer":
                MultiplayerData multiData = JsonUtility.FromJson<MultiplayerData>(jsonData);
                dataHandler.HandleMultiplayerData(multiData.data, messageUse, multiData.id, multiData.dataToChange);
                break;
            case "Navigation":
                NavigationData navData = JsonUtility.FromJson<NavigationData>(jsonData);
                dataHandler.HandleNavData(navData.location, navData.use);
                break;
            case "Picture":
                PictureData picData = JsonUtility.FromJson<PictureData>(jsonData);
                dataHandler.HandlePicData(picData.pic, picData.use);
                break;
            // Handle other message types similarly
            default:
                Debug.LogWarning("Unknown message type: " + messageType);
                break;
        }
    }

    public void SendJsonData(string jsonData)
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Send(jsonData);
        }
    }
}

[Serializable]
public class JsonMessage
{
    public string type;
    public string use;
    public int id;
}

[Serializable]
public class InitialData
{
    public int id;
    public string color; // Hex values as a string. Ex: #223344
    public string type;
    public string data;
}

[Serializable]
public class MessagingData
{
    public int id;
    public string type;
    public string use;
    public Messaging data;
}

[Serializable]
public class VitalsData
{
    public int id;
    public string type;
    public string use;
    public Vitals data;
}

[Serializable]
public class GeosamplesData
{
    public int id;
    public string type;
    public string use;
    public Geosamples data;
}

[Serializable]
public class WaypointsData
{
    public int id;
    public string type;
    public string use;
    public Waypoints data;
}

[Serializable]
public class TaskListData
{
    public int id;
    public string type;
    public string use;
    public TaskList data;
}

[Serializable]
public class AlertsData
{
    public int id;
    public string type;
    public string use;
    public Alerts data;
}

[Serializable]
public class AllBreadCrumbsData
{
    public int id;
    public string type;
    public string use;
    public AllBreadCrumbs data;
}

[Serializable]
public class LocationData
{
    public int id;
    public string type;
    public string use;
    public Location data;
}

[Serializable]
public class MultiplayerData
{
    public int id;
    public string type;
    public string use;
    public FellowAstronaut data;
    public List<string> dataToChange;
}

[Serializable]
public class NavigationData
{
    public int id;
    public string type;
    public string use;
    public Location location;
}

[Serializable]
public class PictureData
{
    public int id;
    public string type;
    public string use;
    public string pic;
}
