using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HalfGaugeScript : MonoBehaviour
{
    GameObject halfGauge;
    GameObject halfRingPartialFiller;
    GameObject halfRingFullFiller;
    GameObject halfRing;
    TextMeshPro gaugeTitle;
    TextMeshPro gaugeValue;
    TextMeshPro gaugeUnit;
    float gaugeMaxValue = 100.0f;
    private IEnumerator testCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        halfGauge = GameObject.Find("HalfGauge");
        halfRing = halfGauge.transform.Find("HalfRing").gameObject;
        gaugeValue = halfGauge.transform.Find("HalfGaugeValue").gameObject.GetComponent<TextMeshPro>();
        gaugeUnit = halfGauge.transform.Find("HalfGaugeUnit").gameObject.GetComponent<TextMeshPro>();

        // This ring filler is toggled on when the gauge value is 0-50%
        halfRingPartialFiller = halfRing.transform.Find("HalfRingPartialFiller").gameObject;

        //This ring filler is toggled on when the gauge value is >= 50%
        halfRingFullFiller = halfRing.transform.Find("HalfRingFullFiller").gameObject;

        gaugeValue.text = "0.0";
        testCoroutine = _UpdateGauge();
        StartCoroutine(testCoroutine);
    }

    IEnumerator _UpdateGauge()
    {
        float value = 1.0f;
        while (true)
        {
            value = value + 1.0f;
            yield return new WaitForSeconds(1.0f);
            gaugeValue.text = value.ToString();

            float percentValue = calculatePercentage(value);
            if (percentValue <= 50)
            {
                //Show partial ring filler, hide full ring filler
                float degreeRotation = calculatePartialRingRotation(percentValue);
                halfRingFullFiller.SetActive(false);
                halfRingPartialFiller.SetActive(true);
                halfRingPartialFiller.transform.Rotate(0.0f, 0.0f, degreeRotation, Space.Self);
            }
            else
            {
                //Hide partial ring filler, show full ring filler
                float degreeRotation = calculateFullRingRotation(percentValue);
                halfRingFullFiller.SetActive(true);
                halfRingPartialFiller.SetActive(false);
                halfRingFullFiller.transform.Rotate(0.0f, 0.0f, degreeRotation, Space.Self);

            }
        }
    }

    float calculatePercentage(float gaugeValue)
    {
        return (gaugeValue / gaugeMaxValue) * 100;
    }
    float calculatePartialRingRotation(float percentage)
    {
        return -(percentage / 100) * 180;
    }
    float calculateFullRingRotation(float percentage)
    {
        return (percentage / 100) * 180;
    }

    /*    void toggleGauge(float gaugeValue)
        {

        }*/
}
