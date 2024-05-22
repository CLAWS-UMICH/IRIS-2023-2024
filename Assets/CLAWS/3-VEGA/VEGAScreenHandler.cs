using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VEGAScreenHandler : MonoBehaviour
{
    Subscription<SpeechToText> vegaCommandEvent;
    GameObject screen;
    GameObject microphone;
    TextMeshPro text;
    bool screenOpened;
    // Start is called before the first frame update
    void Start()
    {
        vegaCommandEvent = EventBus.Subscribe<SpeechToText>(onNewVEGACommand);
        screen = transform.Find("Screen").gameObject;
        microphone = transform.Find("Microphone").gameObject;
        text = transform.Find("Screen").Find("text").transform.GetComponent<TextMeshPro>();
        text.text = "";
        screen.SetActive(false);
        screenOpened = false;
        microphone.SetActive(false);
    }

    public void onVoice()
    {
        microphone.SetActive(true);
    }

    public void onStartVegaCommand()
    {
        microphone.SetActive(false);
        screenOpened = true;
        screen.SetActive(true);
        StartCoroutine(CloseScreenWithDelay(10));
    }

    private void onNewVEGACommand(SpeechToText c)
    {
        if (screenOpened)
        {
            text.text = c.text;
        }
    }

    private IEnumerator CloseScreenWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        screen.SetActive(false);
        screenOpened = false;
    }
}
