using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightLvlHierarchy : MonoBehaviour
{
    public List<string> levelsToCloseWhenClicked = new List<string>();

    public void OnClick()
    {
        EventBus.Publish(new UnHighlight(levelsToCloseWhenClicked));
    }
}
