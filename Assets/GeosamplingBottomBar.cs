using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosamplingBottomBar : MonoBehaviour
{
    public List<GameObject> Children = new List<GameObject>();

    private void Start()
    {
        EventBus.Subscribe<GeosampleModeStartedEvent>(OnModeStart);
        EventBus.Subscribe<GeosampleModeEndedEvent>(OnModeEnd);

        if (GeosamplingManager.GeosamplingMode == true)
        {
            OnModeStart(new());
        }
        else
        {
            OnModeEnd(new());
        }
    }

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

    private void OnModeStart(GeosampleModeStartedEvent e)
    {
        foreach (var child in Children)
        {
            child.SetActive(true);
        }
    }

    private void OnModeEnd(GeosampleModeEndedEvent e)
    {
        foreach (var child in Children)
        {
            child.SetActive(false);
        }
    }
}
