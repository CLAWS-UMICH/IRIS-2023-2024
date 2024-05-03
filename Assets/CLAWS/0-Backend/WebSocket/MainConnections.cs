using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainConnections : MonoBehaviour
{
    [SerializeField] string webSocketUrl;
    [SerializeField] bool autoConnectWebSocket = false;
    [SerializeField] string tssUrl;
    [SerializeField] bool autoConnectTSS = false;
    [SerializeField] int id;

    private bool websocketConnected;
    private bool TSSConnected;

    // Start is called before the first frame update
    void Start()
    {
        websocketConnected = false;
        TSSConnected = false;

        if (autoConnectTSS)
        {
            ConnectTSS(tssUrl);
            AstronautInstance.User.id = id;
        }

        if (autoConnectWebSocket)
        {
            StartCoroutine(_ConnectWebSocket(webSocketUrl, "", "Brian2", 1, id - 1));
        }
    }

    private bool ConnectWebsocket(string connectionString, string color, string name, int num, int _id)
    {
        if (num == 0)
        {
            return transform.GetComponent<WebSocketClient>().ReConnect(connectionString);
        } else
        {
            return transform.GetComponent<WebSocketClient>().ReConnect(connectionString, color, name, _id);
        }
    }

    private void ConnectTSS(string url)
    {
        transform.GetComponent<TSScConnection>().TSSConnect(url);
    }

    IEnumerator _ConnectWebSocket(string connectionString, string color, string name, int num, int _id)
    {
        while (!ConnectWebsocket(connectionString, color, name, num, _id))
        {
            yield return new WaitForSeconds(5f);

            Debug.Log("WebSocket: Connection Failed. Trying again in 5 seconds.");
            websocketConnected = false;
        }

        websocketConnected = true;

        Debug.Log("WebSocket: Connection Successful");
    }

    public void ConnectToWebsocket(string connectionString, string color, string name, int _id)
    {
        StartCoroutine(_ConnectWebSocket(connectionString, color, name, 1, _id));
    }

    public void ConnectToTSS()
    {
        ConnectTSS(tssUrl);
    }

    public string getWebsocketURL()
    {
        return webSocketUrl;
    }



}
