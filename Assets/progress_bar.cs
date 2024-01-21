using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class progress_bar : MonoBehaviour
{
    private TextMeshPro fraction;
    private GameObject pbar;
    public int totalS = 0;
    public int completedS = 0;
    private List<TaskObj> task_list = AstronautInstance.User.TasklistData.AllTasks;
    private Subscription<TaskFinishedEvent> task_finished_event;
    // Start is called before the first frame update
    void Start()
    {
        task_finished_event = EventBus.Subscribe<TaskFinishedEvent>(Progress);
        totalS = task_list.Count;
        fraction = transform.Find("Fraction").GetComponent<TextMeshPro>();
        pbar = transform.Find("pb_bar").GetComponent<GameObject>();
        fraction.text = completedS + "/" + totalS;
    }

    private void Progress(TaskFinishedEvent e)
    {
        int c = 0;
        for (int i = 0; i < task_list.Count; i++)
        {
            if (task_list[i].status == 1)
            {
                c++;
            }
        }
        float progress = (float)c / task_list.Count;
        totalS = task_list.Count;
        completedS = c;
        fraction.text = completedS + "/" + totalS;
        pbar.transform.localPosition = new Vector3((1-progress)*(-0.5f), pbar.transform.localPosition.y, pbar.transform.localPosition.z);
        pbar.transform.localScale = new Vector3(progress, 1, 1);
    }

    
    
}
