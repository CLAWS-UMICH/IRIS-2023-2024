using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using TMPro;

public class DetailedTask : MonoBehaviour
{
    [SerializeField] TextMeshPro TaskName;
    [SerializeField] TextMeshPro TaskData;
    [SerializeField] TextMeshPro TaskMetaData;

    [SerializeField] Transform Backplate;

    public void InitDetailedView(string name, string data, string metadata)
    {
        TaskName.text = name;
        TaskData.text = data;
        TaskMetaData.text = metadata;

        SetBackplateSize();
    }

    [ContextMenu("func SetBackplateSize")]
    public void SetBackplateSize()
    {
        // set backplate size
        float init_y = Backplate.localPosition.y;
        float init_height = Backplate.localScale.y;

        float height = 0.065f + (0.0107f * TaskData.textInfo.lineCount);
        float y = init_y - (height - init_height) / 2f;

        Backplate.localScale = new(Backplate.localScale.x, height, Backplate.localScale.z);
        Backplate.localPosition = new(Backplate.localPosition.x, y, Backplate.localPosition.z);
    }

    public void OnCloseButtonPressed()
    {
        Destroy(gameObject);
    }
}
