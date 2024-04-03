using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject playerCam;
    public float offset = 0f;
    float zeroValue = 0f;

    private UdpClient serverSocket;
    void Start()
    {
        /*try
        {
            // Create a UDP socket
            serverSocket = new UdpClient(8888);
            Debug.Log("UDP socket created, waiting for messages...");
            // Start listening in a separate thread
            StartListening();
        }
        catch (Exception e)
        {
            Debug.LogError("Error creating UDP socket: " + e.Message);
        }*/
    }
    private void StartListening()
    {
        try
        {
            // Begin receiving messages asynchronously
            serverSocket.BeginReceive(ReceiveCallback, null);
        }
        catch (Exception e)
        {
            Debug.LogError("Error starting to listen for messages: " +e.Message);
        }
    }
    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            // Get the client?s message and endpoint
            IPEndPoint clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receivedData = serverSocket.EndReceive(result, ref clientEndpoint);
            // Convert the byte array to a string
            string message = Encoding.ASCII.GetString(receivedData);
            // Print the received message
            offset = (float)Convert.ToDouble(message) + zeroValue;
            Debug.Log("Received message: " +message);
            // Continue listening for more messages
            StartListening();
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving message: " +e.Message);
        }
    }
    void OnApplicationQuit()
    {
        // Close the UDP socket when the application quits
        if (serverSocket != null)
        {
            serverSocket.Close();
            Debug.Log("UDP socket closed.");
        }
    }
    [ContextMenu("COR Sync")]
    public void CorSync()
    {
        zeroValue = offset * -1;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = playerCam.transform.position;
        gameObject.transform.eulerAngles = new Vector3(
            gameObject.transform.eulerAngles.x,
            offset,
            gameObject.transform.eulerAngles.z
        );
    }
}
