using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListScrollHandler : MonoBehaviour
{
    [SerializeField] private float spacing = 0.1f; // Distance between gameobjects
    [SerializeField] private float lerp = 0.1f;

    [SerializeField] private BoxCollider Bounds;
    [SerializeField] private Transform Content;

    private List<Transform> allButtons = new List<Transform>(); // List to store all buttons

    private Vector3 startBounds;
    private Vector3 endBounds;
    private float colliderOffset;

    private void Start()
    {
        colliderOffset = Bounds.center.y;

        startBounds = transform.localPosition;
        endBounds = startBounds;

        FixLocations();
    }

    private void Update()
    {
        // update the box collider to be scrollable
        Bounds.center = new Vector3(Bounds.center.x, colliderOffset - Bounds.transform.localPosition.y, Bounds.center.z);

        if (Content.localPosition.y < 0)
        {
            Content.localPosition = Vector3.Lerp(Content.localPosition, startBounds, lerp);
        }
        else if (Content.localPosition.y > endBounds.y) 
        {
            Content.localPosition = Vector3.Lerp(Content.localPosition, endBounds, lerp);
        }
    }

    /// <summary>
    /// Adds all buttons to a list. Used in case of new buttons added in the middle of the run.
    /// </summary>
    void CollectAllButtons()
    {
        Transform parentTransform = transform;
        allButtons.Clear();

        foreach (Transform child in parentTransform)
        {
            allButtons.Add(child);
        }
    }

    /// <summary>
    /// Fixes the layout of all objects in the scroll handler.
    /// </summary>
    [ContextMenu("Func FixLayout")]
    public void FixLocations()
    {
        CollectAllButtons();

        for (int i = 0; i < allButtons.Count; i++)
        {
            float yOffset;

            if (i == 0)
            {
                yOffset = 0;
            }
            else
            {
                yOffset = allButtons[i - 1].transform.localPosition.y
                    - (allButtons[i - 1].GetComponent<BoxCollider>().size.y / 2
                        * allButtons[i - 1].transform.localScale.y)
                    - (allButtons[i].GetComponent<BoxCollider>().size.y / 2
                        * allButtons[i].transform.localScale.y)
                    - spacing;
            }

            // Vector3 newPosition = parentTransform.position + new Vector3(xOffset, yOffset, 0f);
            Vector3 newPosition = new Vector3(allButtons[i].transform.localPosition.x, yOffset, allButtons[i].transform.localPosition.z);
            allButtons[i].transform.localPosition = newPosition; // Move each button to the new position
        }

        if (allButtons.Count > 0)
        {
            endBounds = new Vector3(endBounds.x,
                -allButtons[allButtons.Count - 1].transform.localPosition.y
                + (allButtons[allButtons.Count - 1].GetComponent<BoxCollider>().size.y / 2
                            * allButtons[allButtons.Count - 1].transform.localScale.y),
                endBounds.z);
        }
    }

}
