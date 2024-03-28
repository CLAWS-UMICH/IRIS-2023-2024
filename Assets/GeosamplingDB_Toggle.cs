using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosamplingDB_Toggle : MonoBehaviour
{
    public void OnClick()
    {
        if (GeosamplingDB_Manager.Instance.isOpen)
        {
            GeosamplingDB_Manager.CloseScreen();
        }
        else
        {
            GeosamplingDB_Manager.OpenScreen();
        }
    }
}
