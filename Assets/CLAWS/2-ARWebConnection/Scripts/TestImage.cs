using System;
using UnityEngine;
using UnityEngine.UI;

public class TestImage : MonoBehaviour
{
    public Image john;
    public GameObject quad; // Variable to hold the quad GameObject

    // Start is called before the first frame update
    void Start()
    {
        
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
