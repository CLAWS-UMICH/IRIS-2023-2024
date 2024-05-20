using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSStartScreenHandler : MonoBehaviour
{
    GameObject optionScreen;
    ResetGPSSpawn resetGPSSpawn;

    // Start is called before the first frame update
    void Start()
    {
        resetGPSSpawn = GameObject.Find("Controller").gameObject.GetComponent<ResetGPSSpawn>();
        optionScreen = gameObject.transform.Find("ResetOptionScreen").gameObject;
        optionScreen.SetActive(false);
    }

    public void OpenOptionScreen()
    {
        optionScreen.SetActive(true);
    }

    public void CloseOptionScreen()
    {
        optionScreen.SetActive(false);
    }

    public void TSSLocation()
    {
        Location loc = new Location();
        if (AstronautInstance.User.id == 0)
        {
            loc.latitude = AstronautInstance.User.imu.imu.eva1.posx;
            loc.longitude = AstronautInstance.User.imu.imu.eva1.posy;
        } 
        else
        {
            loc.latitude = AstronautInstance.User.imu.imu.eva2.posx;
            loc.longitude = AstronautInstance.User.imu.imu.eva2.posy;
        }

        resetGPSSpawn.Reset(loc);
    }

    public void TruckLocation()
    {
        Location loc = new Location(29.56459834, -95.08144150); // Location of norht of the truck

        resetGPSSpawn.Reset(loc);
    }
}
