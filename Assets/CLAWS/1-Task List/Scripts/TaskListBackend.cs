using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListBackend : MonoBehaviour
{
    public static TaskObj CurrentTask;
    public static SubtaskObj CurrentSubtask;
    public static TaskObj CurrentEmergencyTask; // null if none


    private Subscription<TaskFinishedEvent> taskFinishedEvent;
    private Subscription<TasksDeletedEvent> tasksDeletedEvent;
    private Subscription<TasksEditedEvent> tasksEditedEvent;
    private Subscription<TasksAddedEvent> tasksAddedEvent;

    void Start()
    {
        taskFinishedEvent = EventBus.Subscribe<TaskFinishedEvent>(SetCurrentTask);
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

            // get current task
            foreach (TaskObj t in AstronautInstance.User.TasklistData.AllTasks)
            {
                if (t.status == 1)
                {
                    if (IsComplete(t))
                    {
                        // set task as finished
                        t.status = 2;
                        GameObject.Find("Controller").GetComponent<WebsocketDataHandler>().SendTasklistData();
                        continue;
                    }
                    else
                    {
                        // this is still the current task
                        current_task_set = true;
                        CurrentTask = t;
                        break;
                    }
                }
                else if (t.status == 0)
                {
                    // set the first encountered upcoming task as the current task
                    t.status = 1;
                    current_task_set = true;
                    EventBus.Publish(new TaskStartedEvent(t));
                    GameObject.Find("Controller").GetComponent<WebsocketDataHandler>().SendTasklistData();
                    CurrentTask = t;
                    break;
                }
            }

            // get current emergency task
            CurrentEmergencyTask = null;
            foreach (TaskObj t in AstronautInstance.User.TasklistData.AllTasks)
            {
                if (t.isEmergency && t.status != 2)
                {
                    CurrentEmergencyTask = t;
                    EventBus.Publish(new EmergencyTaskEvent(t));
                    break;
                }
            }

            // get current subtask
            foreach (SubtaskObj t in CurrentTask.subtasks)
            {
                if (t.status == 0)
                {
                    // first in-progress subtask -> current task
                    CurrentSubtask = t;
                    EventBus.Publish(new SubtaskStartedEvent(t));
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

    bool IsComplete(TaskObj task_f)
    {
        foreach (SubtaskObj s in task_f.subtasks)
        {
            if (s.status == 0)
            {
                return false;
            }
        }
        return true;
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
