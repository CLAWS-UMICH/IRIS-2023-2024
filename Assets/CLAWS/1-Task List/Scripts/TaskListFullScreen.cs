using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*  Phase 1 - Task List Team 
 *  Patrick, Alex, John
 */

public class TaskListFullScreen : MonoBehaviour
{
    List<GameObject> TaskList_List;
    List<GameObject> IconsForLines_List;
    List<GameObject> Lines_List;

    [SerializeField] GameObject TaskPrefab;
    [SerializeField] GameObject TaskExpandedPrefab;
    [SerializeField] GameObject SubtaskPrefab;
    [SerializeField] GameObject LinePrefab;

    [SerializeField] ScrollHandler TaskList_ScrollHandler;
    [SerializeField] ClippingManager Clipping;
    [SerializeField] GameObject LinesParent;

    [ContextMenu("func RenderTaskList")]
    public void RenderTaskList()
    {
        ClearList(TaskList_List);
        ClearList(Lines_List);

        IconsForLines_List.Clear();
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

            DrawLines();
            Clipping.SetRenderers();
        }

        StartCoroutine(_Render());
    }

    void DrawLines()
    {
        for (int i = 1; i < IconsForLines_List.Count; i++)
        {
            float y_start = IconsForLines_List[i - 1].transform.position.y
                            - (IconsForLines_List[i - 1].GetComponent<BoxCollider2D>().size.y / 2
                            * IconsForLines_List[i - 1].transform.localScale.y) - 0.003f;
            float y_end = IconsForLines_List[i].transform.position.y
                            + (IconsForLines_List[i].GetComponent<BoxCollider2D>().size.y / 2
                            * IconsForLines_List[i].transform.localScale.y) + 0.003f;

            MakeLine(
                new Vector3(IconsForLines_List[i - 1].transform.position.x, y_start, IconsForLines_List[i - 1].transform.position.z),
                new Vector3(IconsForLines_List[i].transform.position.x, y_end, IconsForLines_List[i].transform.position.z)
            );
        }
    }

    void MakeLine(Vector3 start_f, Vector3 end_f)
    {
        Vector3 midpoint = (start_f + end_f) / 2;
        float scale = start_f.y - end_f.y;

        GameObject line = Instantiate(LinePrefab, LinesParent.transform);
        line.transform.position = midpoint;
        line.transform.localScale = new Vector3(line.transform.localScale.x, scale, line.transform.localScale.z);

        Lines_List.Add(line);
        Clipping.objectsToClip.Add(line);
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
            IconsForLines_List.Add(task_instance.GetIcon());
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
            IconsForLines_List.Add(task_instance.GetIcon());
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

    void ClearList(List<GameObject> ToDelete)
    {
        foreach (GameObject g in ToDelete)
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
        ToDelete.Clear();
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
        IconsForLines_List = new();
        Lines_List = new();

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
