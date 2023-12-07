// Derek Yang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuState : MonoBehaviour
{

    [SerializeField] private bool isIRIS;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject TasksButton;
    [SerializeField] private GameObject NavigationButton;
    [SerializeField] private GameObject MessagesButton;
    [SerializeField] private GameObject SamplesButton;
    [SerializeField] private GameObject VitalsButton;
    [SerializeField] private GameObject ModesButton;
    [SerializeField] private GameObject IRISMenu;
    [SerializeField] private GameObject IRISSamplingButton;
    [SerializeField] private GameObject IRISNavigationButton;
    [SerializeField] private GameObject IRISRouteButton;
    [SerializeField] private GameObject IRISEgressButton;
    [SerializeField] private GameObject IRISAirlockButton;
    [SerializeField] private GameObject IRISCloseButton;
    [SerializeField] private Material regularModes;
    [SerializeField] private Material highlightedModes;

    public void ClickTasks()
    {
        
    }

    public void ClickNavigation()
    {
        
    }
    public void ClickMessages()
    {
        
    }
    public void ClickSamples()
    {
        
    }
    public void ClickVitals()
    {
        
    }

    public void ClickModes()
    {

        // Make IRIS Menu visible
        IRISMenu.SetActive(true);
        isIRIS = true;

        // Change modes button icon to highlighted
        ModesButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material = highlightedModes;

        // Change the backplate transparency of top menu icons
        TasksButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .5f);
        NavigationButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .5f);
        MessagesButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .5f);
        SamplesButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .5f);
        VitalsButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .5f);
    }

    public void ClickIRISSampling()
    {
        ClickHideMenu();
    }

    public void ClickIRISNavigation()
    {
        ClickHideMenu();
    }

    public void ClickIRISRoute()
    {
        ClickHideMenu();
    }

    public void ClickIRISEgress()
    {
        ClickHideMenu();
    }

    public void ClickIRISAirlock()
    {
        ClickHideMenu();
    }

    public void ClickIRISClose()
    {
        // Make IRIS Menu invisible
        IRISMenu.SetActive(false);
        isIRIS = false;

        // Change modes button icon to regular
        ModesButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material = regularModes;

        // Change the backplate transparency of top menu icons back to full opacity
        TasksButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
        NavigationButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
        MessagesButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
        SamplesButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
        VitalsButton.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
    }

    public void ClickHideMenu()
    {
        MainMenu.SetActive(false);
        IRISMenu.SetActive(false);
    }

    public void ClickShowMenu()
    {
        MainMenu.SetActive(true);
        if (isIRIS)
        {
            IRISMenu.SetActive(true);
        }
    }

}
