using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixScrollZ : MonoBehaviour
{
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }
}
