using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBarTransformReference : MonoBehaviour
{
    public Transform CorTransformReference;

    void Update()
    {
        transform.position = CorTransformReference.position;
        transform.rotation = Quaternion.Euler(
            Camera.main.transform.rotation.eulerAngles.x,
            CorTransformReference.rotation.eulerAngles.y,
            Camera.main.transform.rotation.eulerAngles.z);
    }
}
