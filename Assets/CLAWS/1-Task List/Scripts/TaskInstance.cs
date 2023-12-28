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
    [SerializeField] GameObject DetailedViewPrefab;
    

    public void InitTask(TaskObj task_f, bool is_current_task_f)
    {
        Task = task_f;
        Subtask = null;
        Type = TaskType.Main;
        Title.text = Task.title;

        // Task Icon
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
        GameObject DetailedView = Instantiate(DetailedViewPrefab, transform);
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
