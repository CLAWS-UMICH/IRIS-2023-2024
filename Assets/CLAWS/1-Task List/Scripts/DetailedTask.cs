using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using TMPro;

public class DetailedTask : MonoBehaviour
{
    TextMeshPro TaskName;
    TextMeshPro TaskData;
    TextMeshPro TaskMetaData;

    // Start is called before the first frame update
    void Start()
    {
    }


    public void InitDetailedView(string name, string data, string metadata)
    {
        TaskName.text = name;
        TaskData.text = data;
        TaskMetaData.text = metadata;
    }
}
