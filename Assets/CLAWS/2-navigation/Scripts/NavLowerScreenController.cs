using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavLowerScreenController : MonoBehaviour
{
    GameObject parentScreen;
    GameObject addNewButton;
    GameObject mapButton;

    void Start()
    {
        parentScreen = transform.parent.Find("lowerBtnScreen").gameObject;
        addNewButton = parentScreen.transform.Find("addNewButton").gameObject;
        mapButton = parentScreen.transform.Find("3dMapButton").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
