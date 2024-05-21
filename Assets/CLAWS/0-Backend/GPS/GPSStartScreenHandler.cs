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
            var latLong = CoordinateConverter.ToLatLon(AstronautInstance.User.imu.imu.eva1.posx, AstronautInstance.User.imu.imu.eva1.posy, 15, 'R', northern: null, strict: true);
            loc.latitude = latLong.latitude;
            loc.longitude = latLong.longitude;
        } 
        else
        {
            var latLong = CoordinateConverter.ToLatLon(AstronautInstance.User.imu.imu.eva2.posx, AstronautInstance.User.imu.imu.eva2.posy, 15, 'R', northern: null, strict: true);
            loc.latitude = latLong.latitude;
            loc.longitude = latLong.longitude;
        }

        Location loc1 = new Location(29.56459834, -95.08144150);

        resetGPSSpawn.Reset(loc1, GPSUtils.GPSCoordsToAppPosition(loc));

    }

    public void TruckLocation()
    {
        Location loc = new Location(29.56459834, -95.08144150); // Location of north of the truck

        resetGPSSpawn.Reset(loc, new Vector3(0f, 0f, 0f));
    }
}
