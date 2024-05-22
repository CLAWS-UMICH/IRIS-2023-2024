using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FakeTSSMessageSender : MonoBehaviour
{
    [SerializeField] float secondsToUpdate = 3f;
    public Location fakeGPS = new Location();
    private float timer;
    private GameObject mainCamera;

    private void Start()
    {
        GameObject mainCameraHolder = GameObject.Find("MixedRealityPlayspace");
        mainCamera = mainCameraHolder.transform.Find("Main Camera").gameObject;
        Fake_SetGPS();
        //Fake_Vitals();
        StartCoroutine(AddFake());
        //StartCoroutine(WebConnect());
    }
   
    public void Fake_SetGPS()
    {
        AstronautInstance.User.location = fakeGPS;
        EventBus.Publish<UpdatedGPSEvent>(new UpdatedGPSEvent());
        StartCoroutine(UpdateLocation());
    }


    IEnumerator UpdateLocation()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsToUpdate); // Wait for 3 seconds

            // Update Location
            AstronautInstance.User.location = GPSUtils.AppPositionToGPSCoords(mainCamera.transform.position);

        }
    }

    public void Fake_Vitals()
    {
        //AstronautInstance.User.VitalsData.started_at = DateTime.Now.ToString("HH:mm:ss");
        StartCoroutine(UpdateVitals());
    }

    public void InitialWaypoints()
    {


        /*double[,] locationAndTypes = new double[2, 3] // 0 = station, 1 = POI, 2 = geo, 3 = danger
        {
            {29.564654728, -95.081798277, 2}, // J Rover
            {29.564654728, -95.081798277, 2}, // K Astronaut 2
        };

        string[,] letters = new string[2, 1]
        {
            {"J"},
            {"K"},
        };

        List<Waypoint> list = new List<Waypoint>();

        for (int i = 0; i < 9; i++)
        {
            Waypoint way = new Waypoint();
            way.waypoint_id = i;
            way.waypoint_letter = letters[i, 0];
            Location loc = new Location(locationAndTypes[i, 0], locationAndTypes[i, 1]);
            way.location = loc;
            way.type = (int)locationAndTypes[i, 2];
            way.description = "Way " + way.waypoint_letter;
            way.author = 1;
            AstronautInstance.User.WaypointData.AllWaypoints.Add(way);
            list.Add(way);
        }

        EventBus.Publish(new WaypointsAddedEvent(list));*/

    }

    IEnumerator WebConnect()
    {
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<MainConnections>().ConnectToWebsocket("ws://ec2-52-15-178-2.us-east-2.compute.amazonaws.com/hololens", "", "Brian", 0);
    }

    IEnumerator AddFake()
    {
        yield return new WaitForSeconds(3);
        InitialWaypoints();
        /*FakeWayPoints();
        yield return new WaitForSeconds(5);
        Fake();*/
    }

    IEnumerator UpdateVitals()
    {
        while (true)
        {
            //Debug.Log("Test"); 
            AstronautInstance.User.VitalsData.eva_time = 69;
            yield return new WaitForSeconds(secondsToUpdate); // Wait for 3 seconds
            
            // Update vitals with random values
            AstronautInstance.User.VitalsData.batt_percentage = 90;
            //AstronautInstance.User.VitalsData.oxy_sec_storage = UnityEngine.Random.Range(90f, 100f);
            //AstronautInstance.User.VitalsData.suit_pressure = UnityEngine.Random.Range(0.8f, 5.2f);
            //AstronautInstance.User.VitalsData.sub_pressure = UnityEngine.Random.Range(15f, 25f);
            //AstronautInstance.User.VitalsData.o2_pressure = UnityEngine.Random.Range(0.8f, 5.2f);
            //AstronautInstance.User.VitalsData.o2_rate = UnityEngine.Random.Range(50f, 90f);
            //AstronautInstance.User.VitalsData.h2o_gas_pressure = UnityEngine.Random.Range(8f, 15f);
            //AstronautInstance.User.VitalsData.h2o_liquid_pressure = UnityEngine.Random.Range(18f, 25f);
            //AstronautInstance.User.VitalsData.sop_pressure = UnityEngine.Random.Range(6f, 12f);
            //AstronautInstance.User.VitalsData.sop_rate = UnityEngine.Random.Range(1f, 3f);
            //AstronautInstance.User.VitalsData.heart_rate = UnityEngine.Random.Range(60f, 100f);
            //AstronautInstance.User.VitalsData.fan_tachometer = UnityEngine.Random.Range(1000f, 3000f);
            //AstronautInstance.Usesr.VitalsData.battery_capacity = UnityEngine.Random.Range(3000f, 5000f);
            //AstronautInstance.User.VitalsData.temperature = UnityEngine.Random.Range(20f, 30f);
            //AstronautInstance.User.VitalsData.battery_percentage = UnityEngine.Random.Range(70f, 100f);
            //AstronautInstance.User.VitalsData.battery_outputput = UnityEngine.Random.Range(200f, 400f);
            //AstronautInstance.User.VitalsData.oxygen_primary_time = UnityEngine.Random.Range(1000f, 2000f);
            //AstronautInstance.User.VitalsData.oxygen_secondary_time = UnityEngine.Random.Range(1500f, 2500f);
            //AstronautInstance.User.VitalsData.water_capacity = UnityEngine.Random.Range(2000f, 4000f);

            // Increment timer
            timer += 3f;

            // Update timer property (optional)
            TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
            //AstronautInstance.User.VitalsData.timer = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            // Battery time left
            TimeSpan batteryTime = TimeSpan.FromMinutes(5) - timeSpan;
            AstronautInstance.User.VitalsData.batt_time_left = batteryTime.Hours + batteryTime.Minutes + batteryTime.Seconds;

            // O2 time left
            TimeSpan o2Time = TimeSpan.FromMinutes(5) - timeSpan;
            AstronautInstance.User.VitalsData.oxy_time_left = o2Time.Hours + o2Time.Minutes + o2Time.Seconds;

            // H2O time left
            //TimeSpan h2oTime = TimeSpan.FromMinutes(5) - timeSpan;
            //AstronautInstance.User.VitalsData.h2o_time_left = string.Format("{0:D2}:{1:D2}:{2:D2}", h2oTime.Hours, h2oTime.Minutes, h2oTime.Seconds);

            // Publish VitalsUpdatedEvent
            EventBus.Publish<VitalsUpdatedEvent>(new VitalsUpdatedEvent(AstronautInstance.User.VitalsData));

        }
    }

}
