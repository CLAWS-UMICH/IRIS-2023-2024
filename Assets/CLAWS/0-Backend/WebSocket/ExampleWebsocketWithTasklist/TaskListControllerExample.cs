using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskListControllerExample : MonoBehaviour
{

    private Subscription<TasksDeletedEvent> tasksDeletedEvent;
    private Subscription<TasksEditedEvent> tasksEditedEvent;
    private Subscription<TasksAddedEvent> tasksAddedEvent;

    public GameObject taskListButtonPrefab;
    private GameObject sh;
    private TextMeshPro id, title, description, shared_with, status;
    private Dictionary<int, GameObject> idToButton = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        id = taskListButtonPrefab.transform.Find("ID").gameObject.GetComponent<TextMeshPro>();
        status = taskListButtonPrefab.transform.Find("Status").gameObject.GetComponent<TextMeshPro>();
        title = taskListButtonPrefab.transform.Find("Title").gameObject.GetComponent<TextMeshPro>();
        description = taskListButtonPrefab.transform.Find("Description").gameObject.GetComponent<TextMeshPro>();
        shared_with = taskListButtonPrefab.transform.Find("SharedWith").gameObject.GetComponent<TextMeshPro>();

        sh = GameObject.Find("ScrollObjects");

        // Subscribe to the events
        tasksDeletedEvent = EventBus.Subscribe<TasksDeletedEvent>(OnTasksDeleted);
        tasksEditedEvent = EventBus.Subscribe<TasksEditedEvent>(OnTasksEdited);
        tasksAddedEvent = EventBus.Subscribe<TasksAddedEvent>(OnTasksAdded);
    }

    void OnDestroy()
    {
        // Unsubscribe when the script is destroyed
        if (tasksDeletedEvent != null)
        {
            EventBus.Unsubscribe(tasksDeletedEvent);
        }
        if (tasksEditedEvent != null)
        {
            EventBus.Unsubscribe(tasksEditedEvent);
        }
        if (tasksAddedEvent != null)
        {
            EventBus.Unsubscribe(tasksAddedEvent);
        }
    }

    private void OnTasksDeleted(TasksDeletedEvent e)
    {
        //Debug.Log("Deleted");
        List<TaskObj> deletedTasks = e.DeletedTasks; // Which waypoints were deleted (Look at their id's)
        foreach (TaskObj task in deletedTasks)
        {
            if (idToButton.ContainsKey(task.task_id))
            {
                sh.GetComponent<ScrollHandler>().HandleButtonDeletion(idToButton[task.task_id]);
                idToButton.Remove(task.task_id);
            }
        }

    }

    private void OnTasksEdited(TasksEditedEvent e)
    {
        //Debug.Log("Edited");
        List<TaskObj> editTasks = e.EditedTasks; // Which waypoints were edited (Look at their id's)

        foreach (TaskObj task in editTasks)
        {
            if (idToButton.ContainsKey(task.task_id))
            {
                GameObject buttonToEdit = idToButton[task.task_id];
                buttonToEdit.transform.Find("ID").gameObject.GetComponent<TextMeshPro>().text = "ID: " + task.task_id.ToString();
                if (task.status == 0)
                {
                    buttonToEdit.transform.Find("Status").gameObject.GetComponent<TextMeshPro>().text = "Status: In Progress";
                }
                else
                {
                    buttonToEdit.transform.Find("Status").gameObject.GetComponent<TextMeshPro>().text = "Status: Completed";
                }
                buttonToEdit.transform.Find("Title").gameObject.GetComponent<TextMeshPro>().text = "Title: " + task.title.ToString();
            }
        }

    }

    private void OnTasksAdded(TasksAddedEvent e)
    {
        //Debug.Log("Added");
        List<TaskObj> newTasks = e.NewAddedTasks; // Which waypoints are new

        foreach (TaskObj task in newTasks)
        {
            id.text = "ID: " + task.task_id.ToString();
            if (task.status == 0)
            {
                status.text = "Status: In Progress";
            } else
            {
                status.text = "Status: Completed";
            }
            title.text = "Title: " + task.title.ToString();
            GameObject newButton = sh.GetComponent<ScrollHandler>().HandleAddingButton(taskListButtonPrefab);
            idToButton.Add(task.task_id, newButton);
        }
    }
}
