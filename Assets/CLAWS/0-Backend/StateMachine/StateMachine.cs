using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add new Screens to the Enum
[System.Serializable]
public enum Screens
{
    Menu,
    SelectStationWaypoint,
    SelectNavWaypoint,
    SelectGeoWaypoint,
    CreatingWaypoint,
    Vitals,
    Map_3D,
    Map_2D,
    Navigation,
    Tasklist,
}

public class StateMachine : MonoBehaviour
{

    public Screens CurrScreen = Screens.Menu;
    private Subscription<ScreenChangedEvent> screenChangedSubscription; // Store the subscription

    private void Start()
    {
        screenChangedSubscription = EventBus.Subscribe<ScreenChangedEvent>(SwitchScreen);
    }

    private void OnDestroy()
    {
        // Unsubscribe when the script is destroyed
        if (screenChangedSubscription != null)
        {
            EventBus.Unsubscribe(screenChangedSubscription);
        }
    }

    public void SwitchScreen(ScreenChangedEvent e)
    {
        Debug.Log(CurrScreen.ToString() + " -> " + e.Screen.ToString());
        CurrScreen = e.Screen;
    }

    [ContextMenu("CloseScreen")]
    public void CloseScreen()
    {
        EventBus.Publish(new CloseEvent(CurrScreen));
    }

    [ContextMenu("CloseAll")]
    public void CloseAll()
    {
        EventBus.Publish(new CloseEvent(Screens.CreatingWaypoint));
        EventBus.Publish(new CloseEvent(Screens.Vitals));
        EventBus.Publish(new CloseEvent(Screens.Map_3D));
        EventBus.Publish(new CloseEvent(Screens.Map_2D));
        EventBus.Publish(new CloseEvent(Screens.Navigation));
        EventBus.Publish(new CloseEvent(Screens.Tasklist));
        CurrScreen = Screens.Menu;
    }
}

