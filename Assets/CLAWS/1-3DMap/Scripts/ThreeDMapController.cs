using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDMapController : MonoBehaviour
{
    private GameObject mixedRealityPlaySpace;
    private GameObject bigMap;
    private GameObject mainCamera;
    private GameObject map;
    private float scaleRatio; // Scale ratio of the mini map

    private GameObject waypointsParent;
    private GameObject playersParent;
    private GameObject pathParent;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject waypointsPrefab;
    [SerializeField] GameObject pathPrefab;

    public GameObject end;
    private Quaternion placeCameraRotation = Quaternion.identity;

    public float minScale = 1f;
    public float maxScale = 3f;
    public float zoomSpeed = 0.1f;



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

        scaleRatio = map.transform.localScale.x / 40f;
        map.SetActive(false);
    }

    public void PlaceMap()
    {
        // Offset values for forward and down
        float forwardOffset = 1f; // You can adjust this value
        float downOffset = 0.4f;   // You can adjust this value

        // Get the main camera's position and rotation
        Vector3 cameraPosition = mainCamera.transform.position;
        //placeCameraRotation = Quaternion.Euler(0f, mainCamera.transform.rotation.eulerAngles.y, 0f); ;

        // Calculate the new position by adding the offset values
        Vector3 newPosition = cameraPosition + mainCamera.transform.forward * forwardOffset +
                              Vector3.down * downOffset;

        // Set the map's position and rotation
        map.transform.position = newPosition;
        //map.transform.rotation = placeCameraRotation;

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
        return ((realLocation * scaleRatio) + map.transform.position) - (bigMap.transform.position * scaleRatio);
    }

    private void SpawnObjectOnSmallMap(Vector3 smallMapPosition)
    {
        // Instantiate the object on the smaller map at the adjusted position
        GameObject spawnedObject = Instantiate(playerPrefab, smallMapPosition, placeCameraRotation);
        spawnedObject.transform.localScale *= (scaleRatio);
        spawnedObject.transform.parent = playersParent.transform;
    }

    public void ZoomIn()
    {
        AdjustZoom(-1); // Zoom in
    }

    public void ZoomOut()
    {
        AdjustZoom(1); // Zoom out
    }

    private void AdjustZoom(int direction)
    {
        // Calculate the distance between player and target
        float distance = Vector3.Distance(mainCamera.transform.position, map.transform.position);

        // Map the distance to a scale range
        float targetScale = Mathf.Clamp(distance * zoomSpeed * direction, minScale, maxScale);

        // Calculate the original scale of the target object
        Vector3 originalScale = map.transform.localScale;

        // Calculate the new scale while maintaining the same overall size
        float scaleMultiplier = targetScale / originalScale.x;
        Vector3 newScale = originalScale * scaleMultiplier;

        // Apply the new scale to the target object
        map.transform.localScale = newScale;
    }
}
