using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeosamplingDB_ZoneButton : MonoBehaviour
{
    [SerializeField] TextMeshPro ZoneLetter;

    public void SetZoneLetter(string letter)
    {
        ZoneLetter.text = letter;
    }
}
