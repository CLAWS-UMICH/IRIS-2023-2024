using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateCurrentTask : MonoBehaviour
{
    // Start is called before the first frame update
    private Subscription<TaskStartedEvent> taskStartedEvent;
    GameObject screen;
    TextMeshPro currTask;

    void Start()
    {
        taskStartedEvent = EventBus.Subscribe<TaskStartedEvent>(OnUpdateCurrentTask);
        screen = GameObject.Find("curr_task_screen");
        currTask = screen.transform.Find("curr_task").gameObject.GetComponent<TextMeshPro>();
    }

    void OnDestroy()
    {
        // Unsubscribe when the script is destroyed
        if (taskStartedEvent != null)
        {
            EventBus.Unsubscribe(taskStartedEvent);
        }

    }
    private void OnUpdateCurrentTask(TaskStartedEvent e)
    {
        Debug.Log("Test");
        TaskObj task = e.StartedTask;
        currTask.text = task.title.ToString();

        // Update the UI to reflect the new vitals values
    }
}
