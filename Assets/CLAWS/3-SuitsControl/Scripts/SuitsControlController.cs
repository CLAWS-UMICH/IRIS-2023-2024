using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public enum DCUWarningEnum
{
    PrimaryOxyOxy,
    O2SuitPressure,
    Scrubber,
    CO2SuitPressure,
    PrimaryOxyPump,
    Comms,
    FanSpeed,
    HelmetCO2Pressure,
    PrimaryBattery
}

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

    // Battery Parameters
    private float BATT_TIME_MAX = 10800.0f; // sec
    private float BATT_TIME_MIN = 3600.0f; // sec

    // Oxygen Parameters
    private float OXY_STOR_MAX = 100.0f; // %
    private float OXY_STOR_MIN = 20.0f; // %

    private float OXY_PRES_MAX = 3000.0f; // PSI
    private float OXY_PRES_MIN = 600.0f; // PSI

    private float OXY_TIME_MAX = 21600.0f; // sec
    private float OXY_TIME_MIN = 3600.0f; // sec

    private float OXY_CONSUM_MAX = 0.15f; // psi/min
    private float OXY_CONSUM_MIN = 0.05f; // psi/min

    // CO2 Parameters
    private float CO2_PROD_MAX = 0.15f; // psi/min
    private float CO2_PROD_MIN = 0.05f; // psi/min

    // Coolant Parameters
    private float COOL_STOR_MAX = 100.0f; // %
    private float COOL_STOR_MIN = 80.0f; // %

    // Heart Rate Parameters
    private float HEART_RATE_MAX = 160.0f; // bpm
    private float HEART_RATE_MIN = 50.0f; // bpm

    // Pressure Parameters
    private float SUIT_PRES_OXY_MAX = 4.1f; // psi
    private float SUIT_PRES_OXY_MIN = 3.5f; // psi

    private float SUIT_PRES_CO2_MAX = 0.1f; // psi
    private float SUIT_PRES_OTHER_MAX = 0.5f; // psi
    private float SUIT_PRES_TOTAL_MAX = 4.5f; // psi
    private float SUIT_PRES_TOTAL_MIN = 3.5f; // psi

    private float HELMET_PRES_CO2_MAX = 0.15f; // psi

    // Fan Parameters
    private float FAN_SPEED_MAX = 30000.0f; // rpm
    private float FAN_SPEED_MIN = 20000.0f; // rpm

    // Scrubber Parameters
    private float SCRUBBER_CO2_STOR_MAX = 60.0f; // %

    // Temperature Parameters
    private float TEMP_MAX = 90.0f; // farhenheit
    private float TEMP_MIN = 50.0f; // farhenheit

    // Coolant Parameters
    private float COOL_LIQ_MAX = 700.0f; // psi
    private float COOL_LIQ_MIN = 100.0f; // psi
    private float COOL_GAS_MAX = 700.0f; // psi

    // add whatever progress bar stuff needed

    //1st astronaut
    GameObject heartRate, temp, oxyCons, co2Prod, priOxy, batt, suitPresOxy, suitTotPres, suitPresCO2, secOxyStor, secOxyPres, priFan, coolLiquidPres, coolGasPres, powerTime, oxyTime;

    //2nd astronaut
    GameObject heartRate2, temp2, oxyCons2, co2Prod2, priOxy2, batt2, suitPresOxy2, suitTotPres2, suitPresCO22, secOxyStor2, secOxyPres2, priFan2, coolLiquidPres2, coolGasPres2, powerTime2, oxyTime2;


    private void Start()
    {
        vitalsUpdateEvent = EventBus.Subscribe<VitalsUpdatedEvent>(onVitalsUpdate);
        fellowVitalsUpdateEvent = EventBus.Subscribe<FellowAstronautVitalsDataChangeEvent>(onFellowVitalsUpdate);

        // Astr 1
        astr1Board = transform.Find("SuitsControlScreen").gameObject;
        astr1CritBoard = astr1Board.transform.Find("ScreenCrit").gameObject;
        astr1SuitBoard = astr1Board.transform.Find("SuitBoard").gameObject;
        astr1PressureBoard = astr1Board.transform.Find("PressureBoard").gameObject;
        astr1TimeBoard = astr1Board.transform.Find("RemainingBoard").gameObject;

        // assign all vitals based on gameobjects

        //HR
        heartRate = astr1CritBoard.transform.Find("HeartRate").gameObject;
        heartRate.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 0f);
        heartRate.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", 302f);
        heartRate.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Angle", 241f);

        //O2cons
        oxyCons = astr1CritBoard.transform.Find("O2cons").gameObject;
        oxyCons.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 0f);
        oxyCons.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", 302f);
        oxyCons.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Angle", 241f);

        //co2prod
        co2Prod = astr1CritBoard.transform.Find("CO2prod").gameObject;
        co2Prod.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 0f);
        co2Prod.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", 302f);
        co2Prod.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Angle", 241f);

        //temp
        temp = astr1CritBoard.transform.Find("Temp").gameObject;
        temp.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc2", 0f);
        temp.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", 302f);
        temp.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Angle", 241f);
    }

    private void onVitalsUpdate(VitalsUpdatedEvent e)
    {
        // update all the board stuff
        heartRate.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", (float)((1 - e.vitals.heart_rate / 100) * 302));
        heartRate.transform.Find("BodyText").GetComponent<TextMeshPro>().text = e.vitals.heart_rate.ToString("F0");

        oxyCons.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", (float)((1 - e.vitals.heart_rate) * 302));
        oxyCons.transform.Find("BodyText").GetComponent<TextMeshPro>().text = e.vitals.heart_rate.ToString("F0");

        co2Prod.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", (float)((1 - e.vitals.heart_rate) * 302));
        co2Prod.transform.Find("BodyText").GetComponent<TextMeshPro>().text = e.vitals.heart_rate.ToString("F0");

        temp.transform.Find("RingFull").GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", (float)((1 - e.vitals.heart_rate / 100) * 302));
        temp.transform.Find("BodyText").GetComponent<TextMeshPro>().text = e.vitals.heart_rate.ToString("F0");

        checkVitals(e);
    }

    private void onFellowVitalsUpdate(FellowAstronautVitalsDataChangeEvent e)
    {
        // update fellow board stuff
    }

    private void checkVitals(VitalsUpdatedEvent e)
    {
        if (e.vitals.batt_time_left < 3600 || e.vitals.batt_time_left > 10800)
        {
            // display PrimaryBattery notif
        }
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