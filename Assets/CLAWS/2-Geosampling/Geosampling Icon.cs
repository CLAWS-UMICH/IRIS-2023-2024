using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoSampleIcon : MonoBehaviour
{
    public GameObject marker;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    void SetIcon()
    {
        Vector3 position = transform.position;
        position.y = 6.86f;
        marker.transform.position = position;
    }
}
