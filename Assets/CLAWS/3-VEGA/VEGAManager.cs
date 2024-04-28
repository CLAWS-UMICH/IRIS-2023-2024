using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VEGAManager : MonoBehaviour
{
    Subscription<VEGASpeechToTextCommand> vegaCommandEvent;
    // Start is called before the first frame update
    void Start()
    {
        vegaCommandEvent = EventBus.Subscribe<VEGASpeechToTextCommand>(onNewVEGACommand);
    }

    private void onNewVEGACommand(VEGASpeechToTextCommand c)
    {
        if (c.userID == 0)
        {
            if (c.isTheFinal == false)
            {
                Debug.Log("Current VEGA Command: " + c.command);
            }
            else
            {
                Debug.Log("Final VEGA Command: " + c.command);
            }
        }
    }

    public void StartVEGA()
    {
        SpeechRecognitionManager.SwitchToDictation(0);
    }

    public void EndVEGA()
    {
        SpeechRecognitionManager.SwitchToPhraseRecognition();
    }
}
