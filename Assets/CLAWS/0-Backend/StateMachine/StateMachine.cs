using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add new Screens to the Enum
[System.Serializable]
public enum Screens
{
    Menu,

    Tasklist,
    Tasklist_SubOpen,
    Tasklist_Emergency,

    Navigation,
    Navigation_SelectStationNav,
    Navigation_SelectPOINav,
    Navigation_SelectGeoNav,
    Navigation_SelectCompNav,
    Navigation_Confirmation,
    Navigation_CreatingWaypoint,
    Navigation_3D,

    Messaging_Astro_BlankMessage,
    Messaging_Astro_Quick,
    Messaging_Astro_FullMessage,
    Messaging_LLMC_BlankMessage,
    Messaging_LLMC_Quick,
    Messaging_LLMC_FullMessage,
    Messaging_GroupChat_BlankMessage,
    Messaging_GroupChat_Quick,
    Messaging_GroupChat_FullMessage,

    Geo,
    Geo_Database,

    Vitals_Main,
    Vitals_Fellow,

    Screen_Sent,

}

public enum Modes
{
    Normal,
    Sampling,
    Navigation,
    Egress,
}

public class StateMachine : MonoBehaviour
{

    public static Screens CurrScreen = Screens.Menu;
    public static Modes CurrMode = Modes.Normal;
    private Subscription<ScreenChangedEvent> screenChangedSubscription; // Store the subscription
    private Subscription<ModeChangedEvent> modeChangedSubscription;

    private void Start()
    {
        screenChangedSubscription = EventBus.Subscribe<ScreenChangedEvent>(SwitchScreen);
        modeChangedSubscription = EventBus.Subscribe<ModeChangedEvent>(SwitchMode);
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

    public void SwitchMode(ModeChangedEvent e)
    {
        Debug.Log(CurrMode.ToString() + " -> " + e.Mode.ToString());
        CurrMode = e.Mode;
    }

    [ContextMenu("CloseScreen")]
    public void CloseScreen()
    {
        EventBus.Publish(new CloseEvent(CurrScreen));
    }

    [ContextMenu("CloseAll")]
    public void CloseAll()
    {
        /*EventBus.Publish(new CloseEvent(Screens.CreatingWaypoint));
        EventBus.Publish(new CloseEvent(Screens.Vitals_1));
        EventBus.Publish(new CloseEvent(Screens.Vitals_2));
        EventBus.Publish(new CloseEvent(Screens.Map_3D));
        EventBus.Publish(new CloseEvent(Screens.Map_2D));
        EventBus.Publish(new CloseEvent(Screens.Navigation));
        EventBus.Publish(new CloseEvent(Screens.Tasklist));
        CurrMode = Modes.Normal;
        CurrScreen = Screens.Menu;*/
    }
}

