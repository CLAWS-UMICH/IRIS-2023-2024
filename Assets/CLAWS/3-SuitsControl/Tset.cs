using System;
using System.Collections;
using UnityEngine;

public class DummyValueTester : MonoBehaviour
{
    public RadialProgress O2_percent;
    public RadialProgress heartRate;
    public RadialProgress O2_pressure;

    private float updateInterval = 0.5f;
    private float O2_initialValue = 100f;
    private float hr_initialValue = 90f;
    private float pr_initialValue = 2000f;

    private float O2_currentValue;
    private float hr_currentValue;
    private float pr_currentValue;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the current value to the initial value
        O2_currentValue = O2_initialValue;
        hr_currentValue = hr_initialValue;
        pr_currentValue = pr_initialValue;

        // Start the coroutine to simulate parameter updates
        StartCoroutine(UpdateParameterCoroutine());
    }

    // Coroutine to simulate parameter updates every few seconds
    IEnumerator UpdateParameterCoroutine()
    {
        while (O2_currentValue > 0 && hr_currentValue > 0 && pr_currentValue > 0) // Continue updating while the current value is greater than 0
        {
            // Update the circle fill using the RadialProgress script
            O2_percent.UpdatePercent(O2_currentValue);
            heartRate.UpdateHR(hr_currentValue);
            O2_pressure.UpdatePR(pr_currentValue);

            // Wait for the specified update interval
            yield return new WaitForSeconds(updateInterval);

            // Decrease the current value by a certain amount (e.g. 5)
            O2_currentValue -= 5f;
            hr_currentValue -= 5f;
            pr_currentValue -= 5f;

            // Make sure the current value does not go below 0
            if (O2_currentValue > 0 && hr_currentValue < 0 && pr_currentValue < 0)
            {
                O2_currentValue = 0;
                hr_currentValue = 0;
                pr_currentValue = 0;
            }
        }
    }
}
