using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosamplingBottomBar : MonoBehaviour
{

    public void OnAddSamplePressed()
    {
        GeosamplingManager.CreateGeosample();
    }

    public void OnViewSamplesPressed()
    {
        Debug.Log("Not implemented yet!");
    }

    public void OnExitPressed()
    {
        GeosamplingManager.EndGeosamplingMode();
    }
}
