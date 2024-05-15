using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class VEGAManager : MonoBehaviour
{
    [System.Serializable]
    public class FunctionEvent : UnityEvent<List<string>> { }

    // MENU Events
    public FunctionEvent menuOpenTasks = new FunctionEvent();
    public FunctionEvent menuOpenNavigation = new FunctionEvent();
    public FunctionEvent menuOpenMessaging = new FunctionEvent();
    public FunctionEvent menuOpenGeo = new FunctionEvent();
    public FunctionEvent menuOpenVitals = new FunctionEvent();
    public FunctionEvent menuOpenUIA = new FunctionEvent();
    public FunctionEvent menuComplete = new FunctionEvent();
    public FunctionEvent menuCheck = new FunctionEvent();
    public FunctionEvent menuOpenSubDetails = new FunctionEvent();

    // Tasklist Events
    public FunctionEvent tasklistComplete = new FunctionEvent();
    public FunctionEvent tasklistOpenDetails = new FunctionEvent();
    public FunctionEvent tasklistCloseDetails = new FunctionEvent();
    public FunctionEvent tasklistCloseScreen = new FunctionEvent();
    public FunctionEvent tasklistCloseSubtaskScreen = new FunctionEvent();
    public FunctionEvent tasklistScrollUp = new FunctionEvent();
    public FunctionEvent tasklistScrollDown = new FunctionEvent();

    // Navigation Events
    public FunctionEvent navigationSelectLetters = new FunctionEvent();
    public FunctionEvent navigationSelectWaypoint = new FunctionEvent();
    public FunctionEvent navigationAddNew = new FunctionEvent();
    public FunctionEvent navigationOpen3DMap = new FunctionEvent();
    public FunctionEvent navigationConfirmationNavigate = new FunctionEvent();
    public FunctionEvent navigationCreatingWaypointEnterName = new FunctionEvent();
    public FunctionEvent navigationCreatingWaypointCreate = new FunctionEvent();
    public FunctionEvent navigationCloseScreen = new FunctionEvent();
    public FunctionEvent navigationCloseConfirmationScreen = new FunctionEvent();
    public FunctionEvent navigationCloseCreatingWaypointScreen = new FunctionEvent();
    public FunctionEvent navigationScrollUp = new FunctionEvent();
    public FunctionEvent navigationScrollDown = new FunctionEvent();
    public FunctionEvent navigationSelectCompanion = new FunctionEvent();
    public FunctionEvent navigationSelectDanger = new FunctionEvent();
    public FunctionEvent navigationSelectStationNAV = new FunctionEvent();
    public FunctionEvent navigationSelectStationWAY = new FunctionEvent();
    public FunctionEvent navigationSelectInterestNAV = new FunctionEvent();
    public FunctionEvent navigationSelectInterestWAY = new FunctionEvent();
    public FunctionEvent navigationSelectGeoNAV = new FunctionEvent();
    public FunctionEvent navigationSelectGeoWAY = new FunctionEvent();
    public FunctionEvent navigationCancelNavConfirmation = new FunctionEvent();
    public FunctionEvent navigationCancelWaypoint = new FunctionEvent();

    // Messaging Events
    public FunctionEvent messagingLLMC = new FunctionEvent();
    public FunctionEvent messagingLLMCAstro = new FunctionEvent();
    public FunctionEvent messagingAddEmoji = new FunctionEvent();
    public FunctionEvent messagingEnterMessage = new FunctionEvent();
    public FunctionEvent messagingSendMessage = new FunctionEvent();
    public FunctionEvent messagingCancelMessage = new FunctionEvent();
    public FunctionEvent messagingLike = new FunctionEvent();
    public FunctionEvent messagingDislike = new FunctionEvent();
    public FunctionEvent messagingOneHundred = new FunctionEvent();
    public FunctionEvent messagingDanger = new FunctionEvent();
    public FunctionEvent messagingCloseScreen = new FunctionEvent();
    public FunctionEvent messagingCloseEmojiScreen = new FunctionEvent();
    public FunctionEvent messagingScrollUp = new FunctionEvent();
    public FunctionEvent messagingScrollDown = new FunctionEvent();

    // Vitals Events
    public FunctionEvent vitalsSelectAstro = new FunctionEvent();
    public FunctionEvent vitalsCloseScreen = new FunctionEvent();

    // Screen Sent Events
    public FunctionEvent screenSentCloseScreen = new FunctionEvent();

    Subscription<VEGACommand> vegaCommandEvent;

    private Dictionary<Screens, List<string>> screenAssociatedCommands;
    private Dictionary<Modes, List<string>> modeAssociatedCommands;
    private Dictionary<string, (Action<List<string>>, int)> functionDictionary;

    Screens currentScreen;
    Modes currentMode;

    void Start()
    {
        vegaCommandEvent = EventBus.Subscribe<VEGACommand>(onVEGACommand);

        // Initialize screen-associated commands
        screenAssociatedCommands = new Dictionary<Screens, List<string>>
        {
            { Screens.Menu, new List<string> { "Open_Tasks", "Open_Navigation", "Open_Messaging", "Open_Geo", "Open_Vitals", "Open_UIA", "Complete", "Check", "Open_Details" } },

            { Screens.Tasklist, new List<string> { "Complete", "Open_Details", "Scroll_Down", "Scroll_Up", "Close", "Close_Details" } },
            { Screens.Tasklist_SubOpen, new List<string> { "Close" } },
            { Screens.Tasklist_Emergency, new List<string> { "Complete", "Open_Details", "Scroll_Down", "Scroll_Up", "Close", "Close_Details" } },

            { Screens.Navigation, new List<string> { "Select_Companion", "Select_Station", "Select_Interest", "Select_Geo", "Select_Letter", "Select_Waypoint", "Add_New", "Open_3D_Map", "Close", "Scroll_Down", "Scroll_Up" } },
            { Screens.Navigation_SelectStationNav, new List<string> { "Select_Companions", "Select_Station", "Select_Interest", "Select_Geo", "Select_Letter", "Select_Waypoint", "Add_New", "Open_3D_Map", "Close", "Scroll_Down", "Scroll_Up" } },
            { Screens.Navigation_SelectPOINav, new List<string> { "Select_Companions", "Select_Station", "Select_Interest", "Select_Geo", "Select_Letter", "Select_Waypoint", "Add_New", "Open_3D_Map", "Close", "Scroll_Down", "Scroll_Up" } },
            { Screens.Navigation_SelectGeoNav, new List<string> { "Select_Companions", "Select_Station", "Select_Interest", "Select_Geo", "Select_Letter", "Select_Waypoint", "Add_New", "Open_3D_Map", "Close", "Scroll_Down", "Scroll_Up" } },
            { Screens.Navigation_SelectCompNav, new List<string> { "Select_Companions", "Select_Station", "Select_Interest", "Select_Geo", "Select_Letter", "Select_Waypoint", "Add_New", "Open_3D_Map", "Close", "Scroll_Down", "Scroll_Up" } },
            { Screens.Navigation_Confirmation, new List<string> { "Navigate", "Cancel", "Close" } },
            { Screens.Navigation_CreatingWaypoint, new List<string> { "Enter_Name", "Select_Station", "Select_Interest", "Select_Danger", "Select_Geo", "Create", "Cancel", "Close" } },

            { Screens.Messaging_Astro_BlankMessage, new List<string> { "LLMC", "LLMC_Astro", "Add_Emoji", "Enter_Message", "Close", "Scroll_Down", "Scroll_Up" } },
            { Screens.Messaging_Astro_FullMessage, new List<string> { "LLMC", "LLMC_Astro", "Add_Emoji", "Send_Message", "Close", "Scroll_Down", "Scroll_Up", "Cancel" } },
            { Screens.Messaging_Astro_Quick, new List<string> { "Like", "Dislike", "One_Hundred", "Danger", "Close" } },
            { Screens.Messaging_LLMC_BlankMessage, new List<string> { "LLMC", "LLMC_Astro", "Add_Emoji", "Enter_Message", "Close", "Scroll_Down", "Scroll_Up" } },
            { Screens.Messaging_LLMC_FullMessage, new List<string> { "LLMC", "LLMC_Astro", "Add_Emoji", "Send_Message", "Close", "Scroll_Down", "Scroll_Up", "Cancel" } },
            { Screens.Messaging_LLMC_Quick, new List<string> { "Like", "Dislike", "One_Hundred", "Danger", "Close" } },
            { Screens.Messaging_GroupChat_BlankMessage, new List<string> { "LLMC", "LLMC_Astro", "Add_Emoji", "Enter_Message", "Close", "Scroll_Down", "Scroll_Up" } },
            { Screens.Messaging_GroupChat_FullMessage, new List<string> { "LLMC", "LLMC_Astro", "Add_Emoji", "Send_Message", "Close", "Scroll_Down", "Scroll_Up", "Cancel" } },
            { Screens.Messaging_GroupChat_Quick, new List<string> { "Like", "Dislike", "One_Hundred", "Danger", "Close" } },

            { Screens.Vitals_Main, new List<string> { "Select_Astro", "Close" } },
            { Screens.Vitals_Fellow, new List<string> { "Select_Astro", "Close" } },

            { Screens.Screen_Sent, new List<string> { "Close" } },
        };

        // Initialize mode-associated commands
        modeAssociatedCommands = new Dictionary<Modes, List<string>>
        {
            { Modes.Navigation, new List<string> { "apple", "banana" } },
            { Modes.Normal, new List<string> { "car", "bus" } },
        };

        // Initialize function dictionary with function name and parameter count
        functionDictionary = new Dictionary<string, (Action<List<string>>, int)>
        {
            { "Scroll_Down", (Scroll_Down, 0) },
            { "Scroll_Up", (Scroll_Up, 0) },
            { "Close", (Close, 0) },
            { "Cancel", (Cancel, 0) },
            { "Select_Station", (Select_Station, 0) },
            { "Select_Interest", (Select_Interest, 0) },
            { "Select_Geo", (Select_Geo, 0) },

            { "Open_Tasks", (Menu_Open_Tasks, 0) },
            { "Open_Navigation", (Menu_Open_Navigation, 0) },
            { "Open_Messaging", (Menu_Open_Messaging, 0) },
            { "Open_Geo", (Menu_Open_Geo, 0) },
            { "Open_Vitals", (Menu_Open_Vitals, 0) },
            { "Open_UIA", (Menu_Open_UIA, 0) },
            { "Complete", (Menu_Complete, 0) },
            { "Check", (Menu_Check, 0) },
            { "Open_Sub_Details", (Menu_Open_Sub_Details, 1) },

            { "Complete", (Tasklist_Complete, 1) },
            { "Open_Details", (Tasklist_Open_Details, 1) },
            { "Close_Details", (Tasklist_Close_Details, 1) },

            { "Select_Letter", (Navigation_Select_Letters, 1) },
            { "Select_Waypoint", (Navigation_Select_Waypoint, 1) },
            { "Add_New", (Navigation_Add_New, 0) },
            { "Open_3D_Map", (Navigation_Open_3D_Map, 0) },
            { "Navigate", (Navigation_Confirmation_Navigate, 0) },
            { "Enter_Name", (Navigation_CreatingWaypoint_Enter_Name, 0) },
            { "Create", (Navigation_CreatingWaypoint_Create, 0) },
            { "Select_Companion", (Navigation_Select_Companion, 0) },
            { "Select_Danger", (Navigation_Select_Danger, 0) },

            { "LLMC", (Messaging_LLMC, 0) },
            { "LLMC_Astro", (Messaging_LLMC_Astro, 0) },
            { "Add_Emoji", (Messaging_Add_Emoji, 0) },
            { "Enter_Message", (Messaging_Enter_Message, 0) },
            { "Send_Message", (Messaging_Send_Message, 0) },
            { "Like", (Messaging_Like, 0) },
            { "Dislike", (Messaging_Dislike, 0) },
            { "One_Hundred", (Messaging_One_Hundred, 0) },
            { "Danger", (Messaging_Danger, 0) },

            { "Select_Astro", (Vitals_Select_Astro, 0) },
        };
    }

    private void onVEGACommand(VEGACommand c)
    {
        currentScreen = StateMachine.CurrScreen;
        currentMode = StateMachine.CurrMode;

        // Check if the command matches any associated command for the current screen or mode
        if (screenAssociatedCommands.ContainsKey(currentScreen) && screenAssociatedCommands[currentScreen].Contains(c.command_name))
        {
            ExecuteCommand(c.command_name, c.arguments);
        }
        else if (modeAssociatedCommands.ContainsKey(currentMode) && modeAssociatedCommands[currentMode].Contains(c.command_name))
        {
            ExecuteCommand(c.command_name, c.arguments);
        }
        else
        {
            Debug.LogError("Command name does not match any associated command for the current screen or mode");
        }
    }

    private void ExecuteCommand(string commandName, List<string> arguments)
    {
        if (functionDictionary.ContainsKey(commandName))
        {
            var (action, parameterCount) = functionDictionary[commandName];
            if (arguments.Count == parameterCount)
            {
                action(arguments);
            }
            else
            {
                Debug.LogError($"Command '{commandName}' requires {parameterCount} parameters");
            }
        }
        else
        {
            Debug.LogError($"Command '{commandName}' not found in function dictionary");
        }
    }

    // Main Functions
    private void Scroll_Down(List<string> parameters)
    {
        if (currentScreen == Screens.Tasklist || currentScreen == Screens.Tasklist_Emergency)
        {
            InvokeEvent(tasklistScrollDown, "Scroll_Down", parameters);
        } 
        else if (currentScreen == Screens.Navigation || currentScreen == Screens.Navigation_SelectCompNav || currentScreen == Screens.Navigation_SelectGeoNav || currentScreen == Screens.Navigation_SelectPOINav || currentScreen == Screens.Navigation_SelectStationNav)
        {
            InvokeEvent(navigationScrollDown, "Scroll_Down", parameters);
        }
        else if (currentScreen == Screens.Messaging_Astro_BlankMessage || currentScreen == Screens.Messaging_Astro_FullMessage || currentScreen == Screens.Messaging_GroupChat_BlankMessage || currentScreen == Screens.Messaging_GroupChat_FullMessage || currentScreen == Screens.Messaging_LLMC_BlankMessage || currentScreen == Screens.Messaging_LLMC_FullMessage)
        {
            InvokeEvent(messagingScrollDown, "Scroll_Down", parameters);
        } 
        else
        {
            Debug.Log("Unable to match voice command 'Scroll Down' with current screen");
        }
    }

    private void Scroll_Up(List<string> parameters)
    {
        if (currentScreen == Screens.Tasklist || currentScreen == Screens.Tasklist_Emergency)
        {
            InvokeEvent(tasklistScrollUp, "Scroll_Up", parameters);
        }
        else if (currentScreen == Screens.Navigation || currentScreen == Screens.Navigation_SelectCompNav || currentScreen == Screens.Navigation_SelectGeoNav || currentScreen == Screens.Navigation_SelectPOINav || currentScreen == Screens.Navigation_SelectStationNav)
        {
            InvokeEvent(navigationScrollUp, "Scroll_Up", parameters);
        }
        else if (currentScreen == Screens.Messaging_Astro_BlankMessage || currentScreen == Screens.Messaging_Astro_FullMessage || currentScreen == Screens.Messaging_GroupChat_BlankMessage || currentScreen == Screens.Messaging_GroupChat_FullMessage || currentScreen == Screens.Messaging_LLMC_BlankMessage || currentScreen == Screens.Messaging_LLMC_FullMessage)
        {
            InvokeEvent(messagingScrollUp, "Scroll_Up", parameters);
        }
        else
        {
            Debug.Log("Unable to match voice command 'Scroll Up' with current screen");
        }
    }

    private void Close(List<string> parameters)
    {
        if (currentScreen == Screens.Tasklist || currentScreen == Screens.Tasklist_Emergency)
        {
            InvokeEvent(tasklistCloseScreen, "Close", parameters);
        }
        else if (currentScreen == Screens.Tasklist_SubOpen)
        {
            InvokeEvent(tasklistCloseSubtaskScreen, "Close", parameters);
        }
        else if (currentScreen == Screens.Navigation || currentScreen == Screens.Navigation_SelectCompNav || currentScreen == Screens.Navigation_SelectGeoNav || currentScreen == Screens.Navigation_SelectPOINav || currentScreen == Screens.Navigation_SelectStationNav)
        {
            InvokeEvent(navigationCloseScreen, "Close", parameters);
        }
        else if (currentScreen == Screens.Navigation_Confirmation)
        {
            InvokeEvent(navigationCloseConfirmationScreen, "Close", parameters);
        }
        else if (currentScreen == Screens.Navigation_CreatingWaypoint)
        {
            InvokeEvent(navigationCloseCreatingWaypointScreen, "Close", parameters);
        }
        else if (currentScreen == Screens.Messaging_Astro_BlankMessage || currentScreen == Screens.Messaging_Astro_FullMessage || currentScreen == Screens.Messaging_GroupChat_BlankMessage || currentScreen == Screens.Messaging_GroupChat_FullMessage || currentScreen == Screens.Messaging_LLMC_BlankMessage || currentScreen == Screens.Messaging_LLMC_FullMessage)
        {
            InvokeEvent(messagingCloseScreen, "Close", parameters);
        }
        else if (currentScreen == Screens.Messaging_Astro_Quick || currentScreen == Screens.Messaging_GroupChat_Quick || currentScreen == Screens.Messaging_LLMC_Quick)
        {
            InvokeEvent(messagingCloseEmojiScreen, "Close", parameters);
        }
        else if (currentScreen == Screens.Vitals_Fellow || currentScreen == Screens.Vitals_Main)
        {
            InvokeEvent(vitalsCloseScreen, "Close", parameters);
        }
        else if (currentScreen == Screens.Screen_Sent)
        {
            InvokeEvent(screenSentCloseScreen, "Close", parameters);
        }
        else
        {
            Debug.Log("Unable to match voice command 'Close' with current screen");
        }
    }

    private void Cancel(List<string> parameters)
    {
        if (currentScreen == Screens.Navigation_Confirmation)
        {
            InvokeEvent(navigationCancelNavConfirmation, "Cancel", parameters);
        }
        else if (currentScreen == Screens.Navigation_CreatingWaypoint)
        {
            InvokeEvent(navigationCancelWaypoint, "Cancel", parameters);
        }
        else if (currentScreen == Screens.Messaging_Astro_FullMessage || currentScreen == Screens.Messaging_GroupChat_FullMessage || currentScreen == Screens.Messaging_LLMC_FullMessage)
        {
            InvokeEvent(messagingCancelMessage, "Cancel", parameters);
        }
        else
        {
            Debug.Log("Unable to match voice command 'Cancel' with current screen");
        }
    }

    private void Select_Station(List<string> parameters)
    {
        if (currentScreen == Screens.Navigation || currentScreen == Screens.Navigation_SelectCompNav || currentScreen == Screens.Navigation_SelectGeoNav || currentScreen == Screens.Navigation_SelectPOINav || currentScreen == Screens.Navigation_SelectStationNav)
        {
            InvokeEvent(navigationSelectStationNAV, "Select_Station", parameters);
        } else if (currentScreen == Screens.Navigation_CreatingWaypoint)
        {
            InvokeEvent(navigationSelectStationWAY, "Select_Station", parameters);
        } 
        else
        {
            Debug.Log("Unable to match voice command 'Select_Station' with current screen");
        }
    }

    private void Select_Interest(List<string> parameters)
    {
        if (currentScreen == Screens.Navigation || currentScreen == Screens.Navigation_SelectCompNav || currentScreen == Screens.Navigation_SelectGeoNav || currentScreen == Screens.Navigation_SelectPOINav || currentScreen == Screens.Navigation_SelectStationNav)
        {
            InvokeEvent(navigationSelectInterestNAV, "Select_Interest", parameters);
        }
        else if (currentScreen == Screens.Navigation_CreatingWaypoint)
        {
            InvokeEvent(navigationSelectInterestWAY, "Select_Interestn", parameters);
        }
        else
        {
            Debug.Log("Unable to match voice command 'Select_Interest' with current screen");
        }
    }

    private void Select_Geo(List<string> parameters)
    {
        if (currentScreen == Screens.Navigation || currentScreen == Screens.Navigation_SelectCompNav || currentScreen == Screens.Navigation_SelectGeoNav || currentScreen == Screens.Navigation_SelectPOINav || currentScreen == Screens.Navigation_SelectStationNav)
        {
            InvokeEvent(navigationSelectGeoNAV, "Select_Geo", parameters);
        }
        else if (currentScreen == Screens.Navigation_CreatingWaypoint)
        {
            InvokeEvent(navigationSelectGeoWAY, "Select_Geo", parameters);
        }
        else
        {
            Debug.Log("Unable to match voice command 'Select_Geo' with current screen");
        }
    }


    // MENU Methods
    public void Menu_Open_Tasks(List<string> parameters) => InvokeEvent(menuOpenTasks, "Menu_Open_Tasks", parameters);
    public void Menu_Open_Navigation(List<string> parameters) => InvokeEvent(menuOpenNavigation, "Menu_Open_Navigation", parameters);
    public void Menu_Open_Messaging(List<string> parameters) => InvokeEvent(menuOpenMessaging, "Menu_Open_Messaging", parameters);
    public void Menu_Open_Geo(List<string> parameters) => InvokeEvent(menuOpenGeo, "Menu_Open_Geo", parameters);
    public void Menu_Open_Vitals(List<string> parameters) => InvokeEvent(menuOpenVitals, "Menu_Open_Vitals", parameters);
    public void Menu_Open_UIA(List<string> parameters) => InvokeEvent(menuOpenUIA, "Menu_Open_UIA", parameters);
    public void Menu_Complete(List<string> parameters) => InvokeEvent(menuComplete, "Menu_Complete", parameters);
    public void Menu_Check(List<string> parameters) => InvokeEvent(menuCheck, "Menu_Check", parameters);
    public void Menu_Open_Sub_Details(List<string> parameters) => InvokeEvent(menuOpenSubDetails, "Menu_Open_Sub_Details", parameters);

    // Tasklist Methods
    public void Tasklist_Complete(List<string> parameters) => InvokeEvent(tasklistComplete, "Tasklist_Complete", parameters);
    public void Tasklist_Open_Details(List<string> parameters) => InvokeEvent(tasklistOpenDetails, "Tasklist_Open_Details", parameters);
    public void Tasklist_Close_Details(List<string> parameters) => InvokeEvent(tasklistCloseDetails, "Tasklist_Close_Details", parameters);

    // Navigation Methods
    public void Navigation_Select_Letters(List<string> parameters) => InvokeEvent(navigationSelectLetters, "Navigation_Select_Letters", parameters);
    public void Navigation_Select_Waypoint(List<string> parameters) => InvokeEvent(navigationSelectWaypoint, "Navigation_Select_Waypoint", parameters);
    public void Navigation_Add_New(List<string> parameters) => InvokeEvent(navigationAddNew, "Navigation_Add_New", parameters);
    public void Navigation_Open_3D_Map(List<string> parameters) => InvokeEvent(navigationOpen3DMap, "Navigation_Open_3D_Map", parameters);
    public void Navigation_Confirmation_Navigate(List<string> parameters) => InvokeEvent(navigationConfirmationNavigate, "Navigation_Confirmation_Navigate", parameters);
    public void Navigation_CreatingWaypoint_Enter_Name(List<string> parameters) => InvokeEvent(navigationCreatingWaypointEnterName, "Navigation_CreatingWaypoint_Enter_Name", parameters);
    public void Navigation_CreatingWaypoint_Create(List<string> parameters) => InvokeEvent(navigationCreatingWaypointCreate, "Navigation_CreatingWaypoint_Create", parameters);
    public void Navigation_Select_Companion(List<string> parameters) => InvokeEvent(navigationSelectCompanion, "Navigation_Select_Companion", parameters);
    public void Navigation_Select_Danger(List<string> parameters) => InvokeEvent(navigationSelectDanger, "Navigation_Select_Companion", parameters);

    // Messaging Methods
    public void Messaging_LLMC(List<string> parameters) => InvokeEvent(messagingLLMC, "Messaging_LLMC", parameters);
    public void Messaging_LLMC_Astro(List<string> parameters) => InvokeEvent(messagingLLMCAstro, "Messaging_LLMC_Astro", parameters);
    public void Messaging_Add_Emoji(List<string> parameters) => InvokeEvent(messagingAddEmoji, "Messaging_Add_Emoji", parameters);
    public void Messaging_Enter_Message(List<string> parameters) => InvokeEvent(messagingEnterMessage, "Messaging_Enter_Message", parameters);
    public void Messaging_Send_Message(List<string> parameters) => InvokeEvent(messagingSendMessage, "Messaging_Send_Message", parameters);
    public void Messaging_Like(List<string> parameters) => InvokeEvent(messagingLike, "Messaging_Like", parameters);
    public void Messaging_Dislike(List<string> parameters) => InvokeEvent(messagingDislike, "Messaging_Dislike", parameters);
    public void Messaging_One_Hundred(List<string> parameters) => InvokeEvent(messagingOneHundred, "Messaging_One_Hundred", parameters);
    public void Messaging_Danger(List<string> parameters) => InvokeEvent(messagingDanger, "Messaging_Danger", parameters);

    // Vitals Methods
    public void Vitals_Select_Astro(List<string> parameters) => InvokeEvent(vitalsSelectAstro, "Vitals_Select_Astro", parameters);

    private void InvokeEvent(FunctionEvent functionEvent, string functionName, List<string> parameters)
    {
        if (functionEvent.GetPersistentEventCount() > 0)
        {
            functionEvent.Invoke(parameters);
        }
        else
        {
            Debug.LogError($"No function assigned to {functionName}.");
        }
    }
}
