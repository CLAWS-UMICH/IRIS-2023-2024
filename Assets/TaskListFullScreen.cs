using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*  Phase 1 - Task List Team 
 *  Patrick, Alex, John
 */

public class TaskListFullScreen : MonoBehaviour
{
    public TextMeshPro tmp;
    private Subscription<TasksDeletedEvent> tasksDeletedEvent;
    private Subscription<TasksEditedEvent> tasksEditedEvent;
    private Subscription<TasksAddedEvent> tasksAddedEvent;

    void Start()
    {
        tasksDeletedEvent = EventBus.Subscribe<TasksDeletedEvent>(OnTaskDeleted);
        tasksEditedEvent = EventBus.Subscribe<TasksEditedEvent>(Temp<TasksEditedEvent>);
        tasksAddedEvent = EventBus.Subscribe<TasksAddedEvent>(Temp<TasksAddedEvent>);
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
    }

    void OnTaskDeleted(TasksDeletedEvent e)
    {
        // TODO popup message
        tmp.text = JsonUtility.ToJson(e);
    }
    
    void Temp<T>(T e)
    {
        string json = JsonUtility.ToJson(e);
        Debug.Log(json);
    }

}
