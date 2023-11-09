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
    [SerializeField] TaskObj TaskToDelete;

    [ContextMenu("AddTask")]
    private void AddTask()
    {
        List<TaskObj> TasksToAdd = new()
        {
            TaskToAdd
        };

        Debug.Log(string.Format("Adding fake task: {0}", JsonUtility.ToJson(TaskToAdd)));
        EventBus.Publish<TasksAddedEvent>(new TasksAddedEvent(TasksToAdd));
    }

    [ContextMenu("EditTask")]
    private void EditTask()
    {
        List<TaskObj> TasksToEdit = new()
        {
            TaskToEdit
        };

        Debug.Log(string.Format("Editing fake task: {0}", JsonUtility.ToJson(TaskToEdit)));
        EventBus.Publish<TasksEditedEvent>(new TasksEditedEvent(TasksToEdit));
    }

    [ContextMenu("DeleteTask")]
    private void DeleteTask()
    {
        List<TaskObj> TasksToDelete = new()
        {
            TaskToEdit
        };

        Debug.Log(string.Format("Adding fake task: {0}", JsonUtility.ToJson(TaskToDelete)));
        EventBus.Publish<TasksDeletedEvent>(new TasksDeletedEvent(TasksToDelete));
    }
}
