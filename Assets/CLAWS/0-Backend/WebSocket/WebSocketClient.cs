using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using PimDeWitte.UnityMainThreadDispatcher;
using GeographicLib;

public class WebSocketClient : MonoBehaviour
{
    private WebSocket ws;
    private AstronautInstance astroInstance;
    private WebsocketDataHandler dataHandler;
    private string webSocketUrl;

    private void Start()
    {
        astroInstance = GetComponent<AstronautInstance>();
        dataHandler = GetComponent<WebsocketDataHandler>();
    }

    public void Disconnect()
    {
        try
        {
            if (ws != null && ws.IsAlive)
            {
                #if !UNITY_WEBGL
                    dataHandler.SendKill();
                    ws.Close();
                #endif
            }
        }
        catch (Exception ex)
        {
            // Handle or log the exception
            //Debug.LogError("Error occurred in ReConnect method: " + ex.Message);
        }

    }

    [ContextMenu("func ReConnect")]
    public bool ReConnect()
    {
        try
        {
            if (ws != null && ws.IsAlive)
            {
                #if !UNITY_WEBGL
                    dataHandler.SendKill();
                    ws.Close();
                #endif
            }

            #if !UNITY_WEBGL
                ws = new WebSocket(webSocketUrl);
                ws.OnMessage += OnWebSocketMessage;
                ws.Connect();
                return ws.IsAlive;
            #endif

            return true;
        }
        catch (Exception ex)
        {
            // Handle or log the exception
            //Debug.LogError("Error occurred in ReConnect method: " + ex.Message);
            return false;
        }
    }

    public bool ReConnect(string connectionString)
    {
        try
        {
            if (ws != null && ws.IsAlive)
            {
                #if !UNITY_WEBGL
                    dataHandler.SendKill();
                    ws.Close();
                #endif
            }

            webSocketUrl = connectionString;
            #if !UNITY_WEBGL
                ws = new WebSocket(webSocketUrl);
                ws.OnMessage += OnWebSocketMessage;
                ws.Connect();
                return ws.IsAlive;
            #endif

            return true;
        }
        catch (Exception ex)
        {
            // Handle or log the exception
            //Debug.LogError("Error occurred in ReConnect method: " + ex.Message);
            return false;
        }
    }


    public bool ReConnect(string connectionString, string color, string name, int _id)
    {
        try
        {
            
            if (ws != null && ws.IsAlive)
            {
                #if !UNITY_WEBGL
                    dataHandler.SendKill();
                    ws.Close();
                #endif
            }

            webSocketUrl = connectionString;
            #if !UNITY_WEBGL
                ws = new WebSocket(webSocketUrl);
                ws.OnMessage += OnWebSocketMessage;
                ws.Connect();

                dataHandler.SendInitialData(color, name, _id);
                return ws.IsAlive;
            #endif
            return true;
        }
        catch (Exception ex)
        {
            // Handle or log the exception
            //Debug.LogError("Error occurred in ReConnect method: " + ex.Message);
            return false;
        }
    }

