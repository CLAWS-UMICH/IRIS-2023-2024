using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAPanel : MonoBehaviour
{
    public TextMeshPro UIA_tmp;
    public SpriteRenderer spriteRenderer;

    public List<Transform> Cubes;

    float extents_x;
    float extents_y;

    private void Start()
    {
        extents_x = spriteRenderer.bounds.extents.x * 2;
        extents_y = spriteRenderer.bounds.extents.y * 2;
    }

    // 1 2
    // 4 3
    public void Rescale(Vector3 corner1_f, Vector3 corner2_f, Vector3 corner3_f, Vector3 corner4_f)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Calculate position
        Vector3 position = (corner1_f + corner2_f + corner3_f + corner4_f) / 4f;

        // Calculate scale (assumes two adjacent corners)
        Vector3 scale = new Vector3(
            Vector3.Distance(corner1_f, corner2_f) / extents_x,
            Vector3.Distance(corner1_f, corner4_f) / extents_y, 
            0.1f);

        // Calculate rotation (assumes first corner to last corner)
        Vector3 thirdOrthogonalVector = Vector3.Cross(corner2_f - corner1_f, corner4_f - corner1_f);
        Quaternion rotation = Quaternion.LookRotation(thirdOrthogonalVector, Vector3.up);

        // Apply transformations
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }

    [ContextMenu("func rescale")]
    public void debug_rescale()
    {
        List<Vector3> Positions = new();
        foreach (Transform c in Cubes)
        {
            Positions.Add(c.position);
        }

        var topy = (Positions[0].y + Positions[1].y) / 2;
        var bottomy = (Positions[2].y + Positions[3].y) / 2;

        var left = (Positions[0] + Positions[3]) / 2;
        var right = (Positions[1] + Positions[2]) / 2;

        Positions[0] = new Vector3(left.x, topy, left.z);
        Positions[1] = new Vector3(right.x, topy, right.z);
        Positions[2] = new Vector3(right.x, bottomy, right.z);
        Positions[3] = new Vector3(left.x, bottomy, left.z);

        Cubes[0].position = Positions[0];
        Cubes[1].position = Positions[1];
        Cubes[2].position = Positions[2];
        Cubes[3].position = Positions[3];

        Rescale(
            Positions[0],
            Positions[1],
            Positions[2],
            Positions[3]
        );
    }
}
