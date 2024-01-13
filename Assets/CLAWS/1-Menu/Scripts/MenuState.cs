// Derek Yang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuState : MonoBehaviour
{

    [SerializeField] private bool isIRISMenuOpen;
    [SerializeField] private bool isIRISModeSelected;

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

    [SerializeField] private GameObject HideMenuButton;
    [SerializeField] private GameObject IRISHideMenuButton;
    [SerializeField] private GameObject ShowMenuButton;
    [SerializeField] private GameObject ExitIRISModeButton;

    [SerializeField] private GameObject SamplingMarkers;
    [SerializeField] private GameObject NavigationMarkers;
    [SerializeField] private GameObject RouteMarkers;
    [SerializeField] private GameObject EgressMarkers;
    [SerializeField] private GameObject AirlockMarkers;

    [SerializeField] private Material regularModes;
    [SerializeField] private Material highlightedModes;
    [SerializeField] private TMP_Text mode;

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
        isIRISMenuOpen = true;

        // Change hide menu buttons
        HideMenuButton.SetActive(false);
        IRISHideMenuButton.SetActive(true);

        // Show the exit IRIS mode button if an IRIS mode is currently selected
        if (isIRISModeSelected)
        {
            ExitIRISModeButton.SetActive(true);
        }

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

        // Update exit IRIS mode button text
        mode.text = "Exit Sampling Mode";
        isIRISModeSelected = true;

        // Filter markers on minimap
        SamplingMarkers.SetActive(true);
        NavigationMarkers.SetActive(false);
        RouteMarkers.SetActive(false);
        EgressMarkers.SetActive(false);
        AirlockMarkers.SetActive(false);
    }

    public void ClickIRISNavigation()
    {
        ClickHideMenu();

        // Update exit IRIS mode button text
        mode.text = "Exit Navigation Mode";
        isIRISModeSelected = true;

        // Filter markers on minimap
        SamplingMarkers.SetActive(false);
        NavigationMarkers.SetActive(true);
        RouteMarkers.SetActive(false);
        EgressMarkers.SetActive(false);
        AirlockMarkers.SetActive(false);
    }

    public void ClickIRISRoute()
    {
        ClickHideMenu();

        // Update exit IRIS mode button text
        mode.text = "Exit Route Mode";
        isIRISModeSelected = true;

        // Filter markers on minimap
        SamplingMarkers.SetActive(false);
        NavigationMarkers.SetActive(false);
        RouteMarkers.SetActive(true);
        EgressMarkers.SetActive(false);
        AirlockMarkers.SetActive(false);
    }

    public void ClickIRISEgress()
    {
        ClickHideMenu();

        // Update exit IRIS mode button text
        mode.text = "Exit Egress Mode";
        isIRISModeSelected = true;

        // Filter markers on minimap
        SamplingMarkers.SetActive(false);
        NavigationMarkers.SetActive(false);
        RouteMarkers.SetActive(false);
        EgressMarkers.SetActive(true);
        AirlockMarkers.SetActive(false);
    }

    public void ClickIRISAirlock()
    {
        ClickHideMenu();

        // Update exit IRIS mode button text
        mode.text = "Exit Airlock Mode";
        isIRISModeSelected = true;

        // Filter markers on minimap
        SamplingMarkers.SetActive(false);
        NavigationMarkers.SetActive(false);
        RouteMarkers.SetActive(false);
        EgressMarkers.SetActive(false);
        AirlockMarkers.SetActive(true);
    }

    public void ClickIRISClose()
    {
        // Make IRIS Menu invisible
        IRISMenu.SetActive(false);
        isIRISMenuOpen = false;
        ExitIRISModeButton.SetActive(false);

        // Change hide menu buttons
        HideMenuButton.SetActive(true);
        IRISHideMenuButton.SetActive(false);

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
        HideMenuButton.SetActive(false);
        IRISHideMenuButton.SetActive(false);
        ExitIRISModeButton.SetActive(false);
        ShowMenuButton.SetActive(true);
    }

    public void ClickShowMenu()
    {
        MainMenu.SetActive(true);
        ShowMenuButton.SetActive(false);
        if (isIRISMenuOpen)
        {
            IRISMenu.SetActive(true);
            IRISHideMenuButton.SetActive(true);
            
            if (isIRISModeSelected)
            {
                ExitIRISModeButton.SetActive(true);
            }
        }
        else
        {
            HideMenuButton.SetActive(true);
        }
    }

    public void ClickExitIRISMode()
    {
        isIRISModeSelected = false;
        ExitIRISModeButton.SetActive(false);

        // Unfilter markers on minimap
        SamplingMarkers.SetActive(true);
        NavigationMarkers.SetActive(true);
        RouteMarkers.SetActive(true);
        EgressMarkers.SetActive(true);
        AirlockMarkers.SetActive(true);
    }

}