    private void OnDestroy()
    {
        if (ws != null && ws.IsAlive)
        {
            dataHandler.SendKill();
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
        Debug.Log(jsonData);
        // Deserialize the JSON into JsonMessage class
        JsonMessage jsonMessage = JsonUtility.FromJson<JsonMessage>(jsonData);

        // Determine the type of data
        string messageType = jsonMessage.type;
        string messageUse = jsonMessage.use;
        int messageID = jsonMessage.id;

        Debug.Log(jsonMessage.type);
        Debug.Log(jsonMessage.use);

        // Ignore connection that is not initial or does not match astronaut's ID or not -1
        if (messageType != "INITIAL" && messageID != AstronautInstance.User.id && messageID != -1)
        {
            return;
        }

        switch (messageType.ToUpper())
        {
            case "INITIAL":
                InitialData initialData = JsonUtility.FromJson<InitialData>(jsonData);
                dataHandler.HandleInitialData(initialData, "");
                break;
            case "MESSAGING":
                MessagingData messageData = JsonUtility.FromJson<MessagingData>(jsonData);
                dataHandler.HandleMessagingData(messageData.data, messageUse);
                break;
            case "VITALS":
                VitalsData vitalsData = JsonUtility.FromJson<VitalsData>(jsonData);
                dataHandler.HandleVitalsData(vitalsData.data, messageUse);
                break;
            case "GEOSAMPLES":
                GeosamplesData geoData = JsonUtility.FromJson<GeosamplesData>(jsonData);
                dataHandler.HandleGeosamplesData(geoData.data, geoData.zones, messageUse);
                break;
            case "WAYPOINTS":
                WaypointsData waypointsData = JsonUtility.FromJson<WaypointsData>(jsonData);
                dataHandler.HandleWaypointsData(waypointsData.data, messageUse);
                break;
            case "TASKLIST":
                TaskListData taskListData = JsonUtility.FromJson<TaskListData>(jsonData);
                dataHandler.HandleTaskListData(taskListData.data, messageUse);
                break;
            case "ALERTS":
                AlertsData alertsData = JsonUtility.FromJson<AlertsData>(jsonData);
                dataHandler.HandleAlertsData(alertsData.data, messageUse);
                break;
            case "ALLBREADCRUMBS":
                AllBreadCrumbsData breadcrumbsData = JsonUtility.FromJson<AllBreadCrumbsData>(jsonData);
                dataHandler.HandleAllBreadCrumbsData(breadcrumbsData.data, messageUse);
                break;
            case "LOCATION":
                LocationData locationData = JsonUtility.FromJson<LocationData>(jsonData);
                dataHandler.HandleLocationData(locationData.data, messageUse);
                break;
            case "MULTIPLAYER":
                MultiplayerData multiData = JsonUtility.FromJson<MultiplayerData>(jsonData);
                dataHandler.HandleMultiplayerData(multiData.data, messageUse, multiData.id, multiData.dataToChange);
                break;
            case "NAVIGATION":
                NavigationData navData = JsonUtility.FromJson<NavigationData>(jsonData);
                dataHandler.HandleNavData(navData.location, navData.use);
                break;
            case "PICTURE":
                PicturesData picData = JsonUtility.FromJson<PicturesData>(jsonData);
                dataHandler.HandlePicData(picData.data.binary_img, picData.data.title, picData.use);
                break;
            case "HIGHLIGHT":
                HighlightData highData = JsonUtility.FromJson<HighlightData>(jsonData);
                dataHandler.HandleHighlightData(highData.button_id, highData.use);
                break;
            case "AUDIO":
                AudioData audioData = JsonUtility.FromJson<AudioData>(jsonData);
                dataHandler.HandleAudioData(audioData.data, audioData.use);
                break;
            case "AUDIO_PROCESSED":
                AudioData audioData1 = JsonUtility.FromJson<AudioData>(jsonData);
                dataHandler.HandleOrocessedAudioData(audioData1.data, audioData1.use);
                break;
            case "UIAIMAGE_PROCESSED":
                UIAData uiaData= JsonUtility.FromJson<UIAData>(jsonData);
                dataHandler.HandleUIAData(uiaData.data, uiaData.use);
                break;
            case "GEOSAMPLEIMAGE_PROCESSED":
                GeosampleData _geoData = JsonUtility.FromJson<GeosampleData>(jsonData);
                dataHandler.HandleGeosampleData(_geoData.data, _geoData.use);
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
    public string use;
    public string color; // Hex values as a string. Ex: #223344
    public string name; 
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
    public GeosampleZones zones;
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
public class PicturesData
{
    public int id;
    public string type;
    public string use;
    public PictureData data;
}

[Serializable]
public class PictureData
{
    public string binary_img;
    public string title;
}

[Serializable]
public class HighlightData
{
    public int id;
    public string type;
    public string use;
    public int button_id;
}

[Serializable]
public class KillData
{
    public int id;
    public string type;
}

[Serializable]
public class VegaAudio
{
    public string base_64_audio;
    public string text_from_VEGA;
    public bool classify;

    public VegaAudio(string a, string s, bool c)
    {
        base_64_audio = a;
        text_from_VEGA = s;
        classify = c;
    }
}

[Serializable]
public class AudioData
{
    public int id;
    public string type;
    public string use;
    public VegaAudio data;
}


[Serializable]
public class UIAData
{
    public int id;
    public string type;
    public string use;
    public UIAImage data;
}

[Serializable]
public class UIAImage
{
    public string base_64_image;
    public string[] points;
    public string position;
    public string rotation;

    public UIAImage(string a, string[] s, string p, string r)
    {
        base_64_image = a;
        points = s;
        position = p;
        rotation = r;
    }

}

[Serializable]
public class GeosampleImage
{
    public string base_64_image;
    public string[] points;
    public string position;
    public string rotation;
    public string color;
    public string description;
    public string shape;
    public string roughness;

    public GeosampleImage(string a, string[] s, string p, string r, string color_in, string description_in, string shape_in, string roughness_in)
    {
        base_64_image = a;
        points = s;
        position = p;
        rotation = r;
        color = color_in;
        description = description_in;
        shape = shape_in;
        roughness = roughness_in;
    }
}

[Serializable]
public class GeosampleData
{
    public int id;
    public string type;
    public string use;
    public GeosampleImage data;
}