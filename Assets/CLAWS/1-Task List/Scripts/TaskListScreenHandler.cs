using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListScreenHandler : MonoBehaviour
{
    GameObject FullTaskListScreen;
    GameObject CurrentTaskScreen;
    GameObject DetailedTaskScreen;

    // Start is called before the first frame update
    void Start()
    {
        FullTaskListScreen = transform.Find("Prefab_FullTaskListScreen").gameObject;
        CurrentTaskScreen = GameObject.Find("Prefab_CurrentTaskScreen").gameObject;
        DetailedTaskScreen = transform.Find("Detailed Task View").gameObject;

        FullTaskListScreen.SetActive(false);
        CurrentTaskScreen.SetActive(true);
        DetailedTaskScreen.SetActive(false);
    }

    public void OpenTaskListMain()
    {
        FullTaskListScreen.SetActive(true);
        CurrentTaskScreen.SetActive(true);
        DetailedTaskScreen.SetActive(false);
        EventBus.Publish(new ScreenChangedEvent(Screens.Tasklist));
    }

    public void CloseTaskListMain()
    {
        FullTaskListScreen.SetActive(false);
        CurrentTaskScreen.SetActive(true);
        DetailedTaskScreen.SetActive(false);
        EventBus.Publish(new ScreenChangedEvent(Screens.Menu));
    }
}
