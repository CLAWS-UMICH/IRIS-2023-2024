using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HalfGaugeScript : MonoBehaviour
{
    GameObject halfGauge;
    TextMeshPro gaugeTitle;
    TextMeshPro gaugeValue;
    TextMeshPro gaugeUnit;
    private IEnumerator testCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        halfGauge = GameObject.Find("HalfGauge");
        gaugeValue = halfGauge.transform.Find("HalfGaugeValue").gameObject.GetComponent<TextMeshPro>();
        gaugeUnit= halfGauge.transform.Find("HalfGaugeValue").gameObject.GetComponent<TextMeshPro>();
        gaugeValue.text = "0.0";
        testCoroutine = _UpdateGauge();
        StartCoroutine(testCoroutine);
    }

    IEnumerator _UpdateGauge()
    {
        double value = 1.0f;
        while (true)
        {
            value = value + 1.0f;
            yield return new WaitForSeconds(1.0f);
            gaugeValue.text = value.ToString();
        }
    }
}
