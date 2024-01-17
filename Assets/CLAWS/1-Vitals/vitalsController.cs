using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class vitalsController : MonoBehaviour
{
    private Subscription<VitalsUpdatedEvent> vitalsUpdateEvent;
    private Subscription<FellowAstronautVitalsDataChangeEvent> fellowVitalsUpdateEvent;
    public GameObject parent;
    public GameObject bodyObject;
    public GameObject suitObject;
    public GameObject bodyObjectFellow;
    public GameObject suitObjectFellow;
    public GameObject parentFellow;

    //For half gauge
    GameObject halfRingPartialFiller;
    GameObject halfRingFullFiller;
    SpriteRenderer halfRingPartialFillerSprite;
    SpriteRenderer halfRingFullFillerSprite;
    GameObject halfRing;
    TextMeshPro gaugeTitle;
    TextMeshPro gaugeValue;
    TextMeshPro gaugeUnit;

    //For progress bar
    GameObject indicator;

    // Placeholder values for the gauge current value and max value. Currently for pressure (psi) from 0.0-7.0
    public float gaugeMaxValue = 7.0f;

    //1st astronaut
    TextMeshPro heartRate, PO2, SO2, roomID, is_running, is_paused, time,
                timer, started_at, suit_p, sub_p, O2_p, h2O_gas_p, h2O_liq_p, sop_p,
                sop_rate, fan_tach, btry_cap, temp, btry_timeLeft, btry_perc,
                btry_out, O2_primeTime, O2_secTime, h2O_cap, O2timeLeft, O2rate,
                H2OtimeLeft;
    //2nd astronaut
    TextMeshPro heartRate1, PO21, SO21, roomID1, is_running1, is_paused1, time1,
                timer1, started_at1, suit_p1, sub_p1, O2_p1, h2O_gas_p1, h2O_liq_p1, sop_p1,
                sop_rate1, fan_tach1, btry_cap1, temp1, btry_timeLeft1, btry_perc1,
                btry_out1, O2_primeTime1, O2_secTime1, h2O_cap1, O2timeLeft1, O2rate1,
                H2OtimeLeft1;

    void Start()
    {
        vitalsUpdateEvent = EventBus.Subscribe<VitalsUpdatedEvent>(onVitalsUpdate);
        fellowVitalsUpdateEvent = EventBus.Subscribe<FellowAstronautVitalsDataChangeEvent>(onFellowVitalsUpdate);

        //body board
        heartRate = bodyObject.transform.Find("HeartRate").gameObject.GetComponent<TextMeshPro>();
        temp = bodyObject.transform.Find("Temperature").gameObject.GetComponent<TextMeshPro>();

        //suit board
        O2rate = suitObject.transform.Find("O2Rate").gameObject.GetComponent<TextMeshPro>();
        btry_perc = suitObject.transform.Find("Btry_perc").gameObject.GetComponent<TextMeshPro>();

        suit_p = suitObject.transform.Find("Suit_p").gameObject.GetComponent<TextMeshPro>();

        O2_p = suitObject.transform.Find("O2_p").gameObject.GetComponent<TextMeshPro>();

        PO2 = suitObject.transform.Find("PO2").gameObject.GetComponent<TextMeshPro>();

        //main board
        h2O_liq_p = parent.transform.Find("H2O_liq_p").gameObject.GetComponent<TextMeshPro>();
        h2O_gas_p = parent.transform.Find("H2O_gas_p").gameObject.GetComponent<TextMeshPro>();
        fan_tach = parent.transform.Find("Fan_tach").gameObject.GetComponent<TextMeshPro>();

        //connects to game object
        //1st astronaut
        O2timeLeft = parent.transform.Find("O2TimeLeft").gameObject.GetComponent<TextMeshPro>();
        H2OtimeLeft = parent.transform.Find("H2OTimeLeft").gameObject.GetComponent<TextMeshPro>();
        is_running = parent.transform.Find("Running").gameObject.GetComponent<TextMeshPro>();
        is_paused = parent.transform.Find("Paused").gameObject.GetComponent<TextMeshPro>();
        time = parent.transform.Find("Time").gameObject.GetComponent<TextMeshPro>();
        timer = parent.transform.Find("Timer").gameObject.GetComponent<TextMeshPro>();
        started_at = parent.transform.Find("Started").gameObject.GetComponent<TextMeshPro>();
        sub_p = parent.transform.Find("Sub_p").gameObject.GetComponent<TextMeshPro>();
        SO2 = parent.transform.Find("SO2").gameObject.GetComponent<TextMeshPro>();
        roomID = parent.transform.Find("RoomID").gameObject.GetComponent<TextMeshPro>();
        sop_p = parent.transform.Find("Sop_p").gameObject.GetComponent<TextMeshPro>();
        sop_rate = parent.transform.Find("Sop_rate").gameObject.GetComponent<TextMeshPro>();
        btry_cap = parent.transform.Find("Btry_cap").gameObject.GetComponent<TextMeshPro>();
        btry_timeLeft = parent.transform.Find("Btry_timeLeft").gameObject.GetComponent<TextMeshPro>();
        btry_out = parent.transform.Find("Btry_out").gameObject.GetComponent<TextMeshPro>();
        O2_primeTime = parent.transform.Find("O2_primeTime").gameObject.GetComponent<TextMeshPro>();
        O2_secTime = parent.transform.Find("O2_secTime").gameObject.GetComponent<TextMeshPro>();
        h2O_cap = parent.transform.Find("H2O_cap").gameObject.GetComponent<TextMeshPro>();

        //2nd astronaut

        //body board
        heartRate1 = bodyObjectFellow.transform.Find("HeartRate").gameObject.GetComponent<TextMeshPro>();
        temp1 = bodyObjectFellow.transform.Find("Temperature").gameObject.GetComponent<TextMeshPro>();

        //suit board
        O2rate1 = suitObjectFellow.transform.Find("O2Rate").gameObject.GetComponent<TextMeshPro>();
        btry_perc1 = suitObjectFellow.transform.Find("Btry_perc").gameObject.GetComponent<TextMeshPro>();
        suit_p1 = suitObjectFellow.transform.Find("Suit_p").gameObject.GetComponent<TextMeshPro>();
        O2_p1 = suitObjectFellow.transform.Find("O2_p").gameObject.GetComponent<TextMeshPro>();
        PO21 = suitObjectFellow.transform.Find("PO2").gameObject.GetComponent<TextMeshPro>();

        //main board
        h2O_liq_p1 = parent.transform.Find("H2O_liq_p1").gameObject.GetComponent<TextMeshPro>();
        h2O_gas_p1 = parent.transform.Find("H2O_gas_p1").gameObject.GetComponent<TextMeshPro>();
        fan_tach1 = parent.transform.Find("Fan_tach1").gameObject.GetComponent<TextMeshPro>();

        SO21 = parent.transform.Find("SO21").gameObject.GetComponent<TextMeshPro>();
        O2timeLeft1 = parent.transform.Find("O2TimeLeft1").gameObject.GetComponent<TextMeshPro>();
        H2OtimeLeft1 = parent.transform.Find("H2OTimeLeft1").gameObject.GetComponent<TextMeshPro>();
        roomID1 = parent.transform.Find("roomID1").gameObject.GetComponent<TextMeshPro>();
        is_running1 = parent.transform.Find("Running1").gameObject.GetComponent<TextMeshPro>();
        is_paused1 = parent.transform.Find("Paused1").gameObject.GetComponent<TextMeshPro>();
        time1 = parent.transform.Find("Time1").gameObject.GetComponent<TextMeshPro>();
        timer1 = parent.transform.Find("Timer1").gameObject.GetComponent<TextMeshPro>();
        started_at1 = parent.transform.Find("Started1").gameObject.GetComponent<TextMeshPro>();
        sub_p1 = parent.transform.Find("Sub_p1").gameObject.GetComponent<TextMeshPro>();
        sop_p1 = parent.transform.Find("Sop_p1").gameObject.GetComponent<TextMeshPro>();
        sop_rate1 = parent.transform.Find("Sop_rate1").gameObject.GetComponent<TextMeshPro>();
        btry_cap1 = parent.transform.Find("Btry_cap1").gameObject.GetComponent<TextMeshPro>();
        btry_timeLeft1 = parent.transform.Find("Btry_timeLeft1").gameObject.GetComponent<TextMeshPro>();
        btry_out1 = parent.transform.Find("Btry_out1").gameObject.GetComponent<TextMeshPro>();
        O2_primeTime1 = parent.transform.Find("O2_primeTime1").gameObject.GetComponent<TextMeshPro>();
        O2_secTime1 = parent.transform.Find("O2_secTime1").gameObject.GetComponent<TextMeshPro>();
        h2O_cap1 = parent.transform.Find("H2O_cap1").gameObject.GetComponent<TextMeshPro>();
    }

    private void onVitalsUpdate(VitalsUpdatedEvent e)
    {
        //updates text on game object for 1st astronaut

        //body
        heartRate.text = e.vitals.heart_rate.ToString();
        temp.text = e.vitals.temperature.ToString();

        //suit
        O2rate.text = e.vitals.o2_rate.ToString("F0");

        
        suit_p.text = e.vitals.suit_pressure.ToString("F1");
        setGaugeObject(suit_p);
        updateGaugeValue(e.vitals.suit_pressure);
        O2_p.text = e.vitals.o2_pressure.ToString("F1");
        setGaugeObject(O2_p);
        updateGaugeValue(e.vitals.o2_pressure);

        //main board
        h2O_liq_p.text = e.vitals.h2o_liquid_pressure.ToString();
        setBarObject(e.vitals.h2o_liquid_pressure, "liq_psi");

        fan_tach.text = e.vitals.fan_tachometer.ToString();
        setBarObject(e.vitals.fan_tachometer, "fan");

        h2O_gas_p.text = e.vitals.h2o_gas_pressure.ToString();
        setBarObject(e.vitals.h2o_gas_pressure, "gas_psi");

        PO2.text = e.vitals.primary_oxygen.ToString();
        btry_perc.text = e.vitals.battery_percentage.ToString();
        h2O_gas_p.text = "H2O: " + e.vitals.h2o_gas_pressure.ToString();
        O2timeLeft.text = "O2 time remaining: " + e.vitals.o2_time_left.ToString();
        H2OtimeLeft.text = "H2O time remaining: " + e.vitals.h2o_time_left.ToString();
        is_running.text = "Running: " + e.vitals.is_running.ToString();
        is_paused.text = "Paused: " + e.vitals.is_paused.ToString();
        time.text = "Time: " + e.vitals.time.ToString();
        timer.text = "Timer: " + e.vitals.timer.ToString();
        started_at.text = "Started at: " + e.vitals.started_at.ToString();
        sub_p.text = "Sub Pressure: " + e.vitals.sub_pressure.ToString();
        SO2.text = "Secondary O2: " + e.vitals.secondary_oxygen.ToString();
        roomID.text = "RoomID: " + e.vitals.room_id.ToString();
        sop_p.text = "Sop Pressure: " + e.vitals.sop_pressure.ToString();
        sop_rate.text = "Sop Rate: " + e.vitals.sop_rate.ToString();
        btry_cap.text = "Battery Capacity: " + e.vitals.battery_capacity.ToString();
        btry_timeLeft.text = "Battery Time Left: " + e.vitals.battery_time_left.ToString();
        btry_out.text = "Battery Output: " + e.vitals.battery_outputput.ToString();
        O2_primeTime.text = "O2 Primary Time: " + e.vitals.oxygen_primary_time.ToString();
        O2_secTime.text = "O2 Secondary Time: " + e.vitals.oxygen_secondary_time.ToString();
        h2O_cap.text = "H2O Capacity: " + e.vitals.water_capacity.ToString();
        // Add warning screens + when rates are going to fast
    }

    private void onFellowVitalsUpdate(FellowAstronautVitalsDataChangeEvent e) {
        //updates text on game object for 2nd astronaut 
        heartRate1.text = "HeartRate: " + e.AstronautToChange.vitals.heart_rate.ToString();
        h2O_gas_p1.text = "H2O Gas Pressure: " + e.AstronautToChange.vitals.h2o_gas_pressure.ToString();
        PO21.text = "Primary O2: " + e.AstronautToChange.vitals.primary_oxygen.ToString();
        SO21.text = "Secondary O2: " + e.AstronautToChange.vitals.secondary_oxygen.ToString();
        O2timeLeft1.text = "O2 Time Remaining: " + e.AstronautToChange.vitals.o2_time_left.ToString();
        H2OtimeLeft1.text = "H2O Time Remaining: " + e.AstronautToChange.vitals.h2o_time_left.ToString();
        O2rate1.text = "O2 Rate: " + e.AstronautToChange.vitals.o2_rate.ToString();
        roomID1.text = "RoomID: " + e.AstronautToChange.vitals.room_id.ToString();
        is_running1.text = "Running: " + e.AstronautToChange.vitals.is_running.ToString();
        is_paused1.text = "Paused: " + e.AstronautToChange.vitals.is_paused.ToString();
        time1.text = "Time: " + e.AstronautToChange.vitals.time.ToString();
        timer1.text = "Timer: " + e.AstronautToChange.vitals.timer.ToString();
        started_at1.text = "Started at: " + e.AstronautToChange.vitals.started_at.ToString();
        sub_p1.text = "Sub Pressure: " + e.AstronautToChange.vitals.sub_pressure.ToString();

        suit_p1.text = "Suit Pressure: " + e.AstronautToChange.vitals.suit_pressure.ToString();
        setGaugeObject(suit_p1);
        updateGaugeValue(e.AstronautToChange.vitals.suit_pressure);
        O2_p1.text = "O2 Pressure: " + e.AstronautToChange.vitals.o2_pressure.ToString();
        setGaugeObject(O2_p1);
        updateGaugeValue(e.AstronautToChange.vitals.o2_pressure);


        h2O_liq_p1.text = "H2O Liquid Pressure: " + e.AstronautToChange.vitals.h2o_liquid_pressure.ToString();
        sop_p1.text = "Sop Pressure: " + e.AstronautToChange.vitals.sop_pressure.ToString();
        sop_rate1.text = "Sop Rate: " + e.AstronautToChange.vitals.sop_rate.ToString();
        fan_tach1.text = "Fan Tachometer: " + e.AstronautToChange.vitals.fan_tachometer.ToString();
        btry_cap1.text = "Battery Capacity: " + e.AstronautToChange.vitals.battery_capacity.ToString();
        temp1.text = "Temperature: " + e.AstronautToChange.vitals.temperature.ToString();
        btry_timeLeft1.text = "Battery Time Left: " + e.AstronautToChange.vitals.battery_time_left.ToString();
        btry_perc1.text = "Battery Percent: " + e.AstronautToChange.vitals.battery_percentage.ToString();
        btry_out1.text = "Battery Output: " + e.AstronautToChange.vitals.battery_outputput.ToString();
        O2_primeTime1.text = "O2 Primary Time: " + e.AstronautToChange.vitals.oxygen_primary_time.ToString();
        O2_secTime1.text = "O2 Secondary Time: " + e.AstronautToChange.vitals.oxygen_secondary_time.ToString();
        h2O_cap1.text = "H2O Capacity: " + e.AstronautToChange.vitals.water_capacity.ToString();
        // Add warning screens + when rates are going to fast
    }

    // Half gauge functions

    public void setGaugeObject(TextMeshPro halfGauge)
    {
        halfRing = halfGauge.transform.Find("HalfRing").gameObject;
        gaugeUnit = halfGauge.transform.Find("HalfGaugeUnit").gameObject.GetComponent<TextMeshPro>();

        // This ring filler is toggled on when the gauge value is between 0-50%
        halfRingPartialFiller = halfRing.transform.Find("HalfRingPartialFiller").gameObject;
        halfRingPartialFillerSprite = halfRingPartialFiller.GetComponent<SpriteRenderer>();

        //This ring filler is toggled on when the gauge value is >= 50%
        halfRingFullFiller = halfRing.transform.Find("HalfRingFullFiller").gameObject;
        halfRingFullFillerSprite = halfRingFullFiller.GetComponent<SpriteRenderer>();

    }

    public void updateGaugeValue(float incomingGaugeValue)
    {
        setGaugeColor(incomingGaugeValue);

        float percentValue = calculatePercentage(incomingGaugeValue);
        float degreeRotation = calculateRotation(percentValue);
        if (percentValue <= 50)
        {
            //Fetch current Z rotational value of the filler and subtract that before update
            float currentRotationZ = halfRingPartialFiller.transform.rotation.eulerAngles.z - 90;

            //Show partial ring filler, hide full ring filler
            halfRingFullFiller.SetActive(false);
            halfRingPartialFiller.SetActive(true);
            halfRingPartialFiller.transform.Rotate(0.0f, 0.0f, degreeRotation - currentRotationZ, Space.Self);
        }
        else
        {
            //Fetch current Z rotational value of the filler and subtract that before update
            float currentRotationZ = halfRingFullFiller.transform.rotation.eulerAngles.z - 180;

            //Hide partial ring filler, show full ring filler
            halfRingFullFiller.SetActive(true);
            halfRingPartialFiller.SetActive(false);
            halfRingFullFiller.transform.Rotate(0.0f, 0.0f, degreeRotation - currentRotationZ, Space.Self);

        }
    }

    float calculatePercentage(float gaugeValue)
    {
        return (float)Math.Min((gaugeValue / gaugeMaxValue) * 100, 100);
    }
    float calculateRotation(float percentage)
    {
        return -(percentage / 100) * 180;
    }

    void setGaugeColor(float gaugeValue)
    {
        Color warningColor = new Color(1.0f, 0.4f, 0f, 1.0f);  // Color values are normalized (0 to 1)
        Color dangerColor = new Color(1.0f, 0f, 0f, 1.0f);
        Color regularColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        if (gaugeValue <= 1 || gaugeValue >= 5.0)
        {
            halfRingPartialFillerSprite.color = dangerColor;
            halfRingFullFillerSprite.color = dangerColor;
        }
        else if ((gaugeValue > 1 && gaugeValue <= 1.9) || (gaugeValue >= 4.1 && gaugeValue < 5.0))
        {
            halfRingPartialFillerSprite.color = warningColor;
            halfRingFullFillerSprite.color = warningColor;
        }
        else
        {
            halfRingPartialFillerSprite.color = regularColor;
            halfRingFullFillerSprite.color = regularColor;
        }
    }

    //progress bar functions
    void setBarObject(float barValue, string type)
    {
        if (type == "liq_psi")
        {
            indicator = parent.transform.Find("Liq_bar").gameObject.transform.Find("Indicator").gameObject;
        }
        if (type == "gas_psi")
        {
            indicator = parent.transform.Find("gas_bar").gameObject.transform.Find("Indicator").gameObject;
        }
        if (type == "fan")
        {
            indicator = parent.transform.Find("fan_bar").gameObject.transform.Find("Indicator").gameObject;
        }

        Vector3 currentPosition = transform.position;
        currentPosition.x = calculatePSI(barValue);
        indicator.transform.position = currentPosition;
    }



    float calculatePSI(float barValue)
    {
        return (float)Math.Min((barValue / 6) * 10, 10);
    }
}