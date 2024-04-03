using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TestImage : MonoBehaviour
{
    private Subscription<NewPicEvent> newPicEvent;
    private TextMeshPro titleText;  //variable to hold the title 
    private GameObject quad; // Variable to hold the quad GameObject
    private GameObject screen;
    private bool isScreenVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        newPicEvent = EventBus.Subscribe<NewPicEvent>(onNewPic);
        screen = transform.Find("BackPlate").gameObject;
        screen.SetActive(false);
        titleText = screen.transform.Find("Title").GetComponent<TextMeshPro>();
        quad = screen.transform.Find("Quad").gameObject;
    }

    public void ToggleScreenVisibility()
    {
        isScreenVisible = !isScreenVisible;
        if (isScreenVisible)
        {
            // Set the starting position for the screen when it is set to true
            screen.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        // Set the screen (quad GameObject) active or inactive based on the visibility flag
            screen.SetActive(isScreenVisible);
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
        ToggleScreenVisibility();
    }
}