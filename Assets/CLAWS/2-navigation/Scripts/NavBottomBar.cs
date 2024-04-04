using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavBottomBar : MonoBehaviour
{
    GameObject backplate;
    GameObject button;

    // Start is called before the first frame update
    void Start()
    {
        backplate = transform.Find("BackPlate").gameObject;
        button = transform.Find("CancelRouteButton").gameObject;

        HideNavBar();

        EventBus.Subscribe<ModeChangedEvent>(ModeChanged);
    }

    private void ModeChanged(ModeChangedEvent e)
    {
        if (e.Mode == Modes.Navigation)
        {
            ShowNavBar();
        } else
        {
            HideNavBar();
        }
    }

    private void ShowNavBar()
    {
        backplate.SetActive(true);
        button.SetActive(true);
    }

    private void HideNavBar()
    {
        backplate.SetActive(false);
        button.SetActive(false);
    }

    public void OnClick()
    {
        EventBus.Publish(new ModeChangedEvent(Modes.Normal));
    }


}
