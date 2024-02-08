using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosamplingShape : MonoBehaviour
{
    [System.Serializable]
    public enum Shape
    {
        None,
        Polygon,
        Cylinder,
        Cube,
        Cone,
        Sphere,
        Crystalline,
        Ellipsoid,
        Irregular
    }

    public Shape shape;
}
