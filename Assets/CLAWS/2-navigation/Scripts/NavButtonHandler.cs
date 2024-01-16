using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NavButtonHandler : MonoBehaviour
{
    string letter;
    // Start is called before the first frame update
    void Start()
    {
        letter = transform.Find("IconAndText").gameObject.transform.Find("LetterText").gameObject.GetComponent<TextMeshPro>().text;
    }

    public void OnClick()
    {
        //EventBus.Publish(new SelectButton(letter));
    }
}
