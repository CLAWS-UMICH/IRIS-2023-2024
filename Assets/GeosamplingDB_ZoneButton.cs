using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeosamplingDB_ZoneButton : MonoBehaviour
{
    [SerializeField] string Letter;
    [SerializeField] TextMeshPro ZoneLetter_tmp;

    public void SetZoneLetter(string letter)
    {
        Letter = letter;
        ZoneLetter_tmp.text = letter;
    }

    public void OnClick()
    {
        GeosamplingDB_Manager.Instance.OnZoneClicked(Letter);
    }
}
