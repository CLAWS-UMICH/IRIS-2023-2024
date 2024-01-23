using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class progress_bar : MonoBehaviour
{
    private TextMeshPro fraction;
    private GameObject pbar;
    public float progress;
    // Start is called before the first frame update
    void Start()
    {
        fraction = transform.Find("Fraction").GetComponent<TextMeshPro>();
        pbar = transform.Find("pb_background").transform.Find("pb_bar").gameObject;
        pbar.transform.localScale = new Vector3(0, 1, 1);
        //fraction.text = 0 + "/" + 0;
    }

    public void Update_Progress_bar(int completed, int total)
    {
        Debug.Log("CALLED");
        progress = ((float)completed) / total;
        fraction.text = completed + "/" + total;
        pbar.transform.localPosition = new Vector3((1-progress)*(-0.5f), pbar.transform.localPosition.y, pbar.transform.localPosition.z);
        pbar.transform.localScale = new Vector3(progress, 1, 1);
    }

    public void Start_Text(int total)
    {
        fraction.text = 0 + "/" + total;
    }

    
    
}
