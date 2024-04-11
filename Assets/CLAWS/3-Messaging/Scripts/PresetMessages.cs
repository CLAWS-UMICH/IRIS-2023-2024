using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;

public class PresetMessages : MonoBehaviour
{
    [SerializeField] private TMP_Text message;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickConfirmed()
    {
        message.SetText("Confirmed");
    }

    public void ClickRejected()
    {
        message.SetText("Rejected");
    }

    public void ClickNotReady()
    {
        message.SetText("Not Ready");
    }
}
