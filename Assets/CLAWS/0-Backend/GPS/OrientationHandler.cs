
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationHandler : MonoBehaviour
{
    bool originSet = false;

    private GameObject mainCamera;
    private GameObject mainCameraHolder;
    private GameObject mrtkSceneContent;

    void Awake()
    {
        mainCameraHolder = GameObject.Find("MixedRealityPlayspace");
        mainCamera = mainCameraHolder.transform.Find("Main Camera").gameObject;
        mrtkSceneContent = GameObject.Find("MixedRealitySceneContent");
        EventBus.Subscribe<UpdatedGPSEvent>(OnTSLocation);
    }

    void OnTSLocation(UpdatedGPSEvent e)
    {
        Debug.LogWarning("Recieved user location.");

        NewUserLocation(AstronautInstance.User.location);

        // To display the angle between coordinates
        // Debug.Log(LocationUtilities.DistanceAndAngleBetweenCoords(new Coords(locations.list[0].latitude, locations.list[0].latitude), new Coords(locations.list[1].latitude, locations.list[1].latitude)));
    }

    void NewUserLocation(Location location)
    {
        Location user_coordinates = new Location(
                        location.latitude,
                        location.longitude
                    );
        // TODO try getting rid of the if condition
        if (!originSet)
        {
            // set the initial origin
            GPSUtils.ChangeOriginGPSCoords(user_coordinates);

            Debug.Log("Recalculating user location.\n" +
            "Origin is: " + GPSUtils.originGPSCoords.latitude + ' ' + GPSUtils.originGPSCoords.longitude +
            "\nUser location is: " + location.latitude + ", " + location.longitude);

            Vector3 NewCameraAppPosition = GPSUtils.GPSCoordsToAppPosition(user_coordinates);
            NewCameraAppPosition.y = 0f;

            Debug.Log(NewCameraAppPosition.ToString());

            mainCameraHolder.transform.position = NewCameraAppPosition;
        }

        if (!originSet)
        {
            EventBus.Publish<UpdatedGPSOriginEvent>(new UpdatedGPSOriginEvent());
            originSet = true;
        }
    }

    /*public void Recalibrate()
    {
        GPSMsg location = Simulation.User.GPS;
        GPSCoords user_coordinates = new GPSCoords(
                        location.lat,
                        location.lon
                    );

        Debug.Log("Recalculating user location.\n" +
            "Origin is: " + GPSUtils.originGPSCoords.latitude + ' ' + GPSUtils.originGPSCoords.longitude +
            "\nUser location is: " + location.lat + ", " + location.lon);

        Vector3 NewCameraAppPosition = GPSUtils.GPSCoordsToAppPosition(user_coordinates);
        NewCameraAppPosition.y = 0f;

        Debug.Log(NewCameraAppPosition.ToString());

        mainCameraHolder.transform.position = NewCameraAppPosition;
    }*/

    public void SetNorth()
    {
        Debug.LogWarning("Recalculating North.");

        Vector3 mainCameraWorldPosition = mainCamera.transform.position;

        float mainCameraY = mainCamera.transform.localRotation.eulerAngles.y;

        mainCameraHolder.transform.eulerAngles = new Vector3(0f, mainCameraY, 0f);

        Vector3 mainCameraNewWorldPosition = mainCamera.transform.position;

        mainCameraHolder.transform.position = mainCameraHolder.transform.position + -(mainCameraNewWorldPosition - mainCameraWorldPosition);
    }


    // shifts everything around the player North
    public void ShiftNorth(int magnitude)
    {
        Vector3 change = mainCamera.transform.forward * magnitude;
        change.y = 0;
        mainCamera.transform.position += change;
    }

    public void ShiftSouth(int magnitude)
    {
        Vector3 change = mainCamera.transform.forward * magnitude;
        change.y = 0;
        mainCamera.transform.position -= change;
    }

    public void ShiftEast(int magnitude)
    {
        Vector3 change = mainCamera.transform.right * magnitude;
        change.y = 0;
        mainCamera.transform.position -= change;
    }

    public void ShiftWest(int magnitude)
    {
        Vector3 change = mainCamera.transform.right * magnitude;
        change.y = 0;
        mainCamera.transform.position += change;
    }

    public void ShiftRotationClockwise(int magnitude)
    {
        mainCameraHolder.transform.eulerAngles = mainCameraHolder.transform.eulerAngles - new Vector3(0, magnitude, 0);
    }

    public void ShiftRotationCounterClockwise(int magnitude)
    {
        mainCameraHolder.transform.eulerAngles = mainCameraHolder.transform.eulerAngles + new Vector3(0, magnitude, 0);
    }


}