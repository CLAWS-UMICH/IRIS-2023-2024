using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : MonoBehaviour
{
    enum State
    {
        None,
        Tasks,
        Navigation,
        Messages,
        Samples,
        Vitals,
        Modes
    }

    [SerializeField] private State currState = State.None;

    public void ClickTasks()
    {
        if (currState != State.Tasks)
        {
            currState = State.Tasks;
            UpdateMap();
        }
    }

    public void ClickNavigation()
    {
        if (currState != State.Navigation)
        {
            currState = State.Navigation;
            UpdateMap();
        }
    }
    public void ClickMessages()
    {
        if (currState != State.Messages)
        {
            currState = State.Messages;
            UpdateMap();
        }
    }
    public void ClickSamples()
    {
        if (currState != State.Samples)
        {
            currState = State.Samples;
            UpdateMap();
        }
    }
    public void ClickVitals()
    {
        if (currState != State.Vitals)
        {
            currState = State.Vitals;
            UpdateMap();
        }
    }

    public void ClickModes()
    {
        if (currState != State.Modes)
        {
            currState = State.Modes;
            UpdateMap();
        }
    }

    private void UpdateMap()
    {
        if (currState == State.Tasks)
        {

        }
        else if (currState == State.Navigation)
        {

        }
        else if (currState == State.Messages)
        {

        }
        else if (currState == State.Samples)
        {

        }
        else if (currState == State.Vitals)
        {

        }
        else if (currState == State.Modes)
        {

        }
    }
}
