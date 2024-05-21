using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TestMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        double easting = 298329.75;
        double northing = 3272418.00;
        int zoneNum = 15;
        char zoneLetter = 'R';

        var result = CoordinateConverter.ToLatLon(easting, northing, zoneNum, zoneLetter, northern: null, strict: true);
        Debug.Log($"Latitude: {Math.Round(result.latitude, 7)}");
        Debug.Log($"Long: {Math.Round(result.longitude, 7)}");
    }

}
