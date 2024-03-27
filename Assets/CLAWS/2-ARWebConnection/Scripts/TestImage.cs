using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestImage : MonoBehaviour
{
    private Subscription<NewPicEvent> newPicEvent;
    private TextMeshPro titleText;  //variable to hold the title 
    private GameObject quad; // Variable to hold the quad GameObject

    // Start is called before the first frame update
    void Start()
    {
        newPicEvent = EventBus.Subscribe<NewPicEvent>(onNewPic);
        titleText = transform.Find("Title").GetComponent<TextMeshPro>();
        quad = transform.Find("Quad").gameObject;
    }

    public void onNewPic(NewPicEvent e)
    {
        string jsonPayload = e.image;
        string jsonTitle = e.title;
        Debug.Log("Received JSON payload: " + jsonPayload); // Add this line to log the received JSON

        // Decode base64 and display image on quad
        byte[] imageBytes = Convert.FromBase64String(jsonPayload);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(imageBytes);

        // Create a material and assign the texture to it
        Material quadMaterial = new Material(Shader.Find("New Material"));
        quadMaterial.mainTexture = texture;

        // Assign the material to the quad
        quad.GetComponent<Renderer>().material = quadMaterial;

        //Assign title to title (im assuming the json is sending the string as is)
        titleText.text = jsonTitle;
    }
}
