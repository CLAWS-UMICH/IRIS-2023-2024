using System.Collections;
using System.Text.RegularExpressions;
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
    }

    void Update()
    {
        Match match = Regex.Match(ProgressIndicator.text, @"\d+");

        if (match.Success)
        {
            currentValue = float.Parse(match.Value);

            // Calculate rotation based on percentage of progress
            float percentage = 1.0f - (currentValue / 100.0f);
            rotate = Mathf.Lerp(0.0f, 360.0f, percentage);

            // Set the rotation in the material
            SR.GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", rotate);
        }
    }
}

