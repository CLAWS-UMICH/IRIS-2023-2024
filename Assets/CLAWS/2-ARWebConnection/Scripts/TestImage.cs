using System;
using UnityEngine;
using UnityEngine.UI;

public class TestImage : MonoBehaviour
{
    private Subscription<NewPicEvent> newPicEvent;
    public GameObject screen;
    private GameObject backplate; // Variable to hold the backplate GameObject
    private GameObject quad; // Variable to hold the quad GameObject

    // Start is called before the first frame update
    void Start()
    {
        newPicEvent = EventBus.Subscribe<NewPicEvent>(onNewPic);
        screen = transform.Find("ARWebScreen").gameObject;
        backplate = screen.transform.Find("Backplate").gameObject;
        quad = backplate.transform.Find("Quad").gameObject;
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
