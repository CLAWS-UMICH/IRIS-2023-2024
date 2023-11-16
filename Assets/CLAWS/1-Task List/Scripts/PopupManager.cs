using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

public class PopupManager : MonoBehaviour
{
    [SerializeField] GameObject PopupPrefab;
    private GridObjectCollection GridObjCollection;

    void Start()
    {
        EventBus.Subscribe<PopupEvent>(CreatePopup);
        GridObjCollection = GetComponent<GridObjectCollection>();
    }

    [ContextMenu("Func CreateFakePopup")]
    void CreateFakePopup()
    {
        CreatePopup(new PopupEvent("Debug Popup", "This is an example popup"));
    }

    void CreatePopup(PopupEvent e)
    {
        GameObject popup = Instantiate(PopupPrefab, transform);
        popup.GetComponent<Popup>().SetPopup(e.popupType, e.popupText);
        GridObjCollection.UpdateCollection();
    }

}
