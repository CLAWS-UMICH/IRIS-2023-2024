using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListBackend : MonoBehaviour
{

    private Subscription<TaskFinishedEvent> taskFinishedEvent;
    private Subscription<TasksDeletedEvent> tasksDeletedEvent;
    private Subscription<TasksEditedEvent> tasksEditedEvent;
    private Subscription<TasksAddedEvent> tasksAddedEvent;

    void Start()
    {
        taskFinishedEvent = EventBus.Subscribe<TaskFinishedEvent>(OnTaskFinished);
        tasksDeletedEvent = EventBus.Subscribe<TasksDeletedEvent>(SetCurrentTask);
        tasksEditedEvent = EventBus.Subscribe<TasksEditedEvent>(SetCurrentTask);
        tasksAddedEvent = EventBus.Subscribe<TasksAddedEvent>(SetCurrentTask);

        SetCurrentTask<bool>(true);
    }

    void SetCurrentTask<T>(T e)
    {
        IEnumerator _SetCurrentTask()
        {
            yield return new WaitForSeconds(0);

            bool current_task_set = false;
            foreach (TaskObj t in AstronautInstance.User.TasklistData.AllTasks)
            {
                if (t.status == 1)
                {
                    // the current task has already been set
                    current_task_set = true;
                    break;
                }
                else if (t.status == 0)
                {
                    // set the first encountered upcoming task as the current task
                    t.status = 1;
                    GameObject.Find("Controller").GetComponent<WebsocketDataHandler>().SendTasklistData();
                    EventBus.Publish(new TaskStartedEvent(t));
                    current_task_set = true;
                    break;
                }
            }
        }

        StartCoroutine(_SetCurrentTask());
    }

    void OnTaskFinished(TaskFinishedEvent e)
    {
        Debug.Log("setting task finished");
        Debug.Log(JsonUtility.ToJson(e.FinishedTask));
        e.FinishedTask.status = 2;
        SetCurrentTask<TaskFinishedEvent>(e);
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
        if (taskFinishedEvent != null)
        {
            EventBus.Unsubscribe(taskFinishedEvent);
        }
    }
}
