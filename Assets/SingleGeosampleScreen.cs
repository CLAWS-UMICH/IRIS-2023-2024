using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public class SingleGeosampleScreen : MonoBehaviour
{

    public Geosample Sample;

    // TODO get references of all relevant data here
    //      and then drag and drop
    // Ex:
    // [SerializeField] TextMeshPro SampleName_TMP;

    private void Start()
    {
        // set zone if within a zone
        // set ID
        // set coordinates
        // set time
    }
    public void Load(Geosample Sample_f)
    {
        Sample = Sample_f;
        // Set all relevant data 
    }


    // ----------------- Buttons -----------------
    public void OnSampleButtonPressed()
    {
        // TODO show screen
    }
    public void OnZoneButtonPressed()
    {
        // TODO show screen
    }
    public void OnXRFButtonPressed()
    {
        // TODO show screen
    }
    public void OnShapeButtonPressed()
    {
        // TODO show screen
    }
    public void OnColorButtonPressed()
    {
        // TODO show screen
    }
    public void OnPhotoButtonPressed()
    {
        // TODO show screen
    }
    public void OnVEGAButtonPressed()
    {
        // TODO show screen
    }
    public void CloseAllScreens()
    {
        // TODO close all screens before opening a new one
        // so that only 1 screen is open at a time
    }


    // -------------- Setter Methods --------------
    public void SetSampleName(string name)
    {
        // TODO update tmp
    }
    public void SetID()
    {
        // this will be automatic assignment based on global ID
        // maybe a static variable for this class or smth
        // called on start function

        // TODO update tmp
    }
    public void SetZone(string name)
    {
        // TODO update tmp
    }
    public void SetCoordinates()
    {
        // also automatic assignment
        // called on start function

        // TODO update tmp
    }
    public void SetTime()
    {
        // also automatic assignment
        // called on start function

        // TODO update tmp
    }
    public void SetRockType()
    {
        // automatic assignment
        // called after xrf scan

        // TODO update tmp
    }
    public void SetShape()
    {
        // TODO update tmp and pic
    }
    public void SetColor()
    {
        // TODO update tmp and quad color
    }
    public void SetPhoto()
    {
        // TODO show photo
    }
    public void SetNote()
    {
        // TODO update tmp
    }


    // -------------- Screen Visuals --------------
    private void Update()
    {
        // Rotate to user
        transform.forward = transform.position - Camera.main.transform.position;
    }
    
}
