using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*  Phase 1 - Task List Team 
 *  Patrick, Alex, John
 */

public class TaskListFullScreen : MonoBehaviour
{
    private Subscription<TasksDeletedEvent> tasksDeletedEvent;
    private Subscription<TasksEditedEvent> tasksEditedEvent;
    private Subscription<TasksAddedEvent> tasksAddedEvent;

    void Start()
    {
        tasksDeletedEvent = EventBus.Subscribe<TasksDeletedEvent>(OnTaskDeleted);
        tasksEditedEvent = EventBus.Subscribe<TasksEditedEvent>(OnTaskEdited);
        tasksAddedEvent = EventBus.Subscribe<TasksAddedEvent>(OnTaskAdded);
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

    }

    void OnTaskEdited(TasksEditedEvent e)
    {
        
    }

    void OnTaskAdded(TasksAddedEvent e)
    {
        
    }

}
