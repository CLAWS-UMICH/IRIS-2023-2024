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
    [SerializeField] GameObject TaskExpandedPrefab;
    [SerializeField] GameObject SubtaskPrefab;
    [SerializeField] ScrollHandler TaskList_ScrollHandler;
    [SerializeField] LinesBetweenObjects LineDrawer;
    [SerializeField] ClippingManager Clipping;

    [ContextMenu("func RenderTaskList")]
    public void RenderTaskList()
    {
        ClearTaskList();
        Clipping.objectsToClip.Clear();

        // render
        IEnumerator _Render()
        {
            yield return new WaitForSeconds(0);
            foreach (TaskObj taskobj in AstronautInstance.User.TasklistData.AllTasks)
            {
                if (taskobj.status == 1)
                {
                    AddTask(taskobj, true);
                }
                else
                {
                    AddTask(taskobj, false);
                }
            }
            TaskList_ScrollHandler.Fix();
            Clipping.SetRenderers();
        }

        StartCoroutine(_Render());
    }

    /* Add a task to the tasklist display
     * Note: This only adds to the tasklist display, not the actual backend */
    void AddTask(TaskObj taskobj_f, bool is_current_task)
    {
        if (is_current_task)
        {
            GameObject g = Instantiate(TaskExpandedPrefab, TaskList_ScrollHandler.transform);
            TaskInstance task_instance = g.GetComponent<TaskInstance>();
            task_instance.InitTask(taskobj_f, true);
            LineDrawer.Objects.Add(task_instance.GetIcon());
            TaskList_List.Add(g);
            Clipping.objectsToClip.Add(g);

            // add subtasks
            foreach (SubtaskObj subtaskobj in taskobj_f.subtasks)
            {
                GameObject s = Instantiate(SubtaskPrefab, TaskList_ScrollHandler.transform);
                g.GetComponent<TaskInstance>().InitTask(subtaskobj);
                TaskList_List.Add(s);
                Clipping.objectsToClip.Add(s);
            }
        }
        else
        {
            GameObject g = Instantiate(TaskPrefab, TaskList_ScrollHandler.transform);
            TaskInstance task_instance = g.GetComponent<TaskInstance>();
            task_instance.InitTask(taskobj_f, false);
            LineDrawer.Objects.Add(task_instance.GetIcon());
            TaskList_List.Add(g);
            Clipping.objectsToClip.Add(g);
        }
        
    }

    [ContextMenu("func AddTask")]
    void Debug_AddTask()
    {
        TaskObj t = new();
        t.title = "This is a debug task";
        t.isEmergency = false;
        AddTask(t, false);
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
        TaskList_List.Clear();
    }


    // Event Subscriptions

    private Subscription<TasksDeletedEvent> tasksDeletedEvent;
    private Subscription<TasksEditedEvent> tasksEditedEvent;
    private Subscription<TasksAddedEvent> tasksAddedEvent;
    private Subscription<TaskStartedEvent> taskStartedEvent;
    private Subscription<TaskFinishedEvent> taskFinishedEvent;


    void Start()
    {
        TaskList_List = new();
        tasksDeletedEvent = EventBus.Subscribe<TasksDeletedEvent>(OnEvent_RenderTaskList);
        tasksEditedEvent = EventBus.Subscribe<TasksEditedEvent>(OnEvent_RenderTaskList);
        tasksAddedEvent = EventBus.Subscribe<TasksAddedEvent>(OnEvent_RenderTaskList);
        taskStartedEvent = EventBus.Subscribe<TaskStartedEvent>(OnEvent_RenderTaskList);
        taskFinishedEvent = EventBus.Subscribe<TaskFinishedEvent>(OnEvent_RenderTaskList);

        RenderTaskList();
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
        if (taskStartedEvent != null)
        {
            EventBus.Unsubscribe(taskStartedEvent);
        }
        if (taskFinishedEvent != null)
        {
            EventBus.Unsubscribe(taskFinishedEvent);
        }
    }

    void OnEvent_RenderTaskList<T>(T e)
    {
        RenderTaskList();
    }


}
