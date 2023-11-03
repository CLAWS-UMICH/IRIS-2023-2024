using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGPSChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Location l = GPSUtils.AppPositionToGPSCoords(transform.position);
        Debug.Log("Lat: " + l.latitude + " Long: " + l.longitude);
    }
}
