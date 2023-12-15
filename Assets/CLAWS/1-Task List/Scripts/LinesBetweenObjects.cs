using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LinesBetweenObjects : MonoBehaviour
{
    [SerializeField] public List<GameObject> Objects; // objects must be in order and have boxcollider 2d
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Collider ClippingBounds;

    private void Start()
    {
        StartCoroutine(UpdateLines());
    }

    [ContextMenu("func DrawLines")]
    public void DrawLines()
    {
        ClearLines();

        float max = ClippingBounds.bounds.max.y;
        float min = ClippingBounds.bounds.min.y;

        // draw lines
        for (int i = 1; i < Objects.Count; i++)
        {
            float y_start = Objects[i - 1].transform.position.y
                            - (Objects[i - 1].GetComponent<BoxCollider2D>().size.y / 2
                            * Objects[i - 1].transform.localScale.y) - 0.001f;
            float y_end = Objects[i].transform.position.y
                            + (Objects[i].GetComponent<BoxCollider2D>().size.y / 2
                            * Objects[i].transform.localScale.y) + 0.001f;
            
            // clipping
            y_start = Math.Min(max, y_start);
            y_end = Math.Max(min, y_end);
            if (y_start <= y_end)
            {
                continue;
            }

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

        // delete any null obj in Objects
        for (int i = Objects.Count - 1; i >= 0; i--)
        {
            if (Objects[i] == null)
            {
                Objects.RemoveAt(i);
            }
        }

        // remove lines
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

    IEnumerator UpdateLines()
    {
        // TODO we will need to figure out a more efficient way besides line renderer
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            DrawLines();
        }
    }

}
