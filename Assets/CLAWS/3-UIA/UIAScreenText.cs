using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenBackplate : MonoBehaviour
{

    public TextMeshPro UIA_msg;


    // Start is called before the first frame update
    void Start()
    {
        UIA_msg.text = "Open the EMU-1 Switch"; // default state
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
