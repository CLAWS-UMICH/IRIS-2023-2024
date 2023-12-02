using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInstance : MonoBehaviour
{
    private TaskObj Task;
    [SerializeField] GameObject DetailedViewPrefab;

    void Start()
    {
        
    }

    public void ViewDetails()
    {
        GameObject DetailedView = Instantiate(DetailedViewPrefab, transform);
        DetailedTask DetailedTaskView = DetailedView.GetComponent<DetailedTask>();
        DetailedTaskView.InitDetailedView(Task.title, Task.description, "boop");
    }
}
