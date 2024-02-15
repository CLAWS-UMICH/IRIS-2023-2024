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
            // TODO linear execution of subtasks
            Status.text = "In Progress";
            // EventBus.Publish<SubtaskStartedEvent>(new(Subtask));
            SubtaskIcon.SetActive(false);
        }
    }

    public void OnCheckButtonPressed()
    {
        if (Type == TaskType.Subtask)
        {
            ToggleSubtaskStatus();
        }
        else if (Type == TaskType.Emergency)
        {
            Task.status = 2;
            FinishTask();
        }
    }

    private void ToggleSubtaskStatus()
    {
        // this will increment the status (completed -> in progress or in progress -> complete)

        if (Subtask.status >= 1)
        {
            // completed -> in progress
            Subtask.status = 0;
            Status.text = "In Progress";
            SubtaskIcon.SetActive(false);
        }
        else
        {
            // in progress -> complete
            Subtask.status = 1;
            Status.text = "Completed";
            SubtaskIcon.SetActive(true);
            GameObject.Find("Prefab_MainTaskExpanded(Clone)").transform.Find("Task_Progress_Bar").GetComponent<progress_bar>().Update_Progress_bar(GetSubCompleted(), TaskListBackend.CurrentTask.subtasks.Count);
        }

        // propogate changes after delay
        IEnumerator _SendSubtaskStatus()
        {
            yield return new WaitForSeconds(1f);

            List<TaskObj> _editedTask = new();
            _editedTask.Add(TaskListBackend.CurrentTask);
            EventBus.Publish<TasksEditedEvent>(new(_editedTask));

            GameObject.Find("Controller").GetComponent<WebsocketDataHandler>().SendTasklistData();
        }

        StartCoroutine(_SendSubtaskStatus());
    }

    public void InitEmergencyTask(TaskObj task_f)
    {
        Task = task_f;
        Subtask = null;
        Type = TaskType.Emergency;

        Title.text = Task.title;
        Status.text = "Urgent!";
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

    public void FinishTask()
    {
        if (Type == TaskType.Main || Type == TaskType.Emergency)
        {
            GameObject.Find("Prefab_FullTaskListScreen").transform.Find("MainProgressBar").GetComponent<progress_bar>().Update_Progress_bar(GetMainCompleted(), AstronautInstance.User.TasklistData.AllTasks.Count);
            EventBus.Publish<TaskFinishedEvent>(new TaskFinishedEvent(Task));
        }
        else
        {
            EventBus.Publish<SubtaskFinishedEvent>(new SubtaskFinishedEvent(Subtask));
        }

        GameObject.Find("Controller").GetComponent<WebsocketDataHandler>().SendTasklistData();
        GameObject.Find("Prefab_FullTaskListScreen").GetComponent<TaskListBackend>().SetCurrentTask<bool>(true);
    }

    private int GetMainCompleted()
    {
        int c = 0;
        for (int i = 0; i < AstronautInstance.User.TasklistData.AllTasks.Count; i++)
        {
            if (AstronautInstance.User.TasklistData.AllTasks[i].status == 1)
            {
                c++;
            }
        }

        return c;
    }

    private int GetSubCompleted()
    {
        int c = 0;
        foreach (SubtaskObj i in TaskListBackend.CurrentTask.subtasks)
        {
            if (i.status == 1)
            {
                c++;
            }
        }
        return c;
    }
}
