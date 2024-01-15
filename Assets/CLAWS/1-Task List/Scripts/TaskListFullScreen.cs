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
    [SerializeField] GameObject EmergencyTaskPrefab;
    [SerializeField] GameObject LinePrefab;

    [SerializeField] TaskListScrollHandler TaskList_ScrollHandler;
    [SerializeField] ClippingManager Clipping;
    [SerializeField] GameObject LinesParent;

    [SerializeField] GameObject DetailedView;

    Queue<string> FunctionQueue;

    [ContextMenu("func RenderTaskList")]
    public void RenderTaskList()
    {
        FunctionQueue.Enqueue("ClearAll");
        FunctionQueue.Enqueue("Render");
    }

    void ClearAll()
    {
        ClearList(TaskList_List);
        ClearList(Lines_List);

        IconsForLines_List.Clear();
        Clipping.objectsToClip.Clear();
    }

    void Render()
    {
        // Render emergency tasks first
        foreach (TaskObj taskobj in AstronautInstance.User.TasklistData.AllTasks)
        {
            if (taskobj.isEmergency)
            {
                AddEmergencyTask(taskobj);
            }
        }

        // Render non-emergency tasks
        foreach (TaskObj taskobj in AstronautInstance.User.TasklistData.AllTasks)
        {
            if (!taskobj.isEmergency)
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
        }
        TaskList_ScrollHandler.FixLocations();

        DrawLines();
        Clipping.SetRenderers();
    }

    private void Update()
    {
        if (FunctionQueue.Count > 0)
        {
            string operation = FunctionQueue.Dequeue();

            switch (operation)
            {
                case "ClearAll":
                    ClearAll();
                    break;
                case "Render":
                    Render();
                    break;
                default:
                    Debug.LogError("Nothing in function queue");
                    break;
            }
        }
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

    void AddEmergencyTask(TaskObj taskobj_f)
    {
        GameObject g = Instantiate(EmergencyTaskPrefab, TaskList_ScrollHandler.transform);
        TaskInstance task_instance = g.GetComponent<TaskInstance>();
        task_instance.InitEmergencyTask(taskobj_f);
        task_instance.DetailedView = DetailedView;
        TaskList_List.Add(g);
        Clipping.objectsToClip.Add(g);
    }

    void AddTask(TaskObj taskobj_f, bool is_current_task)
    {
        if (is_current_task)
        {
            GameObject g = Instantiate(TaskExpandedPrefab, TaskList_ScrollHandler.transform);
            TaskInstance task_instance = g.GetComponent<TaskInstance>();
            task_instance.InitTask(taskobj_f, true);
            task_instance.DetailedView = DetailedView;
            IconsForLines_List.Add(task_instance.GetIcon());
            TaskList_List.Add(g);
            Clipping.objectsToClip.Add(g);

            // add subtasks
            foreach (SubtaskObj subtaskobj in taskobj_f.subtasks)
            {
                GameObject s = Instantiate(SubtaskPrefab, TaskList_ScrollHandler.transform);
                TaskInstance i = s.GetComponent<TaskInstance>();
                i.InitTask(subtaskobj);
                i.DetailedView = DetailedView;
                TaskList_List.Add(s);
                Clipping.objectsToClip.Add(s);
            }
        }
        else
        {
            GameObject g = Instantiate(TaskPrefab, TaskList_ScrollHandler.transform);
            TaskInstance task_instance = g.GetComponent<TaskInstance>();
            task_instance.InitTask(taskobj_f, false);
            task_instance.DetailedView = DetailedView;
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
        FunctionQueue = new();

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
