using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.Networking;


public class VitalsWebsocketConnector : MonoBehaviour
{
    private WebSocket ws;
    private WebsocketDataHandler websocketDataHandler;
    private VitalsJSONClass js;
    private string serverUrl = "http://your-server-url";

    private void Start()
    {
        ws = new WebSocket("ws://localhost:8000/frontend");
        ws.Connect();
        websocketDataHandler = GetComponent<WebsocketDataHandler>();
        websocketDataHandler.HandleVitalsData(new Vitals(), "PUT");
    }

    IEnumerator CheckServerStatus()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(serverUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                ws.Send("..");
            }
            else
            {
                Debug.LogError("Error checking server status: " + www.error);
            }
        }
    }

    void CallHandleVitalsData()
    {
        // Example: Call HandleVitalsData with use = "GET"
        websocketDataHandler.HandleVitalsData(new Vitals(), "GET");
    }

    private void OnDestroy()
    {
        if (ws.IsAlive || ws != null)
        {
            ws.Close();
        }
    }
}
