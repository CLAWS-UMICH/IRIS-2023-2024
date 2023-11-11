using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDMapController : MonoBehaviour
{
    private GameObject mixedRealityPlaySpace;
    private GameObject mainCamera;
    private GameObject map;
    public float scaleRatio; // Scale ratio of the mini map

    private GameObject waypointsParent;
    private GameObject playersParent;
    private GameObject pathParent;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject waypointsPrefab;
    [SerializeField] GameObject pathPrefab;



    // Start is called before the first frame update
    void Start()
    {
        mixedRealityPlaySpace = GameObject.Find("MixedRealityPlayspace").gameObject;
        mainCamera = mixedRealityPlaySpace.transform.Find("Main Camera").gameObject;

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
        realToSmall(mainCamera.transform.position);
    }

    private Vector3 realToSmall(Vector3 realLocation)
    {
        scaleRatio = map.transform.localScale.x;
        // Adjust the position based on the scale ratio
        Vector3 adjustedPosition = realLocation * scaleRatio + (map.transform.position - new Vector3(0, 0, 0));
        GameObject newPlayer = Instantiate(playerPrefab, adjustedPosition, Quaternion.identity);
        newPlayer.transform.localScale *= scaleRatio / 40;
        newPlayer.transform.parent = playersParent.transform;
        GameObject spawnedObject = Instantiate(playerPrefab, realLocation, Quaternion.identity);
        spawnedObject.transform.localScale *= scaleRatio;

        return adjustedPosition;
    }
}
