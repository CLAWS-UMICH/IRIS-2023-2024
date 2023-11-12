using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDMapController : MonoBehaviour
{
    private GameObject mixedRealityPlaySpace;
    private GameObject bigMap;
    private GameObject mainCamera;
    private GameObject map;
    public float scaleRatio; // Scale ratio of the mini map

    private GameObject waypointsParent;
    private GameObject playersParent;
    private GameObject pathParent;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject waypointsPrefab;
    [SerializeField] GameObject pathPrefab;

    public GameObject end;



    // Start is called before the first frame update
    void Start()
    {
        mixedRealityPlaySpace = GameObject.Find("MixedRealityPlayspace").gameObject;
        mainCamera = mixedRealityPlaySpace.transform.Find("Main Camera").gameObject;
        bigMap = mixedRealityPlaySpace.transform.Find("Martian Terrain").gameObject;

        map = transform.Find("Martian Terrain").gameObject;
        waypointsParent = map.transform.Find("Waypoints").gameObject;
        playersParent = map.transform.Find("Players").gameObject;
        pathParent = map.transform.Find("Path").gameObject;

        scaleRatio = map.transform.localScale.x;
        map.SetActive(false);
    }

    public void PlaceMap()
    {
        // Offset values for forward and down
        float forwardOffset = 1f; // You can adjust this value
        float downOffset = 0.4f;   // You can adjust this value

        // Get the main camera's position
        Vector3 cameraPosition = mainCamera.transform.position;

        // Calculate the new position by adding the offset values
        Vector3 newPosition = cameraPosition + mainCamera.transform.forward * forwardOffset +
                              Vector3.down * downOffset;

        map.transform.position = newPosition;
        map.SetActive(true);
    }

    public void CloseMap()
    {
        map.SetActive(false);
    }

    public void TestSpawn()
    {
        // Use the realToSmall method to spawn an object on the smaller map
        Vector3 smallMapPosition = realToSmall(end.transform.position);
        SpawnObjectOnSmallMap(smallMapPosition);
    }

    private Vector3 realToSmall(Vector3 realLocation)
    {
        return (realLocation * scaleRatio / 40) + (bigMap.transform.position * 0.0389f);
    }

    private void SpawnObjectOnSmallMap(Vector3 smallMapPosition)
    {
        // Instantiate the object on the smaller map at the adjusted position
        GameObject spawnedObject = Instantiate(playerPrefab, smallMapPosition, Quaternion.identity);
        spawnedObject.transform.localScale *= (scaleRatio / 40);
        spawnedObject.transform.parent = playersParent.transform;
    }
}
