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
            { Screens.Navigation, new List<string> { "apple", "banana", "orange" } },
            { Screens.NavConfirmation, new List<string> { "car", "bike", "bus" } },
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
            { "apple", (FunctionForApple, 0) },
            { "banana", (FunctionForBanana, 0) },
            { "orange", (FunctionForOrange, 3) },
            { "car", (FunctionForCar, 0) },
            { "bike", (FunctionForBike, 0) },
            { "bus", (FunctionForBus, 1) },
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

    // Example functions without parameters
    private void FunctionForApple(List<string> parameters)
    {
        Debug.Log("Function for apple called.");
    }

    private void FunctionForBanana(List<string> parameters)
    {
        Debug.Log("Function for banana called.");
    }

    private void FunctionForOrange(List<string> parameters)
    {
        Debug.Log("Function for orange called.");
    }

    private void FunctionForCar(List<string> parameters)
    {
        Debug.Log("Function for car called.");
    }

    private void FunctionForBike(List<string> parameters)
    {
        Debug.Log("Function for bike called.");
    }

    private void FunctionForBus(List<string> parameters)
    {
        Debug.Log("Function for bus called.");
    }
}
