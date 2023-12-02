using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    Subtask, 
    Main
}

public class TaskInstance : MonoBehaviour
{
    [SerializeField] TaskObj Task;
    [SerializeField] SubtaskObj Subtask;
    [SerializeField] TaskType Type;

    [SerializeField] GameObject IconPrefab;
    [SerializeField] GameObject DetailedViewPrefab;

    public void InitTask(TaskObj task_f)
    {
        Task = task_f;
        Subtask = null;
        Type = TaskType.Main;
    }

    public void InitTask(SubtaskObj subtask_f)
    {
        Task = null;
        Subtask = subtask_f;
        Type = TaskType.Subtask;
    }

    // TODO instantiate info based on task or subtask
    public void ViewDetails()
    {
        GameObject DetailedView = Instantiate(DetailedViewPrefab, transform);
        DetailedTask DetailedTaskView = DetailedView.GetComponent<DetailedTask>();
        DetailedTaskView.InitDetailedView(Task.title, Task.description, "boop");
    }

    private void CreateIcon()
    {
        if (Type == TaskType.Main)
        {
            // TODO create icons
        }
        else // Subtask
        {
            // TODO create icons ifff
            // actually I think subtask icons will be housed within the subtask prefab
            // pressing it will publish an event to mark as complete from this script
        }
    }

    private void UpdateIcon(TaskStartedEvent e)
    {
        if (e.StartedTask == Task)
        {
            // TODO update icon (just do it for every task under all circumstances)
        }
    }


    private Subscription<TaskStartedEvent> taskStartedEvent;

    private void Start()
    {
        taskStartedEvent = EventBus.Subscribe<TaskStartedEvent>(UpdateIcon);
    }
    void OnDestroy()
    {
        // Unsubscribe when the script is destroyed
        if (taskStartedEvent != null)
        {
            EventBus.Unsubscribe(taskStartedEvent);
        }
    }
}