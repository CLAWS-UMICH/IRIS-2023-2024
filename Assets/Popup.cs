using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    [SerializeField] TextMeshPro popupType_tmp;
    [SerializeField] TextMeshPro popupText_tmp;

    void Start()
    {
        StartCoroutine(DestroyAfterSeconds(3f));
    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    public void SetPopup(string in_popupType, string in_popupText)
    {
        popupType_tmp.text = in_popupType;
        popupText_tmp.text = in_popupText;
    }
}
