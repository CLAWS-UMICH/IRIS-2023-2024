using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VEGAManager : MonoBehaviour
{
    Subscription<SpeechToText> vegaCommandEvent;
    GameObject screen;
    TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        vegaCommandEvent = EventBus.Subscribe<SpeechToText>(onNewVEGACommand);
        screen = transform.Find("Screen").gameObject;
        text = transform.Find("Screen").Find("text").transform.GetComponent<TextMeshPro>();
        text.text = "";
        screen.SetActive(false);
    }

    private void onNewVEGACommand(SpeechToText c)
    {
        text.text = c.text;
        StartCoroutine(CloseScreenWithDelay(2));
    }

    private IEnumerator CloseScreenWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        screen.SetActive(false);
    }

    /*public void StartVEGA()
    {
        SpeechRecognitionManager.SwitchToDictation(0);
        text.text = "";
        screen.SetActive(true);
    }

    public void EndVEGA()
    {
        SpeechRecognitionManager.SwitchToPhraseRecognition();
        text.text = "";
    }*/
}
