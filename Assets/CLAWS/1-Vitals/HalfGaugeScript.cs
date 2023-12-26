using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HalfGaugeScript : MonoBehaviour
{
    GameObject halfGauge;
    GameObject halfRingPartialFiller;
    GameObject halfRingFullFiller;
    SpriteRenderer halfRingPartialFillerSprite;
    SpriteRenderer halfRingFullFillerSprite;
    GameObject halfRing;
    TextMeshPro gaugeTitle;
    TextMeshPro gaugeValue;
    TextMeshPro gaugeUnit;

    // Placeholder values for the gauge current value and max value. Currently for pressure (psi) from 0.0-7.0
    public float gaugeMaxValue = 6.0f;
    public float value = 0.0f;

    private IEnumerator testCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        halfGauge = GameObject.Find("HalfGauge");
        halfRing = halfGauge.transform.Find("HalfRing").gameObject;
        gaugeValue = halfGauge.transform.Find("HalfGaugeValue").gameObject.GetComponent<TextMeshPro>();
        gaugeUnit = halfGauge.transform.Find("HalfGaugeUnit").gameObject.GetComponent<TextMeshPro>();

        // This ring filler is toggled on when the gauge value is between 0-50%
        halfRingPartialFiller = halfRing.transform.Find("HalfRingPartialFiller").gameObject;
        halfRingPartialFillerSprite = halfRingPartialFiller.GetComponent<SpriteRenderer>(); 

        //This ring filler is toggled on when the gauge value is >= 50%
        halfRingFullFiller = halfRing.transform.Find("HalfRingFullFiller").gameObject;
        halfRingFullFillerSprite = halfRingFullFiller.GetComponent<SpriteRenderer>();

        testCoroutine = _UpdateGauge();
        StartCoroutine(testCoroutine);
    }

    IEnumerator _UpdateGauge()
    {
        while (true)
        {
            //Comment the line below before testing values in the inspector
            value = value + 0.1f;
            setGaugeColor(value);

            yield return new WaitForSeconds(1.0f);
            gaugeValue.text = Math.Round(value,1).ToString();


            float percentValue = calculatePercentage(value);
            float degreeRotation = calculateRotation(percentValue);
            if (percentValue <= 50)
            {
                //Fetch current Z rotational value of the filler and subtract that before update
                float currentRotationZ = halfRingPartialFiller.transform.rotation.eulerAngles.z - 90;

                //Show partial ring filler, hide full ring filler
                halfRingFullFiller.SetActive(false);
                halfRingPartialFiller.SetActive(true);
                halfRingPartialFiller.transform.Rotate(0.0f, 0.0f, degreeRotation-currentRotationZ, Space.Self);
            }
            else
            {
                //Fetch current Z rotational value of the filler and subtract that before update
                float currentRotationZ = halfRingFullFiller.transform.rotation.eulerAngles.z - 180;

                //Hide partial ring filler, show full ring filler
                halfRingFullFiller.SetActive(true);
                halfRingPartialFiller.SetActive(false);
                halfRingFullFiller.transform.Rotate(0.0f, 0.0f, degreeRotation-currentRotationZ, Space.Self);

            }
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
}
