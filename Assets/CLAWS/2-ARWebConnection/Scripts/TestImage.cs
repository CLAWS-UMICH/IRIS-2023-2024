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
    private Vector3 defaultScale;
    private bool isScreenVisible;
    [SerializeField] Material quadMaterial;

    // Start is called before the first frame update
    void Start()
    {
        isScreenVisible = false;
        newPicEvent = EventBus.Subscribe<NewPicEvent>(onNewPic);
        screen = transform.Find("BackPlate").gameObject;
        screen.SetActive(false);
        titleText = screen.transform.Find("Title").GetComponent<TextMeshPro>();
        quad = screen.transform.Find("Quad").gameObject;
        defaultScale = quad.transform.localScale;
    }

    public void TurnOn()
    {
        isScreenVisible = true;
        // Set the starting position for the screen when it is set to true
        screen.transform.localPosition = new Vector3(0f, 0f, 0f);
        quad.transform.localScale = defaultScale;
        
        // Set the screen (quad GameObject) active or inactive based on the visibility flag
        screen.SetActive(true);
    }

    public void TurnOff()
    {
        isScreenVisible = false;

        // Set the screen (quad GameObject) active or inactive based on the visibility flag
        screen.SetActive(false);
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

        quadMaterial.mainTexture = texture;

        // Assign the material to the quad
        quad.GetComponent<Renderer>().material = quadMaterial;

        //Assign title to title (im assuming the json is sending the string as is)
        titleText.text = jsonTitle;
        TurnOn();
    }
}