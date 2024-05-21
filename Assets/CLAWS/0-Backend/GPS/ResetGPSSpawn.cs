using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGPSSpawn : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Main Camera").gameObject;
    }

    public void Reset(Location loc, Vector3 pos)
    {
        player.transform.position = pos;
        GPSUtils.ChangeOriginGPSCoords(loc);
        //transform.parent.transform.Find("AllScreens").Find("Navigation").Find("WaypointController").GetComponent<WaypointsController>().UpdateLocationsOfWaypoints();
    }

}
