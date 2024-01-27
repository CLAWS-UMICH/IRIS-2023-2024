using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScroll : MonoBehaviour
{
    [SerializeField] private float spacing = 0.1f; // Distance between gameobjects
    [SerializeField] private int buttonsEnabledCount = 3; // Number of gameobjects to scroll per button press

    private List<GameObject> allButtons = new List<GameObject>(); // List to store all buttons
    private Dictionary<GameObject, bool> gameObjectMap = new Dictionary<GameObject, bool>();

    public GameObject HandleAddingButton(GameObject newButton)
    {
        GameObject button = Instantiate(newButton, transform);
        allButtons.Add(button);
        gameObjectMap.Add(button, false);
        RefreshLayout();

        return button;
    }

    private IEnumerator SmoothMove(GameObject obj, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = obj.transform.position;

        if (gameObjectMap.ContainsKey(obj) && gameObjectMap[obj])
        {
            while (elapsedTime < duration && obj != null && !ReferenceEquals(obj, null))
            {
                obj.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        if (obj != null && !ReferenceEquals(obj, null))
        {
            gameObjectMap[obj] = true;

            obj.transform.position = targetPosition;
        }
    }

    private void RefreshLayout()
    {
        int count = 0;
        float prevOffset = 0;
        GameObject prevButton = null;
        foreach (GameObject obj in allButtons)
        {
            if (obj != null && !ReferenceEquals(obj, null))
            {
                if (count < buttonsEnabledCount)
                {
                    BoxCollider currentCollider = obj.GetComponent<BoxCollider>();
                    float yOffset = 0;
                    if (prevButton)
                    {
                        // Assuming you are using BoxCollider for simplicity. Adjust accordingly for other collider types.
                        BoxCollider prevCollider = prevButton.GetComponent<BoxCollider>();

                        if (prevCollider && currentCollider)
                        {
                            float prevHeight = prevCollider.size.y * prevButton.transform.localScale.y;
                            float currentHeight = currentCollider.size.y * obj.transform.localScale.y;

                            yOffset = prevOffset - ((prevHeight / 2f) + (currentHeight / 2f) + (spacing / 100));
                        }
                    }
                   

                    float xOffset = (currentCollider.size.x * obj.transform.localScale.x) / 2f;


                    Vector3 newPosition = transform.position + new Vector3(xOffset, yOffset, 0f);

                    // Use Coroutine for smooth movement
                    StartCoroutine(SmoothMove(obj, newPosition, 0.5f));

                    obj.SetActive(true);
                    prevButton = obj;
                    prevOffset = yOffset;
                }
                else
                {
                    obj.SetActive(false);
                }
                count++;
            }
        }
    }

    // Function to handle button deletion based on GameObject
    public void HandleButtonDeletion(GameObject deletedButton)
    {
        if (allButtons.Remove(deletedButton))
        {
            Destroy(deletedButton);
            gameObjectMap.Remove(deletedButton);
            RefreshLayout();
        }
        else
        {
            Debug.LogWarning("Deleted button not found in the list.");
        }
    }

}
