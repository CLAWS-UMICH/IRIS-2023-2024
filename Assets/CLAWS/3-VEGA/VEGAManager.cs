using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VEGAManager : MonoBehaviour
{
    Subscription<VEGASpeechToTextCommand> vegaCommandEvent;
    GameObject screen;
    TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        vegaCommandEvent = EventBus.Subscribe<VEGASpeechToTextCommand>(onNewVEGACommand);
        screen = transform.Find("Screen").gameObject;
        text = transform.Find("Screen").Find("text").transform.GetComponent<TextMeshPro>();
        text.text = "";
        screen.SetActive(false);
    }

    private void onNewVEGACommand(VEGASpeechToTextCommand c)
    {
        if (c.userID == 0)
        {
            if (c.isTheFinal == false)
            {
                //Debug.Log("Current VEGA Command: " + c.command);
                text.text = c.command;
            }
            else
            {
                //Debug.Log("Final VEGA Command: " + c.command);
                text.text = c.command;
                StartCoroutine(CloseScreenWithDelay());
            }
        }
    }

    private IEnumerator CloseScreenWithDelay()
    {
        yield return new WaitForSeconds(1f);
        screen.SetActive(false);
    }

    public void StartVEGA()
    {
        SpeechRecognitionManager.SwitchToDictation(0);
        text.text = "";
        screen.SetActive(true);
    }

    public void EndVEGA()
    {
        SpeechRecognitionManager.SwitchToPhraseRecognition();
        text.text = "";
    }
}
