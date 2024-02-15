using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    // Start is called before the first frame update
    public float radius = 3;
    public float circleThickness = 0.02f;

    private void Start()
    {

        var boundary = new GameObject { name = "GeoSample Boundary" };
        var lineRenderer = boundary.AddComponent<LineRenderer>();

        var circleSegments = 360;
        lineRenderer.material.color = new Color(99f/255, 131f/255, 216f/255); // color from figma #6992D8
        lineRenderer.useWorldSpace = true; // lines render around world origin
        lineRenderer.startWidth = circleThickness; // consistent circle thickness
        lineRenderer.endWidth = circleThickness; // consistent circle thickness
        lineRenderer.positionCount = circleSegments + 1; // closes the circle

        Vector3 position = transform.position;

        var pointCount = circleSegments + 1;
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; ++i)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / circleSegments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius) + position;
        }

        lineRenderer.SetPositions(points);
    }

}
