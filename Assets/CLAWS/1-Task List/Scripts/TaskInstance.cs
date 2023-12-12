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
    [SerializeField] TaskListIcon Icon;

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
            Icon.SetIcon(TaskListIconEnum.UpcomingTask);
        }
    }

    private void UpdateIcon(TaskStartedEvent e)
    {
        if (e.StartedTask == Task)
        {
            Icon.SetIcon(TaskListIconEnum.CurrentTask);
        }
    }
    private void UpdateIcon(TaskFinishedEvent e)
    {
        if (e.FinishedTask == Task)
        {
            Icon.SetIcon(TaskListIconEnum.CurrentTask);
        }
    }

    private Subscription<TaskStartedEvent> taskStartedEvent;
    private Subscription<TaskFinishedEvent> taskFinishedEvent;

    private void Start()
    {
        taskStartedEvent = EventBus.Subscribe<TaskStartedEvent>(UpdateIcon);
        taskFinishedEvent = EventBus.Subscribe<TaskFinishedEvent>(UpdateIcon);
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