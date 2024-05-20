using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAProgressBar : MonoBehaviour
{
    public GameObject pbar;
    public float progress;
    public bool isSubtask = true;

    void Start()
    {
        pbar.transform.localScale = new Vector3(0, 1, 1);
    }

    public void Update_Progress_bar(int completed, int total)
    {
        progress = ((float)completed) / total;
        pbar.transform.localPosition = new Vector3((1 - progress) * (-0.5f), pbar.transform.localPosition.y, pbar.transform.localPosition.z);
        pbar.transform.localScale = new Vector3(progress, 1, 1);
    }
}