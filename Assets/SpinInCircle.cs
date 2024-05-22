using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinInCircle : MonoBehaviour
{
    IEnumerator Spin()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            transform.Rotate(Vector3.up, 3f);
        }
    }
    void Start()
    {
        StartCoroutine(Spin());
    }

}
