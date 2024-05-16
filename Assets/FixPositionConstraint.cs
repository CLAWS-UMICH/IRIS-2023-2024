using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPositionConstraint : MonoBehaviour
{
    /// <summary>
    /// Fix the *local* position based on x, y, z
    /// </summary>
    /// 

    public bool x;
    public bool y;
    public bool z;

    Vector3 OriginalPosition;

    void Start()
    {
        OriginalPosition = transform.localPosition;
    }

    void Update()
    {
        Vector3 newPosition = transform.localPosition;
        if (x)
        {
            newPosition.x = OriginalPosition.x;
        }
        if (y)
        {
            newPosition.y = OriginalPosition.y;
        }
        if (z)
        {
            newPosition.z = OriginalPosition.z;
        }
        transform.localPosition = newPosition;
    }
}
