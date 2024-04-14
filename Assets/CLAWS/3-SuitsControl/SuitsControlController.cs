using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SuitsControlController : MonoBehaviour
{
    private Subscription<VitalsUpdatedEvent> vitalsUpdateEvent;
    private Subscription<FellowAstronautVitalsDataChangeEvent> fellowVitalsUpdateEvent;

    private GameObject astr1Board;
    private GameObject astr1CritBoard;
    private GameObject astr1SuitBoard;
    private GameObject astr1PressureBoard;
    private GameObject astr1TimeBoard;

    private GameObject astr2Board;
    private GameObject astr2CritBoard;
    private GameObject astr2SuitBoard;
    private GameObject astr2PressureBoard;
    private GameObject astr2TimeBoard;

    // parameter stuff
    
    // add whatever progress bar stuff needed

    // astro1 + astro2
    TextMeshPro primary_oxygen, secondary_oxygen, suit_pressure, sub_pressure, o2_pressure, o2_rate, h20_gas_pressure, sop_pressure, sop_rate, heart_rate, fan_tachometer,
        battery_capacity, temperature, battery_time_left, o2_time_left, h2o_time_left, battery_percentage, battery_output, oxygen_primary_time, oxygen_secondary_time, water_capacity;
    TextMeshPro primary_oxygen2, secondary_oxygen2, suit_pressure2, sub_pressure2, o2_pressure2, o2_rate2, h20_gas_pressure2, sop_pressure2, sop_rate2, heart_rate2, fan_tachometer2,
        battery_capacity2, temperature2, battery_time_left2, o2_time_left2, h2o_time_left2, battery_percentage2, battery_output2, oxygen_primary_time2, oxygen_secondary_time2, water_capacity2;

    private void Start()
    {
        vitalsUpdateEvent = EventBus.Subscribe<VitalsUpdatedEvent>(onVitalsUpdate);
        fellowVitalsUpdateEvent = EventBus.Subscribe<FellowAstronautVitalsDataChangeEvent>(onFellowVitalsUpdate);

        // Astr 1
        astr1Board = transform.Find("MainVitalBoard").gameObject;
        astr1CritBoard = astr1Board.transform.Find("CritBoard").gameObject;
        astr1SuitBoard = astr1Board.transform.Find("SuitBoard").gameObject;
        astr1PressureBoard = astr1Board.transform.Find("PressureBoard").gameObject;
        astr1TimeBoard = astr1Board.transform.Find("RemainingBoard").gameObject;

        // assign all vitals based on gameobjects

    }

    private void onVitalsUpdate(VitalsUpdatedEvent e)
    {

        
    }

    private void onFellowVitalsUpdate(FellowAstronautVitalsDataChangeEvent e)
    {

    }

    private string FloatToTimeString(float timeInSeconds)
    {
        int hours = (int)(timeInSeconds / 3600);
        int minutes = (int)((timeInSeconds % 3600) / 60);
        int seconds = (int)(timeInSeconds % 60);

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    float calculatePSI(float gaugeValue)
    {
        return (float)Math.Min((gaugeValue / 6) * 10, 10);
    }

    float calculatePercentage(float gaugeValue, float gaugeMaxValue)
    {
        return (float)Math.Min((gaugeValue / gaugeMaxValue) * 100, 100);  
    }

    float calculateRotation(float percentage)
    {
        return - (percentage / 100) * 180;
    }

    void MoveIndicator(float percentage, GameObject i)
    {
        // Define the range of the local x position
        float minX = -1.86f;
        float maxX = 10.13f;

        // Calculate the target x position within the specified range
        float targetX = Mathf.Lerp(minX, maxX, percentage);

        // Set the local position of the indicator
        Vector3 targetPosition = i.transform.localPosition;
        targetPosition.x = targetX;
        i.transform.localPosition = targetPosition;
    }

    // insert progress bar stuff


    // progress bar color changing
}