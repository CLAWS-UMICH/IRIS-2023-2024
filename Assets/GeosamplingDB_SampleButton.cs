using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class GeosamplingDB_SampleButton : MonoBehaviour
{
    [SerializeField] TextMeshPro SampleLabel;
    [SerializeField] TextMeshPro SampleName;
    [SerializeField] GameObject Star;

    Geosample geosample;

    public void SetSample(Geosample g)
    {
        geosample = g;

        SampleLabel.text = g.zone_id.ToString() + g.geosample_id.ToString();
        SampleName.text = g.geosample_id.ToString("D5");
        Star.SetActive(g.starred);
    }

    public void OnClick()
    {
        GeosamplingDB_Manager.Instance.OnSampleClicked(geosample);
    }
}
