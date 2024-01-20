using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class ConnectToWebScreenHandler : MonoBehaviour
{
    public NonNativeKeyboard nameKeyboard;
    public NonNativeKeyboard linkKeyboard;

    GameObject connectScreen;
    GameObject disconnectScreen;

    TMP_InputField connectionLinkText;
    TMP_InputField nameText;
    TextMeshPro disconnectText;

    bool connected;

    WebSocketClient controller;

    string currentKeyboard;
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
        connectionLinkText.text = "";
        hex = "";
        nameText.text = "";
        disconnectText.text = "";

        nameKeyboard.OnTextUpdated += OnTextUpdated;
        linkKeyboard.OnTextUpdated += OnTextUpdated;

    }

    public void ChangeKeyboardName()
    {
        currentKeyboard = "name";
    }

    public void ChangeKeyboardLink()
    {
        currentKeyboard = "link";
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
            hex = "";
            connectionLinkText.text = "";
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
            controller.ReConnect(connectionLinkText.text, hex, nameText.text);
        } else
        {
            controller.ReConnect(connectionLinkText.text);
        }
        connected = true;
        disconnectText.text = connectionLinkText.text;

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
        // Handle the updated text and the reference to the current keyboard
        UpdateTextMeshProText(text);
    }

    private void UpdateTextMeshProText(string newText)
    {
        // Update the TextMeshPro text of the current keyboard with the entered text
        if (currentKeyboard == "name")
        {
            // Update nameKeyboard's text
            nameText.text = newText;
        }
        else if (currentKeyboard == "link")
        {
            // Update linkKeyboard's text
            connectionLinkText.text = newText;
        }
    }

    public void SelectColor(string _hex)
    {
        Debug.Log(_hex);
        hex = _hex;
    }

}
