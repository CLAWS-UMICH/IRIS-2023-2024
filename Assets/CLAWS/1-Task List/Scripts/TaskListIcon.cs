using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskListIconEnum
{
    CompletedTask,
    CurrentTask,
    UpcomingTask,
    CompletedSubtask,
    CurrentSubtask,
    UpcomingSubtask
}

public class TaskListIcon : MonoBehaviour
{
    [SerializeField] Material CompletedTask_Material;
    [SerializeField] Material CurrentTask_Material;
    [SerializeField] Material UpcomingTask_Material;
    [SerializeField] Material CompletedSubtask_Material;
    [SerializeField] Material CurrentSubtask_Material;
    [SerializeField] Material UpcomingSubtask_Material;

    public void SetIcon(TaskListIconEnum f_icon)
    {
        switch (f_icon)
        {
            case TaskListIconEnum.CompletedSubtask:
                GetComponent<MeshRenderer>().material = CompletedSubtask_Material;
                break;
            case TaskListIconEnum.CompletedTask:
                transform.localScale = Vector3.one * 0.2f;
                GetComponent<MeshRenderer>().material = CompletedTask_Material;
                break;
            case TaskListIconEnum.CurrentSubtask:
                GetComponent<MeshRenderer>().material = CurrentSubtask_Material;
                break;
            case TaskListIconEnum.CurrentTask:
                transform.localScale = Vector3.one * 0.3f;
                GetComponent<MeshRenderer>().material = CurrentTask_Material;
                break;
            case TaskListIconEnum.UpcomingSubtask:
                GetComponent<MeshRenderer>().material = UpcomingSubtask_Material;
                break;
            case TaskListIconEnum.UpcomingTask:
                transform.localScale = Vector3.one * 0.2f;
                GetComponent<MeshRenderer>().material = UpcomingTask_Material;
                break;
            default:
                Debug.LogError("[Task List Error] Icon messed up");
                break;
        }
    }
}
