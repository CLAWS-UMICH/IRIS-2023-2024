using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TaskType
{
    Subtask, 
    Main, 
    Emergency
}

public class TaskInstance : MonoBehaviour
{
    [SerializeField] TaskObj Task;
    [SerializeField] SubtaskObj Subtask;
    [SerializeField] TaskType Type;
    [SerializeField] TaskListIcon Icon;

    [SerializeField] TextMeshPro Title;
    [SerializeField] TextMeshPro Status;

    [SerializeField] GameObject SubtaskIcon; // this is a checkmark

    public GameObject DetailedView;

    public void InitTask(TaskObj task_f, bool is_current_task_f)
    {
        Task = task_f;
        Subtask = null;
        Type = TaskType.Main;

        Title.text = Task.title;

        // icon
        if (is_current_task_f)
        {
            Icon.SetIcon(TaskListIconEnum.CurrentTask);
        }
        else
        {
            if (Task.status == 0)
            {
                Icon.SetIcon(TaskListIconEnum.UpcomingTask);
                Status.gameObject.SetActive(false);
            }
            else
            {
                Icon.SetIcon(TaskListIconEnum.CompletedTask);
                Status.gameObject.SetActive(true);
                Status.text = "Completed";
            }
        }
        
    }

    public void InitTask(SubtaskObj subtask_f)
    {
        Task = null;
        Subtask = subtask_f;
        Type = TaskType.Subtask;

        Title.text = Subtask.title;
        if (Subtask.status >= 1)
        {
            Status.text = "Completed";
            SubtaskIcon.SetActive(true);
        }
        else
        {
            // mark all subtasks as in progress
            Status.text = "In Progress";
            EventBus.Publish<SubtaskStartedEvent>(new(Subtask));
            SubtaskIcon.SetActive(false);
        }
    }

    [ContextMenu("func ToggleSubtaskStatus")]
    public void ToggleSubtaskStatus()
    {
        // this will increment the status (completed -> in progress or in progress -> complete)

        if (Subtask.status >= 1)
        {
            // completed -> in progress
            Subtask.status = 0;
            Status.text = "In Progress";
            EventBus.Publish<SubtaskStartedEvent>(new(Subtask));
            SubtaskIcon.SetActive(false);
        }
        else
        {
            // in progress -> complete
            Subtask.status = 1;
            Status.text = "Completed";
            EventBus.Publish<SubtaskFinishedEvent>(new(Subtask));
            SubtaskIcon.SetActive(true);
        }

        GameObject.Find("Controller").GetComponent<WebsocketDataHandler>().SendTasklistData();
    }

    public void InitEmergencyTask(TaskObj task_f)
    {
        Task = task_f;
        Subtask = null;
        Type = TaskType.Emergency;

        Title.text = Task.title;
        Status.text = Task.description;
    }

    public GameObject GetIcon()
    {
        return Icon.gameObject;
    }

    public void ViewDetails()
    {
        DetailedView.SetActive(true);
        DetailedTask DetailedTaskView = DetailedView.GetComponent<DetailedTask>();

        if (Type == TaskType.Main || Type == TaskType.Emergency)
        {
            DetailedTaskView.InitDetailedView(Task.title, Task.description, "boop");
        }
        else
        {
            DetailedTaskView.InitDetailedView(Subtask.title, Subtask.description, "boop");
        }
    }

    [ContextMenu("func FinishTask")]
    public void FinishTask()
    {
        if (Type == TaskType.Main || Type == TaskType.Emergency)
        {
            EventBus.Publish<TaskFinishedEvent>(new TaskFinishedEvent(Task));
        }
        else
        {
            EventBus.Publish<SubtaskFinishedEvent>(new SubtaskFinishedEvent(Subtask));
        }
    }
}
