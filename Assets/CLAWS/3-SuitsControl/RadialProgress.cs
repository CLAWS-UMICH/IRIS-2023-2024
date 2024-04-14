using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class RadialProgress : MonoBehaviour
{
    public GameObject SR; //will make private once full list of vitals, then  will make seperate objects for each and .find them
    public TextMeshPro ProgressIndicator;
    private Subscription<VitalsUpdatedEvent> vitalsRadialEvent;

    void Start()
    {
        //vitalsRadialEvent = EventBus.Subscribe<VitalsUpdatedEvent>(UpdateRadialProgress);
        SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 0f);
        SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", 0f);
    }

    public void UpdatePercent(float currentVal/*VitalsUpdatedEvent e*/)
    {
        float degrees = 360 - currentVal/*(float)e.vitals.oxy_percentage*/ * 3.6f;

        // Update the material's properties for the circle
        SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", degrees);

        // Display the percentage as text, if desired
        if (ProgressIndicator != null)
        {
            ProgressIndicator.text = currentVal + "%"/*e.vitals.oxy_pri_storage.ToString("F0")*/;
        }
    }

    public void UpdateHR(float currentVal/*VitalsUpdatedEvent e*/)
    {
        float normalizedValue = (currentVal - 50) / (160 - 50);

        // Convert the normalized value to degrees on a 360 scale
        float degrees = 360 * normalizedValue;

        // Update the material's properties for the circle
        SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", degrees);

        // Display the percentage as text, if desired
        if (ProgressIndicator != null)
        {
            ProgressIndicator.text = currentVal.ToString()/*e.vitals.oxy_pri_storage.ToString("F0")*/;
        }
    }

    public void UpdatePR(float currentVal/*VitalsUpdatedEvent e*/)
    {
        float normalizedValue = (currentVal - 600) / (3000 - 600);

        // Convert the normalized value to degrees on a 360 scale
        float degrees = normalizedValue * 360;

        // Update the material's properties for the circle
        SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", degrees);

        // Display the percentage as text, if desired
        if (ProgressIndicator != null)
        {
            ProgressIndicator.text = currentVal.ToString()/*e.vitals.oxy_pri_storage.ToString("F0")*/;
        }
    }
}

