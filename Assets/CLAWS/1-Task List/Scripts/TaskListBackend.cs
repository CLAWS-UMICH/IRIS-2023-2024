using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListBackend : MonoBehaviour
{

    private Subscription<TaskFinishedEvent> taskFinishedEvent;
    private List<TaskObj> currentTasks = AstronautInstance.User.TasklistData.AllTasks;
    // Start is called before the first frame update
    void Start()
    {
        taskFinishedEvent = EventBus.Subscribe<TaskFinishedEvent>(OnTaskFinished);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTaskFinished(TaskFinishedEvent e)
    {
        e.FinishedTask.status = 3;
        for (int i = 0; i < currentTasks.Count; ++i)
        {
            if (i != currentTasks.Count && currentTasks[i].status == 3)
            {
                currentTasks[i + 1].status = 2;
                GameObject.Find("Controller").GetComponent<WebsocketDataHandler>().SendTasklistData();
                EventBus.Publish(new TaskStartedEvent(currentTasks[i + 1]));
                break;
            }
        }
    }
}
