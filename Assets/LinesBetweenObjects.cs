using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesBetweenObjects : MonoBehaviour
{
    [SerializeField] public List<GameObject> Objects; // objects must be in order and have boxcollider 2d
    [SerializeField] private GameObject linePrefab; 

    [ContextMenu("func DrawLines")]
    public void DrawLines()
    {
        ClearLines();

        // draw lines
        for (int i = 1; i < Objects.Count; i++)
        {
            float y_start = Objects[i - 1].transform.position.y
                            - (Objects[i - 1].GetComponent<BoxCollider2D>().size.y / 2
                            * Objects[i - 1].transform.localScale.y) - 0.001f;
            float y_end = Objects[i].transform.position.y
                            + (Objects[i].GetComponent<BoxCollider2D>().size.y / 2
                            * Objects[i].transform.localScale.y) + 0.001f;
            Vector3[] positions = {
                new Vector3(Objects[i - 1].transform.position.x, y_start, Objects[i - 1].transform.position.z),
                new Vector3(Objects[i].transform.position.x, y_end, Objects[i].transform.position.z)
            };
            LineRenderer line = Instantiate(linePrefab, transform).GetComponent<LineRenderer>();
            line.SetPositions(positions);
        }
    }

    private void ClearLines()
    {
        List<GameObject> children = new();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(child);
            }
            else
#endif
            {
                Destroy(child);
            }
        }
    }
}
