using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map3DController : MonoBehaviour
{
    GameObject player;
    GameObject mapParent;
    GameObject map;
    static GameObject waypoints;
    GameObject breadcrumbs;
    GameObject bigMap;

    [SerializeField] GameObject wayPrefab;

    static Vector3 bigMapScale;
    static Vector3 smallMapScale;

    [SerializeField] float yOffset = -0.1f;
    [SerializeField] float zOffset = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Main Camera").gameObject;
        mapParent = transform.Find("MapParent_3D").gameObject;
        map = mapParent.transform.Find("Map_3D").gameObject;
        waypoints = mapParent.transform.Find("WaypointParent_3D").gameObject;
        breadcrumbs = mapParent.transform.Find("BreadParent_3D").gameObject;
        bigMap = GameObject.Find("Big").gameObject;

        map.SetActive(true);
        waypoints.SetActive(true);
        breadcrumbs.SetActive(true);

        Close3DMap();

        bigMapScale = bigMap.transform.localScale;
        smallMapScale = map.transform.localScale;

    }

    public void Open3DMap()
    {
        // Calculate the spawn position based on player's position and view direction
        Vector3 spawnPosition = player.transform.position + player.transform.forward * zOffset + player.transform.up * yOffset;

        spawnPosition.y = player.transform.position.y + yOffset;

        // Set the position of mapParent
        mapParent.transform.position = spawnPosition;

        mapParent.SetActive(true);
    }

    public void Close3DMap()
    {
        mapParent.SetActive(false);
    }

    public static Vector3 TranslatePosition(Vector3 originalPos)
    {
        // Calculate the conversion factors for each axis
        float xConversionFactor = smallMapScale.x / bigMapScale.x;
        //float yConversionFactor = smallMapScale.y / bigMapScale.y;
        float zConversionFactor = smallMapScale.y / bigMapScale.y;

        // Apply the conversion factors to translate the position to new pos
        Vector3 newPos = new Vector3(
            originalPos.x * xConversionFactor,
            //originalPos.y * yConversionFactor,
            0,
            originalPos.z * zConversionFactor
        );

        return newPos;
    }

    public static GameObject SpawnWaypoint(GameObject prefab, Vector3 location)
    {
        GameObject newWaypoint = Instantiate(prefab, TranslatePosition(location), Quaternion.identity);
        newWaypoint.transform.position += new Vector3(0f, 0.01625f, 0f);

        // Calculate the conversion factors for each axis
        float conversionFactorScale = smallMapScale.x / bigMapScale.x * 7;

        newWaypoint.transform.localScale *= conversionFactorScale;

        // Set the parent of the instantiated waypoint to the waypoints object
        newWaypoint.transform.parent = waypoints.transform;

        return newWaypoint;
    }
}
