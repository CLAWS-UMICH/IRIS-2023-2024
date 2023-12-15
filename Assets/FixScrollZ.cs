using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixScrollZ : MonoBehaviour
{
    [SerializeField] float z;

    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    }
}
