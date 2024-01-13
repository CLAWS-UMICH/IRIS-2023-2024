using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.UI;
using WebSocketSharp;

public class TestImage : MonoBehaviour
{

    private Subscription<NewPicEvent> newPicEvent;
    RawImage displayImage;

    public GameObject quad;

    // Start is called before the first frame update
    void Start()
    {
        newPicEvent = EventBus.Subscribe<NewPicEvent>(onNewPic);
    }

    public void onNewPic(NewPicEvent e)
    {
        string jsonPayload = e.image;
        Debug.Log("Received JSON payload: " + jsonPayload); // Add this line to log the received JSON

        // Decode base64 and display image on quad
        byte[] imageBytes = Convert.FromBase64String(jsonPayload);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(imageBytes);

        // Create a material and assign the texture to it
        Material quadMaterial = new Material(Shader.Find("Standard"));
        quadMaterial.mainTexture = texture;

        // Assign the material to the quad
        quad.GetComponent<Renderer>().material = quadMaterial;
    }
}
