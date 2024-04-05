using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class ConnectToWebScreenHandler : MonoBehaviour
{

    GameObject connectScreen;
    GameObject disconnectScreen;

    TMP_InputField connectionLinkText;
    TMP_InputField nameText;
    TextMeshPro disconnectText;

    bool connected;

    WebSocketClient controller;

    string hex;

    // Start is called before the first frame update
    void Start()
    {
        connectScreen = transform.Find("ConnectToWeb").gameObject;
        disconnectScreen = transform.Find("DisconnectScreen").gameObject;
        connectionLinkText = connectScreen.transform.Find("LinkField").transform.Find("TextField").transform.Find("InputField (TMP)").GetComponent<TMP_InputField>();
        nameText = connectScreen.transform.Find("NameField").transform.Find("TextField").transform.Find("InputField (TMP)").GetComponent<TMP_InputField>();
        disconnectText = disconnectScreen.transform.Find("NameField").transform.Find("NameText").GetComponent<TextMeshPro>();
        controller = GameObject.Find("Controller").GetComponent<WebSocketClient>();

        connectScreen.SetActive(false);
        disconnectScreen.SetActive(false);

        connected = false;
        connectionLinkText.text = GameObject.Find("Controller").GetComponent<MainConnections>().getWebsocketURL();
        Debug.Log("TEsT: " + connectionLinkText.text);
        hex = "";
        nameText.text = "";
        disconnectText.text = "";

    }

    public void CloseConnectDisconnectScreen()
    {
        connectScreen.SetActive(false);
        disconnectScreen.SetActive(false);
    }

    public void OpenConnectDisconnectScreen()
    {
        if (connected)
        {
            connectScreen.SetActive(false);
            disconnectScreen.SetActive(true);
        } else
        {
            hex = "";
            //connectionLinkText.text = "";
            nameText.text = "";
            disconnectText.text = "";
            connectScreen.SetActive(true);
            disconnectScreen.SetActive(false);
        }
    }

    public void ConnectToWebSocket()
    {
        if (hex != "" && nameText.text != "")
        {
            GameObject.Find("Controller").GetComponent<MainConnections>().ConnectToWebsocket(connectionLinkText.text, hex, nameText.text);
        } else
        {
            GameObject.Find("Controller").GetComponent<MainConnections>().ConnectToWebsocket(connectionLinkText.text, "", "");
        }

        CloseConnectDisconnectScreen();
    }

    public void DisconnectWebSocket()
    {
        controller.Disconnect();
        connected = false;

        OpenConnectDisconnectScreen();
    }

    public void SelectColor(string _hex)
    {
        hex = _hex;
    }

}
