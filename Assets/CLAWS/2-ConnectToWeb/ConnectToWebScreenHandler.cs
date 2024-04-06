using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class ConnectToWebScreenHandler : MonoBehaviour
{

    GameObject connectScreen;
    GameObject disconnectScreen;
    GameObject chooseScreen;

    TMP_InputField connectionLinkText;
    TMP_InputField nameText;
    TextMeshPro disconnectText;

    bool connected;
    bool openWeb = false;
    bool openTSS = false;

    WebSocketClient controller;

    string hex;

    // Start is called before the first frame update
    void Start()
    {
        connectScreen = transform.Find("ConnectToWeb").gameObject;
        disconnectScreen = transform.Find("DisconnectScreen").gameObject;
        chooseScreen = transform.Find("ConnectionChooseScreen").gameObject;
        connectionLinkText = connectScreen.transform.Find("LinkField").transform.Find("TextField").transform.Find("InputField (TMP)").GetComponent<TMP_InputField>();
        nameText = connectScreen.transform.Find("NameField").transform.Find("TextField").transform.Find("InputField (TMP)").GetComponent<TMP_InputField>();
        disconnectText = disconnectScreen.transform.Find("NameField").transform.Find("NameText").GetComponent<TextMeshPro>();
        controller = GameObject.Find("Controller").GetComponent<WebSocketClient>();

        connectScreen.SetActive(false);
        disconnectScreen.SetActive(false);
        chooseScreen.SetActive(false);

        connected = false;
        connectionLinkText.text = GameObject.Find("Controller").GetComponent<MainConnections>().getWebsocketURL();
        hex = "";
        nameText.text = "";
        disconnectText.text = "";



    }

    public void CloseConnectDisconnectScreen()
    {
        openWeb = false;
        openTSS = false;
        connectScreen.SetActive(false);
        disconnectScreen.SetActive(false);
        chooseScreen.SetActive(false); 
    }

    public void OpenConnectDisconnectScreen()
    {
        if (openWeb)
        {
            if (connected)
            {
                connectScreen.SetActive(false);
                disconnectScreen.SetActive(true);
            }
            else
            {
                hex = "";
                //connectionLinkText.text = "";
                nameText.text = "";
                disconnectText.text = "";
                connectScreen.SetActive(true);
                disconnectScreen.SetActive(false);
            }
        } else if (openTSS)
        {
            // TODO Connect TSS
            GameObject.Find("Controller").GetComponent<MainConnections>().ConnectToTSS();
            CloseConnectDisconnectScreen();
        }
    }

    public void OpenWebScreen()
    {
        openWeb = true;
        openTSS = false;
        OpenConnectDisconnectScreen();
    }

    public void OpenTSSScreen()
    {
        openTSS = true;
        openWeb = false;
        OpenConnectDisconnectScreen();
    }

    public void OpenChoiceScreen()
    {
        CloseConnectDisconnectScreen();
        chooseScreen.SetActive(true);
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
