using UnityEngine;
using System.Collections.Generic;

public class UpdateCurrentTask : MonoBehaviour
{
    private Subscription<SubtaskStartedEvent> taskStartedEvent;

    [SerializeField] TaskInstance Task;
    [SerializeField] List<GameObject> EmergencyBackplates;


    void Start()
    {
        taskStartedEvent = EventBus.Subscribe<SubtaskStartedEvent>(OnUpdateCurrentTask);
    }

    private void OnUpdateCurrentTask(SubtaskStartedEvent e)
    {
        Debug.Log("ASdfssafa");
        Debug.Log(TaskListBackend.CurrentEmergencyTask != null);
        if (TaskListBackend.CurrentEmergencyTask != null)
        {
            // emergency task
            Task.InitEmergencyTask(TaskListBackend.CurrentEmergencyTask);
            SetActiveEmergencyBackplates(true);
        }
        else
        {
            // subtask
            Task.InitTask(e.StartedTask);
            SetActiveEmergencyBackplates(false);
        }
    }

    private void SetActiveEmergencyBackplates(bool active)
    {
        foreach (GameObject g in EmergencyBackplates)
        {
            g.SetActive(active);
        }
    }

    void OnDestroy()
    {
        if (taskStartedEvent != null)
        {
            EventBus.Unsubscribe(taskStartedEvent);
        }

    }
}
