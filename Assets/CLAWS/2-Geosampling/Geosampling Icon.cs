using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoSampleIcon : MonoBehaviour
{
    public GameObject prefab;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = transform.position;
        position.y = 6.86f; 
        Instantiate(prefab, position, Quaternion.identity);

    }
}
