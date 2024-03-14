using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class progress_bar_nav : MonoBehaviour
{
    private GameObject pbar;
    public float progress;
    void Start()
    {
        pbar = transform.Find("pb_background").transform.Find("pb_bar").gameObject;
        Debug.Log("");
        pbar.transform.localScale = new Vector3(0, 1, 1);
    }
    public void Update_Progress_bar(float p)
    {
        Debug.Log(p);
        pbar = transform.Find("pb_background").transform.Find("pb_bar").gameObject;
        pbar.transform.localScale = new Vector3(0, 1, 1);
        progress = p/100;
        if (pbar != null)
        {
            pbar.transform.localPosition = new Vector3((1 - progress) * (-0.5f), pbar.transform.localPosition.y, pbar.transform.localPosition.z);
        }
        else
        {
            Debug.LogError("pbar is null");
        }
        pbar.transform.localScale = new Vector3(progress, 1, 1);
        Debug.Log("working");
    }

}

