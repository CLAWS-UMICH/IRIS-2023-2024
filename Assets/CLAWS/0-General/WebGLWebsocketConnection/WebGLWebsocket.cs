using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WebGLWebsocket : MonoBehaviour
{
    private GameObject openWebButton;
    private GameObject openedParent;
    public TMP_InputField inputField;
    private GameObject controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Controller");
        openWebButton = GameObject.Find("OpenWebButton");
        openedParent = GameObject.Find("OpenedWebsocket");
        CloseTextBox();
    }

    public void OpenTextBox()
    {
        openedParent.SetActive(true);
        openWebButton.SetActive(false);
    }

    public void CloseTextBox()
    {
        openedParent.SetActive(false);
        openWebButton.SetActive(true);
    }

    public void ConnectToWebsocketServer()
    {
        controller.GetComponent<WebSocketClient>().ReConnect(inputField.text);
        CloseTextBox();
    }
}
