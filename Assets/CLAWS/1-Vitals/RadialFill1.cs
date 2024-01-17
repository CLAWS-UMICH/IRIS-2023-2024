using UnityEngine;

public class RadialFill1 : MonoBehaviour
{
    GameObject RingGO;
    SpriteMask Ring;
    public float fillSpeed = 1f; // Adjust the speed of the fill
    private float currentFillAmount = 0f;

    void Start()
    {
        RingGO = GameObject.Find("Ring");
        Ring = RingGO.GetComponent<SpriteMask>();
        // Assuming you have a Sprite Mask with a material that has a "_Range" property
    }

    void Update()
    {
        // Update the fill amount based on time or any other condition
        currentFillAmount += fillSpeed * Time.deltaTime;

        // Clamp the fill amount between 0 and 1
        currentFillAmount = Mathf.Clamp01(currentFillAmount);

        // Update the material's "_Range" property for the radial fill effect
        Ring.material.SetFloat("Custom Range", currentFillAmount);
    }
}
