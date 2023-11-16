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
        // TODO popup message
        tmp.text = JsonUtility.ToJson(AstronautInstance.User.TasklistData);
    }

    void OnTaskEdited(TasksEditedEvent e)
    {
        int emergency_count = 0;
        foreach (TaskObj task in e.EditedTasks)
        {
            if(task.isEmergency)
            {
                emergency_count += 1;
            }
        }
        Debug.Log("This many emergency tasks were added: " + emergency_count);
        // TODO popup message
        tmp.text = JsonUtility.ToJson(AstronautInstance.User.TasklistData);
    }

    void OnTaskAdded(TasksAddedEvent e)
    {
        int emergency_count = 0;
        foreach (TaskObj task in e.NewAddedTasks)
        {
            if (task.isEmergency)
            {
                emergency_count += 1;
            }
        }
        Debug.Log("This many new emergency tasks were added: " + emergency_count);
        // TODO popup message
        tmp.text = JsonUtility.ToJson(AstronautInstance.User.TasklistData);
    }

}
