using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*  Phase 1 - Task List Team 
 *  Patrick, Alex, John
 */

public class TaskListFullScreen : MonoBehaviour
{
    [SerializeField] List<GameObject> TaskList_List;
    [SerializeField] GameObject TaskPrefab;
    [SerializeField] ScrollHandler TaskList_ScrollHandler;

    [ContextMenu("func RenderTaskList")]
    void RenderTaskList()
    {
        ClearTaskList();

        foreach (TaskObj taskobj in AstronautInstance.User.TasklistData.AllTasks)
        {
            AddTask(taskobj);
        }
    }

    /* Add a task to the tasklist display
     * Note: This only adds to the tasklist display, not the actual backend */
    void AddTask(TaskObj taskobj_f)
    {
        GameObject g = Instantiate(TaskPrefab, TaskList_ScrollHandler.transform);
        // TODO update g with the correct stuff
        TaskList_ScrollHandler.Fix();
        TaskList_List.Add(g);
    }

    [ContextMenu("func AddTask")]
    void Debug_AddTask()
    {
        TaskObj t = new();
        t.title = "This is a debug task";
        t.isEmergency = false;
        AddTask(t);
    }

    /* Note: This only clears the tasklist display, not the actual list in the backend */
    void ClearTaskList()
    {
        foreach (GameObject g in TaskList_List)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(g);
            }
            else
#endif
            {
                Destroy(g);
            }
        }
    }


    // Event Subscriptions

    private Subscription<TasksDeletedEvent> tasksDeletedEvent;
    private Subscription<TasksEditedEvent> tasksEditedEvent;
    private Subscription<TasksAddedEvent> tasksAddedEvent;

    void Start()
    {
        TaskList_List = new();
        tasksDeletedEvent = EventBus.Subscribe<TasksDeletedEvent>(OnTaskDeleted);
        tasksEditedEvent = EventBus.Subscribe<TasksEditedEvent>(OnTaskEdited);
        tasksAddedEvent = EventBus.Subscribe<TasksAddedEvent>(OnTaskAdded);
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

    void OnTaskDeleted(TasksDeletedEvent e)
    {
        RenderTaskList();
    }

    void OnTaskEdited(TasksEditedEvent e)
    {
        RenderTaskList();
    }

    void OnTaskAdded(TasksAddedEvent e)
    {
        RenderTaskList();
    }

}
