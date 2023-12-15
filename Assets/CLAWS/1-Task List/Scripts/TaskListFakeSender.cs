using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * To send fake data to the task list
 */
public class TaskListFakeSender : MonoBehaviour
{
    [SerializeField] TaskObj TaskToAdd;
    [SerializeField] TaskObj TaskToEdit;
    [SerializeField] int TaskToDelete;
    [SerializeField] int TaskToFinish;

    [ContextMenu("Func AddTask")]
    private void AddTask()
    {
        AstronautInstance.User.TasklistData.AllTasks.Add(new TaskObj(TaskToAdd));

        List<TaskObj> TasksToAdd = new()
        {
            TaskToAdd
        };

        Debug.Log(string.Format("Adding fake task: {0}", JsonUtility.ToJson(TaskToAdd)));
        EventBus.Publish<TasksAddedEvent>(new TasksAddedEvent(TasksToAdd));
    }

    [ContextMenu("Func EditTask")]
    private void EditTask()
    {
        for(int i = 0; i < AstronautInstance.User.TasklistData.AllTasks.Count; ++i)
        {
            if(AstronautInstance.User.TasklistData.AllTasks[i].task_id == TaskToEdit.task_id)
            {
                AstronautInstance.User.TasklistData.AllTasks[i] = new TaskObj(TaskToEdit);
            }
        }

        List<TaskObj> TasksToEdit = new()
        {
            TaskToEdit
        };


        Debug.Log(string.Format("Editing fake task: {0}", JsonUtility.ToJson(TaskToEdit)));
        EventBus.Publish<TasksEditedEvent>(new TasksEditedEvent(TasksToEdit));
    }

    [ContextMenu("Func DeleteTask")]
    private void DeleteTask()
    {
        TaskObj _TaskToDelete = AstronautInstance.User.TasklistData.AllTasks.Find((taskobj) => taskobj.task_id == TaskToDelete);
        AstronautInstance.User.TasklistData.AllTasks.Remove(_TaskToDelete);

        List<TaskObj> TasksToDelete = new()
        {
            _TaskToDelete
        };

        Debug.Log(string.Format("Deleting fake task: {0}", JsonUtility.ToJson(_TaskToDelete)));
        EventBus.Publish<TasksDeletedEvent>(new TasksDeletedEvent(TasksToDelete));
    }

    [ContextMenu("Func FinishTask")]
    private void FinishTask()
    {
        TaskObj _TaskToFinish = AstronautInstance.User.TasklistData.AllTasks.Find((taskobj) => taskobj.task_id == TaskToFinish);
        EventBus.Publish<TaskFinishedEvent>(new TaskFinishedEvent(_TaskToFinish));
    }
}
