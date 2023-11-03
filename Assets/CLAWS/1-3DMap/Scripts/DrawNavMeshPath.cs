using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class DrawNavMeshPath : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform targetPosition;
    public GameObject prefab;
    private List<GameObject> instantiatedObjects = new List<GameObject>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void DrawPath()
    {
        ClearPath();
        NavMeshPath path = new NavMeshPath();
        Vector3[] corners;

        if (agent.CalculatePath(targetPosition.position, path))
        {
            corners = path.corners;

            for (int i = 0; i < corners.Length - 1; i++)
            {
                Vector3 direction = (corners[i + 1] - corners[i]).normalized;
                float distance = Vector3.Distance(corners[i], corners[i + 1]);

                while (distance >= 1f)
                {
                    Vector3 newPosition = corners[i] + direction * 1f;
                    GameObject instantiatedObject = Instantiate(prefab, newPosition, Quaternion.identity);
                    instantiatedObjects.Add(instantiatedObject);
                    distance -= 1f;
                    corners[i] = newPosition;
                }
            }
        }
    }

    void ClearPath()
    {
        foreach (GameObject obj in instantiatedObjects)
        {
            // Destroy each object in the list
            Destroy(obj);
        }

        // Clear the list after deleting all objects
        instantiatedObjects.Clear();
    }
}
