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

    [SerializeField] GameObject wayPrefab;

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

        map.SetActive(true);
        waypoints.SetActive(true);
        breadcrumbs.SetActive(true);

        Close3DMap();

    }

    public void Open3DMap()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.Navigation_3D));
        // Calculate the spawn position based on player's position and view direction
        Vector3 spawnPosition = player.transform.position + player.transform.forward * zOffset + player.transform.up * yOffset;

        spawnPosition.y = player.transform.position.y + yOffset;

        // Set the position of mapParent
        mapParent.transform.position = spawnPosition;

        mapParent.SetActive(true);
    }

    public void Close3DMap()
    {
        EventBus.Publish(new ScreenChangedEvent(Screens.Menu));
        mapParent.SetActive(false);
    }

    public static Vector3 TranslatePosition(Vector3 originalPos)
    {
        // Calculate the conversion factors for each axis
        float xConversionFactor = 0.0001258214f;

        //float yConversionFactor = smallMapScale.y / bigMapScale.y;
        float zConversionFactor = 0.0001258214f;
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
        newWaypoint.transform.position += new Vector3(0.071f, 0.03f, 0.102f);
        
        newWaypoint.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        // Set the parent of the instantiated waypoint to the waypoints object
        newWaypoint.transform.parent = waypoints.transform;


        return newWaypoint;
    }
}
