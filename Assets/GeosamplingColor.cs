using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosamplingColor : MonoBehaviour
{
    public GameObject Default_icon;
    public SpriteRenderer Color_icon;
    
    public void SetColor(Color color)
    {
        Color_icon.gameObject.SetActive(true);
        Color_icon.color = color;
        
        Default_icon.SetActive(false);
    }

    public void SetColor(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            SetColor(color);
        }
        else
        {
            Debug.LogError("Hex color could not convert to color");
        }
    }


    [ContextMenu("func SetColor")]
    public void SetColorTest()
    {
        SetColor("#FF6060");
    }
}
