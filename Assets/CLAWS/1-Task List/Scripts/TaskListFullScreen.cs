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
    [SerializeField] GameObject SubtaskPrefab;
    [SerializeField] ScrollHandler TaskList_ScrollHandler;
    [SerializeField] LinesBetweenObjects LineDrawer;
    [SerializeField] ClippingManager Clipping;

    [ContextMenu("func RenderTaskList")]
    public void RenderTaskList()
    {
        ClearTaskList();
        Clipping.objectsToClip.Clear();

        IEnumerator _Render()
        {
            yield return new WaitForSeconds(0.1f);
            foreach (TaskObj taskobj in AstronautInstance.User.TasklistData.AllTasks)
            {
                AddTask(taskobj);
            }
            TaskList_ScrollHandler.Fix();
            Clipping.SetRenderers();
        }

        StartCoroutine(_Render());
    }

    /* Add a task to the tasklist display
     * Note: This only adds to the tasklist display, not the actual backend */
    void AddTask(TaskObj taskobj_f)
    {
        GameObject g = Instantiate(TaskPrefab, TaskList_ScrollHandler.transform);
        TaskInstance task_instance = g.GetComponent<TaskInstance>();
        task_instance.InitTask(taskobj_f);
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
        TaskList_List.Clear();
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
