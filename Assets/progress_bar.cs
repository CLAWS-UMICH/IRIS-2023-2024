using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class progress_bar : MonoBehaviour
{
    public float progress = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Progress()
    {
        this.gameObject.transform.localPosition = new Vector3((1-progress)*(-0.5f), this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
        this.gameObject.transform.localScale = new Vector3(progress, 1, 1);
    }
    
}
