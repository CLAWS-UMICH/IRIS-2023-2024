using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelRoute : MonoBehaviour
{
    public GameObject cancelRouteBtn;    

    void Start()
    {
        if (AstronautInstance.User.currently_navigating)
        {
            cancelRouteBtn.SetActive(true);
            Debug.Log("navigating, button should should up");
        }
        else
        {
            cancelRouteBtn.SetActive(false);
            Debug.Log("Not navigating, button shouldn't should up");
        }
    }

    public void cancelRoute()
    {
        AstronautInstance.User.currently_navigating = false;
        cancelRouteBtn.SetActive(false);
    }
}
