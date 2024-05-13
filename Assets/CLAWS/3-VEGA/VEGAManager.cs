using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VEGAManager : MonoBehaviour
{
    Subscription<VEGACommand> vegaCommandEvent;

    private Dictionary<Screens, List<string>> screenAssociatedCommands;
    private Dictionary<Modes, List<string>> modeAssociatedCommands;
    private Dictionary<string, (Action<List<string>>, int)> functionDictionary;

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

            { Screens.Navigation, new List<string> { "Select_Companions", "Select_Stations", "Select_Interest", "Select_Geo", "Select_Letter", "Select_Waypoint", "Add_New", "Open_3D_Map", "Close" } },
            { Screens.Navigation_SelectStationNav, new List<string> { "Select_Companions", "Select_Stations", "Select_Interest", "Select_Geo", "Select_Letter", "Select_Waypoint", "Add_New", "Open_3D_Map", "Close" } },
            { Screens.Navigation_SelectPOINav, new List<string> { "Select_Companions", "Select_Stations", "Select_Interest", "Select_Geo", "Select_Letter", "Select_Waypoint", "Add_New", "Open_3D_Map", "Close" } },
            { Screens.Navigation_SelectGeoNav, new List<string> { "Select_Companions", "Select_Stations", "Select_Interest", "Select_Geo", "Select_Letter", "Select_Waypoint", "Add_New", "Open_3D_Map", "Close" } },
            { Screens.Navigation_SelectCompNav, new List<string> { "Select_Companions", "Select_Stations", "Select_Interest", "Select_Geo", "Select_Letter", "Select_Waypoint", "Add_New", "Open_3D_Map", "Close" } },
            { Screens.Navigation_Confirmation, new List<string> { "Navigate", "Cancel", "Close", "Scroll_Up", "Close", "Close_Details" } },
            { Screens.Navigation_CreatingWaypoint, new List<string> { "Enter_Name", "Select_Station", "Select_Interest", "Select_Danger", "Select_Sample", "Create", "Cancel", "Close" } },
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
            { "Open_Tasks", (Menu_Open_Tasks, 0) },
            { "Open_Navigation", (Menu_Open_Navigation, 0) },
            { "Open_Messaging", (Menu_Open_Messaging, 0) },
            { "Open_Geo", (Menu_Open_Geo, 0) },
            { "Open_Vitals", (Menu_Open_Vitals, 0) },
            { "Open_UIA", (Menu_Open_UIA, 0) },
            { "Complete", (Menu_Complete, 0) },
            { "Check", (Menu_Check, 0) },
            { "Open_Sub_Details", (Menu_Open_Sub_Details, 1) },
        };
    }

    private void onVEGACommand(VEGACommand c)
    {
        Screens currentScreen = StateMachine.CurrScreen;
        Modes currentMode = StateMachine.CurrMode;

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

    // Menu Functions
    private void Menu_Open_Tasks(List<string> parameters)
    {
        // TODO: Call Function
    }

    private void Menu_Open_Navigation(List<string> parameters)
    {
        // TODO: Call Function
    }

    private void Menu_Open_Messaging(List<string> parameters)
    {
        // TODO: Call Function
    }

    private void Menu_Open_Geo(List<string> parameters)
    {
        // TODO: Call Function
    }

    private void Menu_Open_Vitals(List<string> parameters)
    {
        // TODO: Call Function
    }

    private void Menu_Open_UIA(List<string> parameters)
    {
        // TODO: Call Function
    }
    private void Menu_Complete(List<string> parameters)
    {
        // TODO: Call Function
    }
    private void Menu_Check(List<string> parameters)
    {
        // TODO: Call Function
    }
    private void Menu_Open_Sub_Details(List<string> parameters)
    {
       // TODO: Call Function
    }
}
