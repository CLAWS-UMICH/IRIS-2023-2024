using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeosamplingShapes : MonoBehaviour
{
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

    public Shape CurrentShape;

    public GameObject None_icon;
    public GameObject Polygon_icon;
    public GameObject Cylinder_icon;
    public GameObject Cube_icon;
    public GameObject Cone_icon;
    public GameObject Sphere_icon;
    public GameObject Crustalline_icon;
    public GameObject Ellipsoid_icon;
    public GameObject Irregular_icon;

    public TextMeshPro Shape_tmp;

    private void Start()
    {
        CurrentShape = Shape.None;
    }

    public void SetShape(Shape shape_in)
    {
        // deactivate old icon
        switch (CurrentShape)
        {
            case Shape.None:
                None_icon.SetActive(false);
                break;
            case Shape.Polygon:
                Polygon_icon.SetActive(false);
                break;
            case Shape.Cube:
                Cube_icon.SetActive(false);
                break;
            case Shape.Cylinder:
                Cylinder_icon.SetActive(false);
                break;
            case Shape.Cone:
                Cone_icon.SetActive(false);
                break;
            case Shape.Sphere:
                Sphere_icon.SetActive(false);
                break;
            case Shape.Crystalline:
                None_icon.SetActive(false);
                break;
            case Shape.Ellipsoid:
                None_icon.SetActive(false);
                break;
            case Shape.Irregular:
                None_icon.SetActive(false);
                break;
            default:
                Debug.LogError("Shape error");
                break;
        }
        // activate new icon
        switch (shape_in)
        {
            case Shape.None:
                None_icon.SetActive(true);
                Shape_tmp.text = "Shape";
                break;
            case Shape.Polygon:
                Polygon_icon.SetActive(true);
                Shape_tmp.text = "Polygon";
                break;
            case Shape.Cube:
                Cube_icon.SetActive(true);
                Shape_tmp.text = "Cube";
                break;
            case Shape.Cylinder:
                Cylinder_icon.SetActive(true);
                Shape_tmp.text = "Cylinder";
                break;
            case Shape.Cone:
                Cone_icon.SetActive(true);
                Shape_tmp.text = "Cone";
                break;
            case Shape.Sphere:
                Sphere_icon.SetActive(true);
                Shape_tmp.text = "Sphere";
                break;
            case Shape.Crystalline:
                None_icon.SetActive(true);
                Shape_tmp.text = "Crystalline";
                break;
            case Shape.Ellipsoid:
                None_icon.SetActive(true);
                Shape_tmp.text = "Ellipsoid";
                break;
            case Shape.Irregular:
                None_icon.SetActive(true);
                Shape_tmp.text = "Irregular";
                break;
            default:
                Debug.LogError("Shape error");
                Shape_tmp.text = "Error!";
                break;
        }
    }

    [ContextMenu("func SetShapeTest")]
    public void SetShapeTest()
    {
        SetShape(Shape.Polygon);
    }
}
