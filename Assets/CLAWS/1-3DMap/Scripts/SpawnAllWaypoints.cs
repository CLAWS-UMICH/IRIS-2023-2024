using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAllWaypoints : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        prefab = prefab;

    }

    public void Spawn()
    {
        GameObject goA = Instantiate(prefab, GPSUtils.GPSCoordsToAppPosition(new Location(29.5648150, -95.0817410)), Quaternion.identity);
        goA.name = "Station A";
        GameObject goB = Instantiate(prefab, GPSUtils.GPSCoordsToAppPosition(new Location(29.5646824, -95.0811564)), Quaternion.identity);
        goB.name = "Station B";
        GameObject goC = Instantiate(prefab, GPSUtils.GPSCoordsToAppPosition(new Location(29.5650460, -95.0810944)), Quaternion.identity);
        goC.name = "Station C";
        GameObject goD = Instantiate(prefab, GPSUtils.GPSCoordsToAppPosition(new Location(29.5645430, -95.0816440)), Quaternion.identity);
        goD.name = "Station D";
        GameObject goE = Instantiate(prefab, GPSUtils.GPSCoordsToAppPosition(new Location(29.5648290, -95.0813750)), Quaternion.identity);
        goE.name = "Station E";
        GameObject goF = Instantiate(prefab, GPSUtils.GPSCoordsToAppPosition(new Location(29.5647012, -95.0813750)), Quaternion.identity);
        goF.name = "Station F";
        GameObject goG = Instantiate(prefab, GPSUtils.GPSCoordsToAppPosition(new Location(29.5651359, -95.0807408)), Quaternion.identity);
        goG.name = "Station G";
        GameObject goH = Instantiate(prefab, GPSUtils.GPSCoordsToAppPosition(new Location(29.5651465, -95.0814092)), Quaternion.identity);
        goH.name = "Station H";
        GameObject goI = Instantiate(prefab, GPSUtils.GPSCoordsToAppPosition(new Location(29.5648850, -95.0808360)), Quaternion.identity);
        goI.name = "Station I";
    }
}
