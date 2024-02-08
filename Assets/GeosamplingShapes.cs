using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeosamplingShapes : MonoBehaviour
{

    public GeosamplingShape.Shape CurrentShape;

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
        CurrentShape = GeosamplingShape.Shape.None;
    }

    public void SetShape(GeosamplingShape.Shape shape_in)
    {
        // deactivate old icon
        
        None_icon.SetActive(false);                
        Polygon_icon.SetActive(false);
        Cube_icon.SetActive(false);
        Cylinder_icon.SetActive(false);
        Cone_icon.SetActive(false);
        Sphere_icon.SetActive(false);
        Crustalline_icon.SetActive(false);
        Ellipsoid_icon.SetActive(false);
        Irregular_icon.SetActive(false);
        
        // activate new icon
        switch (shape_in)
        {
            case GeosamplingShape.Shape.None:
                None_icon.SetActive(true);
                Shape_tmp.text = "Shape";
                break;
            case GeosamplingShape.Shape.Polygon:
                Polygon_icon.SetActive(true);
                Shape_tmp.text = "Polygon";
                break;
            case GeosamplingShape.Shape.Cube:
                Cube_icon.SetActive(true);
                Shape_tmp.text = "Cube";
                break;
            case GeosamplingShape.Shape.Cylinder:
                Cylinder_icon.SetActive(true);
                Shape_tmp.text = "Cylinder";
                break;
            case GeosamplingShape.Shape.Cone:
                Cone_icon.SetActive(true);
                Shape_tmp.text = "Cone";
                break;
            case GeosamplingShape.Shape.Sphere:
                Sphere_icon.SetActive(true);
                Shape_tmp.text = "Sphere";
                break;
            case GeosamplingShape.Shape.Crystalline:
                Crustalline_icon.SetActive(true);
                Shape_tmp.text = "Crystalline";
                break;
            case GeosamplingShape.Shape.Ellipsoid:
                Ellipsoid_icon.SetActive(true);
                Shape_tmp.text = "Ellipsoid";
                break;
            case GeosamplingShape.Shape.Irregular:
                Irregular_icon.SetActive(true);
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
        SetShape(GeosamplingShape.Shape.Polygon);
    }
}
