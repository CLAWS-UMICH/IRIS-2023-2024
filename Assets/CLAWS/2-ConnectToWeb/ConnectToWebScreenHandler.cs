using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class ConnectToWebScreenHandler : MonoBehaviour
{
    public NonNativeKeyboard nonNativeKeyboard;
    GameObject connectScreen;
    GameObject disconnectScreen;
    WebSocketClient controller;
    TMP_InputField connectionText;
    TextMeshPro disconnectText;
    bool connected;
    string currentConnectionString;

    // Start is called before the first frame update
    void Start()
    {
        connectScreen = transform.Find("ConnectToWeb").gameObject;
        disconnectScreen = transform.Find("DisconnectScreen").gameObject;
        connectionText = connectScreen.transform.Find("NameField").transform.Find("TextField").transform.Find("InputField (TMP)").GetComponent<TMP_InputField>();
        disconnectText = disconnectScreen.transform.Find("NameField").transform.Find("NameText").GetComponent<TextMeshPro>();
        controller = GameObject.Find("Controller").GetComponent<WebSocketClient>();

        connectScreen.SetActive(false);
        disconnectScreen.SetActive(false);

        connected = false;
        currentConnectionString = "";
        connectionText.text = "";
        disconnectText.text = "";

        nonNativeKeyboard.OnTextUpdated += OnTextUpdated;

    }

    public void CloseConnectDisconnectScreen()
    {
        connectScreen.SetActive(false);
        disconnectScreen.SetActive(false);

        // Change Screen
    }

    public void OpenConnectDisconnectScreen()
    {
        if (connected)
        {
            connectScreen.SetActive(false);
            disconnectScreen.SetActive(true);
        } else
        {
            currentConnectionString = "";
            connectionText.text = "";
            disconnectText.text = "";
            connectScreen.SetActive(true);
            disconnectScreen.SetActive(false);
        }
    }

    public void ConnectToWebSocket()
    {
        controller.ReConnect(connectionText.text);
        connected = true;
        disconnectText.text = connectionText.text;

        CloseConnectDisconnectScreen();
    }

    public void DisconnectWebSocket()
    {
        controller.Disconnect();
        connected = false;

        OpenConnectDisconnectScreen();
    }

    private void OnTextUpdated(string text)
    {
        // Handle the updated text, you can use this to update your TextMeshPro text in real-time
        UpdateTextMeshProText(text);
    }

    private void UpdateTextMeshProText(string newText)
    {
        // Update the TextMeshPro text with the entered text
        connectionText.text = newText;
    }

}
