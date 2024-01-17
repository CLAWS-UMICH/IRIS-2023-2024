using System.Collections;
using UnityEngine;
using TMPro;

public class RadialProgress : MonoBehaviour
{
    public GameObject SR;
    public TextMeshPro ProgressIndicator;
    public float currentValue;
    public float speed;
    float rotate;

    void Start()
    {
        SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 0.0f);
        SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", 360.0f);
        ProgressIndicator.transform.Find("ProgressIndicator").GetComponent<TextMeshPro>();
        currentValue = 100.0f;
        rotate = 360.0f;
    }

    void Update()
    {
        if (currentValue <= 100.0f)
        {
            currentValue += speed * Time.deltaTime;
            ProgressIndicator.text = ((int)currentValue).ToString() + "%";

            // Calculate rotation based on percentage of progress
            float percentage = 1.0f - (currentValue / 100.0f);
            rotate = Mathf.Lerp(0.0f, 360.0f, percentage);

            // Set the rotation in the material
            SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", rotate);
        }
        else
        {
            ProgressIndicator.text = "Done";
        }
    }
}

