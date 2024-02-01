using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class SensorReceiver : MonoBehaviour
{
    UdpClient udpClient;
    int port = 12345; // Match the port number in Arduino code

    void Start()
    {
        udpClient = new UdpClient(port);
        udpClient.BeginReceive(ReceiveData, null);
    }

    void ReceiveData(IAsyncResult result)
    {
        IPEndPoint source = new IPEndPoint(0, 0);
        byte[] received = udpClient.EndReceive(result, ref source);

        // Parse received data (assuming floats for XYZ, roll, pitch, yaw)
        float x = BitConverter.ToSingle(received, 0);

        Debug.Log(x);

        // Do something with the sensor data (e.g., update HoloLens objects)
        // ...

        // Continue listening for more data
        udpClient.BeginReceive(ReceiveData, null);
    }

    void OnDestroy()
    {
        udpClient.Close();
    }
}