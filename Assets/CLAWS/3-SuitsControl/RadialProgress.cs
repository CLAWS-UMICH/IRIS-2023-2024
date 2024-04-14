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
        vitalsRadialEvent = EventBus.Subscribe<VitalsUpdatedEvent>(UpdateRadialProgress);
        SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 0f);
        SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", 0f);
    }

    void UpdateRadialProgress(VitalsUpdatedEvent e)
    {
        float degrees = 360 - (float)e.vitals.oxy_percentage * 3.6f;

        // Update the material's properties for the circle
        SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", degrees);

        // Display the percentage as text, if desired
        if (ProgressIndicator != null)
        {
            ProgressIndicator.text = e.vitals.oxy_pri_storage.ToString("F0") + "<size=75%>%</size>";
        }

    }
}

