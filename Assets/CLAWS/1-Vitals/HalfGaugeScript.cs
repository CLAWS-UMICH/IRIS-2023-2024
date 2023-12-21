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

    //Placeholder values for the gauge current value and max value. Will need to be fetched for prod
    public float gaugeMaxValue = 100.0f;
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

        //This ring filler is toggled on when the gauge value is >= 50%
        halfRingFullFiller = halfRing.transform.Find("HalfRingFullFiller").gameObject;

        testCoroutine = _UpdateGauge();
        StartCoroutine(testCoroutine);
    }

    IEnumerator _UpdateGauge()
    {
        while (true)
        {
            //Comment the line below before testing values in the inspector
            value = value + 1.0f;

            yield return new WaitForSeconds(1.0f);
            gaugeValue.text = value.ToString();
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
        return (gaugeValue / gaugeMaxValue) * 100;
    }
    float calculateRotation(float percentage)
    {
        return -(percentage / 100) * 180;
    }


}
