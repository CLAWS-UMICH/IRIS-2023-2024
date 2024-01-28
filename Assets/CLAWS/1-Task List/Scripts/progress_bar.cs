using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class progress_bar : MonoBehaviour
{
    private TextMeshPro fraction;
    private GameObject pbar;
    public float progress;
    public bool isSubtask = true;

    void Start()
    {
        fraction = transform.Find("Fraction").GetComponent<TextMeshPro>();
        pbar = transform.Find("pb_background").transform.Find("pb_bar").gameObject;
        pbar.transform.localScale = new Vector3(0, 1, 1);

        Init_Progress_bar();

        EventBus.Subscribe<TasklistBackendUpdated>(Init);
    }

    public void Update_Progress_bar(int completed, int total)
    {
        progress = ((float)completed) / total;
        fraction.text = completed + "/" + total;
        pbar.transform.localPosition = new Vector3((1 - progress) * (-0.5f), pbar.transform.localPosition.y, pbar.transform.localPosition.z);
        pbar.transform.localScale = new Vector3(progress, 1, 1);
    }

    public void Init(TasklistBackendUpdated e)
    {
        Init_Progress_bar();
    }

    public void Init_Progress_bar()
    {
        IEnumerator _Init()
        {
            if (isSubtask)
            {
                while (TaskListBackend.CurrentTask.subtasks == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                int count = 0;
                foreach (SubtaskObj s in TaskListBackend.CurrentTask.subtasks)
                {
                    if (s.status == 1)
                    {
                        count++;
                    }
                }
                Update_Progress_bar(count, TaskListBackend.CurrentTask.subtasks.Count);
            }
            else
            {
                int count = 0;
                foreach (TaskObj s in AstronautInstance.User.TasklistData.AllTasks)
                {
                    if (s.status == 2)
                    {
                        count++;
                    }
                }
                Update_Progress_bar(count, AstronautInstance.User.TasklistData.AllTasks.Count);
            }
        }

        
        StartCoroutine(_Init());
    }   
    
}
