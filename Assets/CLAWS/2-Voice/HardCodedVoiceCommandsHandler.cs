using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardCodedVoiceCommandsHandler : MonoBehaviour
{
    // Parents
    GameObject Menu_Parent;
    GameObject Tasklist_Parent;
    GameObject Navigation_Parent;
    GameObject Messaging_Parent;
    GameObject Geo_Parent;
    GameObject Vitals_Parent;
    GameObject ScreenSent_Parent;

    // Tasklist
    GameObject Tasklist;
    GameObject Tasklist_SubOpen;
    GameObject Tasklist_Emergency;

    // Navigation
    GameObject Navigation_SelectStationNav;
    GameObject Navigation_SelectPOINav;
    GameObject Navigation_SelectGeoNav;
    GameObject Navigation_SelectCompNav;
    GameObject Navigation_Confirmation;
    GameObject Navigation_CreatingWaypoint;
    GameObject Navigation_3D;

    // Messaging
    GameObject Messaging_Astro_BlankMessage;
    GameObject Messaging_Astro_Quick;
    GameObject Messaging_Astro_FullMessage;
    GameObject Messaging_LLMC_BlankMessage;
    GameObject Messaging_LLMC_Quick;
    GameObject Messaging_LLMC_FullMessage;
    GameObject Messaging_GroupChat_BlankMessage;
    GameObject Messaging_GroupChat_Quick;
    GameObject Messaging_GroupChat_FullMessage;

    // Vitals
    GameObject Vitals_Main;
    GameObject Vitals_Fellow;

    Subscription<ScreenChangedEvent> screenChangedSubscription;

    Dictionary<Screens, List<string>> commandMap;

    // Start is called before the first frame update
    void Start()
    {
        Menu_Parent = transform.Find("Menu").gameObject;
        Tasklist_Parent = transform.Find("Tasklist").gameObject;
        Navigation_Parent = transform.Find("Navigation").gameObject;
        Messaging_Parent = transform.Find("Messaging").gameObject;
        Geo_Parent = transform.Find("Geo").gameObject;
        Vitals_Parent = transform.Find("Vitals").gameObject;
        ScreenSent_Parent = transform.Find("ScreenSent").gameObject;

        Tasklist = Tasklist_Parent.transform.Find("Tasklist").gameObject;
        Tasklist_SubOpen = Tasklist_Parent.transform.Find("Tasklist_SubOpen").gameObject;
        Tasklist_Emergency = Tasklist_Parent.transform.Find("Tasklist_Emergency").gameObject;

        Navigation_SelectStationNav = Navigation_Parent.transform.Find("Navigation_selectStationNav").gameObject;
        Navigation_SelectPOINav = Navigation_Parent.transform.Find("Navigation_SelectPOINav").gameObject;
        Navigation_SelectGeoNav = Navigation_Parent.transform.Find("Navigation_SelectGeoNav").gameObject;
        Navigation_SelectCompNav = Navigation_Parent.transform.Find("Navigation_SelectCompNav").gameObject;
        Navigation_Confirmation = Navigation_Parent.transform.Find("Navigation_Confirmation").gameObject;
        Navigation_CreatingWaypoint = Navigation_Parent.transform.Find("Navigation_CreatingWaypoint").gameObject;
        Navigation_3D = Navigation_Parent.transform.Find("Navigation_3D").gameObject;

        Messaging_Astro_BlankMessage = Messaging_Parent.transform.Find("Messaging_Astro_BlankMessage").gameObject;
        Messaging_Astro_Quick = Messaging_Parent.transform.Find("Messaging_Astro_Quick").gameObject;
        Messaging_Astro_FullMessage = Messaging_Parent.transform.Find("Messaging_Astro_FullMessage").gameObject;
        Messaging_LLMC_BlankMessage = Messaging_Parent.transform.Find("Messaging_LLMC_BlankMessage").gameObject;
        Messaging_LLMC_Quick = Messaging_Parent.transform.Find("Messaging_LLMC_Quick").gameObject;
        Messaging_LLMC_FullMessage = Messaging_Parent.transform.Find("Messaging_LLMC_FullMessage").gameObject;
        Messaging_GroupChat_BlankMessage = Messaging_Parent.transform.Find("Messaging_GroupChat_BlankMessage").gameObject;
        Messaging_GroupChat_Quick = Messaging_Parent.transform.Find("Messaging_GroupChat_Quick").gameObject;
        Messaging_GroupChat_FullMessage = Messaging_Parent.transform.Find("Messaging_GroupChat_FullMessage").gameObject;

        Vitals_Main = Vitals_Parent.transform.Find("Vitals_Main").gameObject;
        Vitals_Fellow = Vitals_Parent.transform.Find("Vitals_Fellow").gameObject;

        screenChangedSubscription = EventBus.Subscribe<ScreenChangedEvent>(OnSwitchScreen);
        TurnOffAllScreens();

        commandMap = new Dictionary<Screens, List<string>>
        {
            { Screens.Menu, new List<string> { "Open Tasks", "Open Navigation", "Open Messaging", "Open Samples", "Open Vitals", "Place UIA Panel", "Configure UIA", "Show UIA Cubes", "Screen Fix", "Screen Follow", "Screen Sync" } },

            { Screens.Tasklist, new List<string> { "Scroll Down", "Scroll Up", "Close Screen" } },
            { Screens.Tasklist_SubOpen, new List<string> { "Close Screen" } },
            { Screens.Tasklist_Emergency, new List<string> { "Scroll Down", "Scroll Up", "Close Screen" } },

            { Screens.Navigation_SelectStationNav, new List<string> { "Select Companions", "Select Interest Points", "Select Sample Zones", "Add New", "Open 3D Map", "Close Screen", "Scroll Down", "Scroll Up", "Select Hotel", "Select India"} },
            { Screens.Navigation_SelectPOINav, new List<string> { "Select Companions", "Select Stations", "Select Sample Zones", "Add New", "Open 3D Map", "Close Screen", "Scroll Down", "Scroll Up" } },
            { Screens.Navigation_SelectGeoNav, new List<string> { "Select Companions", "Select Stations", "Select Interest Points", "Add New", "Open 3D Map", "Close Screen", "Scroll Down", "Scroll Up", "Select Alpha", "Select Bravo", "Select Charlie", "Select Delta", "Select Echo", "Select Foxtrot" } },
            { Screens.Navigation_SelectCompNav, new List<string> { "Select Stations", "Select Interest Points", "Select Sample Zones", "Add New", "Open 3D Map", "Close Screen", "Scroll Down", "Scroll Up", "Select Romeo", "Select X-ray", "Select Yankee" } },
            { Screens.Navigation_Confirmation, new List<string> { "Navigate", "Cancel", "Close Screen" } },
            { Screens.Navigation_CreatingWaypoint, new List<string> { "Close Screen" } },
            { Screens.Navigation_3D, new List<string> { "Close Screen" } },

            { Screens.Messaging_Astro_BlankMessage, new List<string> {  } },
            { Screens.Messaging_Astro_Quick, new List<string> {  } },
            { Screens.Messaging_Astro_FullMessage, new List<string> {  } },
            { Screens.Messaging_LLMC_BlankMessage, new List<string> {  } },
            { Screens.Messaging_LLMC_Quick, new List<string> {  } },
            { Screens.Messaging_LLMC_FullMessage, new List<string> {  } },
            { Screens.Messaging_GroupChat_BlankMessage, new List<string> {  } },
            { Screens.Messaging_GroupChat_Quick, new List<string> {  } },
            { Screens.Messaging_GroupChat_FullMessage, new List<string> {  } },

            { Screens.Geo, new List<string> { "Add Sample", "View Samples", "Exit Mode", "Identify Potential Samples" } }, // "Sample Name", "Take XRF Scan", "Set Shape", "Set Color", "Take Photo", "Enter Voice Notes" 
            { Screens.Geo_Database, new List<string> { "Start Geosampling Mode", "Close Screen" } },

            { Screens.UIA, new List<string> { "UIA Next", "UIA Complete", "Start UIA", "Show UIA Cubes", "Configure UIA" } },

            { Screens.Vitals_Main, new List<string> { "Select Astro", "Close Screen" } },
            { Screens.Vitals_Fellow, new List<string> { "Select Astro", "Close Screen" } },

            { Screens.Screen_Sent, new List<string> { "Close Screen" } },
        };
    }

    public void TurnOffAllScreens()
    {
        Menu_Parent.SetActive(false);
        Tasklist.SetActive(false);
        Tasklist_SubOpen.SetActive(false);
        Tasklist_Emergency.SetActive(false);
        Navigation_SelectStationNav.SetActive(false);
        Navigation_SelectPOINav.SetActive(false);
        Navigation_SelectGeoNav.SetActive(false);
        Navigation_SelectCompNav.SetActive(false);
        Navigation_Confirmation.SetActive(false);
        Navigation_CreatingWaypoint.SetActive(false);
        Navigation_3D.SetActive(false);
        Messaging_Astro_BlankMessage.SetActive(false);
        Messaging_Astro_Quick.SetActive(false);
        Messaging_Astro_FullMessage.SetActive(false);
        Messaging_LLMC_BlankMessage.SetActive(false);
        Messaging_LLMC_Quick.SetActive(false);
        Messaging_LLMC_FullMessage.SetActive(false);
        Messaging_GroupChat_BlankMessage.SetActive(false);
        Messaging_GroupChat_Quick.SetActive(false);
        Messaging_GroupChat_FullMessage.SetActive(false);
        Vitals_Main.SetActive(false);
        Vitals_Fellow.SetActive(false);
    }

    public void OnSwitchScreen(ScreenChangedEvent e)
    {
        SwitchScreen(e.Screen);
    }

    public void SwitchScreen(Screens s)
    {
        TurnOffAllScreens();

        switch (s)
        {
            case Screens.Menu:
                Menu_Parent.SetActive(true);
                break;
            case Screens.Tasklist:
                Tasklist.SetActive(true);
                break;
            case Screens.Tasklist_SubOpen:
                Tasklist_SubOpen.SetActive(true);
                break;
            case Screens.Tasklist_Emergency:
                Tasklist_Emergency.SetActive(true);
                break;
            case Screens.Navigation:
                Navigation_SelectStationNav.SetActive(true);
                break;
            case Screens.Navigation_SelectStationNav:
                Navigation_SelectStationNav.SetActive(true);
                break;
            case Screens.Navigation_SelectPOINav:
                Navigation_SelectPOINav.SetActive(true);
                break;
            case Screens.Navigation_SelectGeoNav:
                Navigation_SelectGeoNav.SetActive(true);
                break;
            case Screens.Navigation_SelectCompNav:
                Navigation_SelectCompNav.SetActive(true);
                break;
            case Screens.Navigation_Confirmation:
                Navigation_Confirmation.SetActive(true);
                break;
            case Screens.Navigation_CreatingWaypoint:
                Navigation_CreatingWaypoint.SetActive(true);
                break;
            case Screens.Navigation_3D:
                Navigation_3D.SetActive(true);
                break;
            case Screens.Messaging_Astro_BlankMessage:
                Messaging_Astro_BlankMessage.SetActive(true);
                break;
            case Screens.Messaging_Astro_Quick:
                Messaging_Astro_Quick.SetActive(true);
                break;
            case Screens.Messaging_Astro_FullMessage:
                Messaging_Astro_FullMessage.SetActive(true);
                break;
            case Screens.Messaging_LLMC_BlankMessage:
                Messaging_LLMC_BlankMessage.SetActive(true);
                break;
            case Screens.Messaging_LLMC_Quick:
                Messaging_LLMC_Quick.SetActive(true);
                break;
            case Screens.Messaging_LLMC_FullMessage:
                Messaging_LLMC_FullMessage.SetActive(true);
                break;
            case Screens.Messaging_GroupChat_BlankMessage:
                Messaging_GroupChat_BlankMessage.SetActive(true);
                break;
            case Screens.Messaging_GroupChat_Quick:
                Messaging_GroupChat_Quick.SetActive(true);
                break;
            case Screens.Messaging_GroupChat_FullMessage:
                Messaging_GroupChat_FullMessage.SetActive(true);
                break;
            case Screens.Geo:
                Geo_Parent.SetActive(true);
                break;
            case Screens.Vitals_Main:
                Vitals_Main.SetActive(true);
                break;
            case Screens.Vitals_Fellow:
                Vitals_Fellow.SetActive(true);
                break;
            case Screens.Screen_Sent:
                ScreenSent_Parent.SetActive(true);
                break;
        }


        GameObject.Find("SampleCommandScreen").GetComponent<SampleCommandHandler>().setCommands(commandMap[s]);
    }

    public void PlayMusic()
    {
        EventBus.Publish<PlayAudio>(new PlayAudio("Rickroll"));
    }



}
