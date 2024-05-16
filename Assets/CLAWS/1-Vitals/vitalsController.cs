using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class vitalsController : MonoBehaviour
{
    private Subscription<VitalsUpdatedEvent> vitalsUpdateEvent;
    private Subscription<FellowAstronautVitalsDataChangeEvent> fellowVitalsUpdateEvent;

    private GameObject astr1Board;
    private GameObject astr1BodyBoard;
    private GameObject astr1SuitBoard;
    private GameObject astr1BackupBoard;
    private GameObject astr1BarBoard;
    private GameObject astr1TimeBoard;

    private GameObject astr2Board;
    private GameObject astr2BodyBoard;
    private GameObject astr2SuitBoard;
    private GameObject astr2BackupBoard;
    private GameObject astr2BarBoard;
    private GameObject astr2TimeBoard;

    // Battery parameters
    private float BATT_TIME_CAP = 21600;
    private float BATT_FILL_RATE => BATT_TIME_CAP / 150.0f;

    // Oxygen parameters
    private float OXY_TIME_CAP = 10800;
    private float OXY_PRESSURE_CAP = 3000.0f;
    private float OXY_FILL_RATE = 0.8f;

    // Depressurization parameters
    private float DEPRESS_TIME = 15;
    private float RESTING_HEART_RATE = 90.0f;
    private float EVA_HEART_RATE = 140.0f;
    private float HAB_OXY_PRESSURE = 3.0723f;
    private float HAB_CO2_PRESSURE = 0.0059f;
    private float HAB_OTHER_PRESSURE = 11.5542f;
    private float SUIT_OXY_PRESSURE = 4.0f;
    private float SUIT_CO2_PRESSURE = 0.001f;
    private float SUIT_OTHER_PRESSURE = 0.0f;

    // Suit parameters
    private float SUIT_OXY_CONSUMPTION = 0.1f;
    private float SUIT_CO2_PRODUCTION = 0.1f;

    // Suit fan parameters
    private float SUIT_FAN_SPIN_UP_RATE = 0.9f;
    private float SUIT_FAN_RPM = 30000.0f;
    private float SUIT_FAN_ERROR_RPM = 5000.0f;

    // Suit scrubber parameters
    private float SUIT_SCRUBBER_CAP = 1.0f;
    private float SUIT_SCRUBBER_FILL_RATE = 0.8f;
    private float SUIT_SCRUBBER_FLUSH_RATE = 0.85f;

    // Suit coolant parameters
    private float SUIT_COOLANT_NOMINAL_TEMP = 65.0f;
    private float SUIT_COOLANT_NOMINAL_PRESSURE = 500.0f;

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
    TextMeshPro heartRate, temp, oxyCons, co2Prod, priOxyStor, priOxy, batt, suitPresOxy, suitTotPres, suitPresCO2, secOxyStor, secOxyPres, priFan, coolLiquidPres, coolGasPres, powerTime, oxyTime;

    //2nd astronaut
    TextMeshPro heartRate2, temp2, oxyCons2, co2Prod2, priOxy2, priOxyStor2, batt2, suitPresOxy2, suitTotPres2, suitPresCO22, secOxyStor2, secOxyPres2, priFan2, coolLiquidPres2, coolGasPres2, powerTime2, oxyTime2;

    void Start()
    {
        vitalsUpdateEvent = EventBus.Subscribe<VitalsUpdatedEvent>(onVitalsUpdate);
        fellowVitalsUpdateEvent = EventBus.Subscribe<FellowAstronautVitalsDataChangeEvent>(onFellowVitalsUpdate);


        //I put this creation step in the suit controller dont know if its better,
        //I also put the guage updates there since whats here doesnt really work
        // Astr 1
        //astr1Board = transform.Find("MainVitalBoard").gameObject;
        //astr1BodyBoard = astr1Board.transform.Find("BodyBoard").gameObject;
        //astr1SuitBoard = astr1Board.transform.Find("SuitBoard").gameObject;
        //astr1BackupBoard = astr1Board.transform.Find("BackupBoard").gameObject;
        //astr1BarBoard = astr1Board.transform.Find("BarBoard").gameObject;
        //astr1TimeBoard = astr1Board.transform.Find("RemainingBoard").gameObject;

        //heartRate = astr1BodyBoard.transform.Find("HeartRateRing").Find("HeartRate").GetComponent<TextMeshPro>();
        //temp = astr1BodyBoard.transform.Find("TempRing").Find("Temp").GetComponent<TextMeshPro>();
        //oxyCons = astr1BodyBoard.transform.Find("OxyConsRing").GetComponent<TextMeshPro>();
        //co2Prod = astr1BodyBoard.transform.Find("CO2ProdRing").GetComponent<TextMeshPro>();
        //priOxy = astr1SuitBoard.transform.Find("PriOxyRing").Find("Percent").GetComponent<TextMeshPro>();
        //batt = astr1SuitBoard.transform.Find("BattRing").Find("Percent").GetComponent<TextMeshPro>();
        //suitPresOxy = astr1SuitBoard.transform.Find("SuitPresOxyRing").GetComponent<TextMeshPro>();
        //suitTotPres = astr1SuitBoard.transform.Find("SuitTotPresRing").GetComponent<TextMeshPro>();
        //suitPresCO2 = astr1SuitBoard.transform.Find("SuitPresCO2Ring").GetComponent<TextMeshPro>();
        //secOxyStor = astr1BackupBoard.transform.Find("SecOxyStorRing").Find("Percent").GetComponent<TextMeshPro>();
        //secOxyPres = astr1BackupBoard.transform.Find("SecOxyPresRing").Find("Percent").GetComponent<TextMeshPro>();
        //priFan = astr1BarBoard.transform.Find("rpmVal").GetComponent<TextMeshPro>();
        //coolLiquidPres = astr1BarBoard.transform.Find("liqVal").GetComponent<TextMeshPro>();
        //coolGasPres = astr1BarBoard.transform.Find("gasVal").GetComponent<TextMeshPro>();
        //powerTime = astr1TimeBoard.transform.Find("PowerTime").Find("Btry_timeLeft").GetComponent<TextMeshPro>();
        //oxyTime = astr1TimeBoard.transform.Find("OxyTime").Find("Oxy_timeLeft").GetComponent<TextMeshPro>();

        //// Astr 2
        //astr2Board = transform.Find("FellowVitalBoard").gameObject;
        //astr2BodyBoard = astr2Board.transform.Find("BodyBoard").gameObject;
        //astr2SuitBoard = astr2Board.transform.Find("SuitBoard").gameObject;
        //astr2BackupBoard = astr2Board.transform.Find("BackupBoard").gameObject;
        //astr2BarBoard = astr2Board.transform.Find("BarBoard").gameObject;
        //astr2TimeBoard = astr2Board.transform.Find("RemainingBoard").gameObject;

        //heartRate2 = astr2BodyBoard.transform.Find("HeartRateRing").Find("HeartRate").GetComponent<TextMeshPro>();
        //temp2 = astr2BodyBoard.transform.Find("TempRing").Find("Temp").GetComponent<TextMeshPro>();
        //oxyCons2 = astr2BodyBoard.transform.Find("OxyConsRing").GetComponent<TextMeshPro>();
        //co2Prod2 = astr2BodyBoard.transform.Find("CO2ProdRing").GetComponent<TextMeshPro>();
        //priOxy2 = astr2SuitBoard.transform.Find("PriOxyRing").Find("Percent").GetComponent<TextMeshPro>();
        //batt2 = astr2SuitBoard.transform.Find("BattRing").Find("Percent").GetComponent<TextMeshPro>();
        //suitPresOxy2 = astr2SuitBoard.transform.Find("SuitPresOxyRing").GetComponent<TextMeshPro>();
        //suitTotPres2 = astr2SuitBoard.transform.Find("SuitTotPresRing").GetComponent<TextMeshPro>();
        //suitPresCO22 = astr2SuitBoard.transform.Find("SuitPresCO2Ring").GetComponent<TextMeshPro>();
        //secOxyStor2 = astr2BackupBoard.transform.Find("SecOxyStorRing").Find("Percent").GetComponent<TextMeshPro>();
        //secOxyPres2 = astr2BackupBoard.transform.Find("SecOxyPresRing").Find("Percent").GetComponent<TextMeshPro>();
        //priFan2 = astr2BarBoard.transform.Find("rpmVal").GetComponent<TextMeshPro>();
        //coolLiquidPres2 = astr2BarBoard.transform.Find("liqVal").GetComponent<TextMeshPro>();
        //coolGasPres2 = astr2BarBoard.transform.Find("gasVal").GetComponent<TextMeshPro>();
        //powerTime2 = astr2TimeBoard.transform.Find("PowerTime").Find("Btry_timeLeft").GetComponent<TextMeshPro>();
        //oxyTime2 = astr2TimeBoard.transform.Find("OxyTime").Find("Oxy_timeLeft").GetComponent<TextMeshPro>();

    }

    private void onVitalsUpdate(VitalsUpdatedEvent e)
    {
        // Body
        //heartRate.text = e.vitals.heart_rate.ToString("F0");
        //temp.text = e.vitals.temperature.ToString("F0");
        //oxyCons.text = e.vitals.oxy_consumption.ToString("F1");
        //setGaugeObject(oxyCons);
        //updateGaugeValue((float)e.vitals.oxy_consumption);
        //co2Prod.text = e.vitals.co2_production.ToString("F1");
        //setGaugeObject(co2Prod);
        //updateGaugeValue((float)e.vitals.co2_production);

        //// Suit
        //priOxy.text = e.vitals.oxy_pri_storage.ToString("F0") + "<size=75%>%</size>"; 
        //batt.text = (e.vitals.batt_time_left / BATT_TIME_CAP).ToString("F0"); // Percentage
        //suitPresOxy.text = e.vitals.suit_pressure_oxy.ToString("F1");
        //setGaugeObject(suitPresOxy);
        //updateGaugeValue((float)e.vitals.suit_pressure_oxy);
        //suitTotPres.text = e.vitals.suit_pressure_total.ToString("F1");
        //setGaugeObject(suitTotPres);
        //updateGaugeValue((float)e.vitals.suit_pressure_total);
        //suitPresCO2.text = e.vitals.suit_pressure_co2.ToString("F1");
        //setGaugeObject(suitPresCO2);
        //updateGaugeValue((float)e.vitals.suit_pressure_co2);

        //// Bars
        //priFan.text = e.vitals.fan_pri_rpm.ToString("F0");
        //setBarObject((float)e.vitals.fan_pri_rpm, "fan", astr1BarBoard);
        //coolLiquidPres.text = e.vitals.coolant_liquid_pressure.ToString("F1");
        //setBarObject((float)e.vitals.coolant_liquid_pressure, "liq_psi", astr1BarBoard);
        //coolGasPres.text = e.vitals.coolant_gas_pressure.ToString("F1");
        //setBarObject((float)e.vitals.coolant_gas_pressure, "gas_psi", astr1BarBoard);

        //// Backups
        //secOxyStor.text = e.vitals.oxy_sec_storage.ToString("F0") + "<size=75%>%</size>";
        //secOxyPres.text = e.vitals.oxy_sec_pressure.ToString("F0") + "<size=75%>%</size>";

        //// Time Remaining
        //powerTime.text = FloatToTimeString((float)e.vitals.batt_time_left); // 00:00:00
        //oxyTime.text = FloatToTimeString((float)e.vitals.oxy_time_left); // 00:00:00

        //AstronautInstance.User.VitalsData.batt_percentage = e.vitals.batt_time_left / BATT_TIME_CAP;
        //AstronautInstance.User.VitalsData.oxy_percentage = e.vitals.oxy_time_left / OXY_TIME_CAP;
    }

    private void onFellowVitalsUpdate(FellowAstronautVitalsDataChangeEvent e) {
        // Body
        //heartRate2.text = e.vital.heart_rate.ToString("F0");
        //temp2.text = e.vital.temperature.ToString("F0");
        //oxyCons2.text = e.vital.oxy_consumption.ToString("F1");
        //setGaugeObject(oxyCons2);
        //updateGaugeValue((float)e.vital.oxy_consumption);
        //co2Prod2.text = e.vital.co2_production.ToString("F1");
        //setGaugeObject(co2Prod2);
        //updateGaugeValue((float)e.vital.co2_production);

        //// Suit
        //priOxy2.text = e.vital.oxy_pri_storage.ToString("F0") + "<size=75%>%</size>";
        //batt2.text = (e.vital.batt_time_left / BATT_TIME_CAP).ToString("F0"); // Percentage
        //suitPresOxy2.text = e.vital.suit_pressure_oxy.ToString("F1");
        //setGaugeObject(suitPresOxy2);
        //updateGaugeValue((float)e.vital.suit_pressure_oxy);
        //suitTotPres2.text = e.vital.suit_pressure_total.ToString("F1");
        //setGaugeObject(suitTotPres2);
        //updateGaugeValue((float)e.vital.suit_pressure_total);
        //suitPresCO22.text = e.vital.suit_pressure_co2.ToString("F1");
        //setGaugeObject(suitPresCO22);
        //updateGaugeValue((float)e.vital.suit_pressure_co2);

        //// Bars
        //priFan2.text = e.vital.fan_pri_rpm.ToString("F0");
        //setBarObject((float)e.vital.fan_pri_rpm, "fan", astr2BarBoard);
        //coolLiquidPres2.text = e.vital.coolant_liquid_pressure.ToString("F1");
        //setBarObject((float)e.vital.coolant_liquid_pressure, "liq_psi", astr2BarBoard);
        //coolGasPres2.text = e.vital.coolant_gas_pressure.ToString("F1");
        //setBarObject((float)e.vital.coolant_gas_pressure, "gas_psi", astr2BarBoard);

        //// Backups
        //secOxyStor2.text = e.vital.oxy_sec_storage.ToString("F0") + "<size=75%>%</size>";
        //secOxyPres2.text = e.vital.oxy_sec_pressure.ToString("F0") + "<size=75%>%</size>";

        //// Time Remaining
        //powerTime2.text = FloatToTimeString((float)e.vital.batt_time_left); // 00:00:00
        //oxyTime2.text = FloatToTimeString((float)e.vital.oxy_time_left); // 00:00:00
    }

    private string FloatToTimeString(float timeInSeconds)
    {
        int hours = (int)(timeInSeconds / 3600);
        int minutes = (int)((timeInSeconds % 3600) / 60);
        int seconds = (int)(timeInSeconds % 60);

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
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
        //setGaugeColor(incomingGaugeValue);

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
    void setBarObject(float barValue, string type, GameObject parent)
    {
        float percentage = 0f;
        if (type == "liq_psi")
        {
            indicator = parent.transform.Find("Liq_bar").gameObject.transform.Find("Indicator").gameObject;
            percentage = barValue / 500f;
        }
        if (type == "gas_psi")
        {
            indicator = parent.transform.Find("gas_bar").gameObject.transform.Find("Indicator").gameObject;
            percentage = barValue / 500f;
        }
        if (type == "fan")
        {
            indicator = parent.transform.Find("fan_bar").gameObject.transform.Find("Indicator").gameObject;
            percentage = barValue / 50000f;
        }

        MoveIndicator(percentage, indicator);
    }


    // -1.86 -> 10.13

    float calculatePSI(float barValue)
    {
        return (float)Math.Min((barValue / 6) * 10, 10);
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
}