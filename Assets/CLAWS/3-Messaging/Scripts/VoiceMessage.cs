using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using TMPro;

public class VoiceMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text message;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RecordMessage()
    {
        DictationRecognizer dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;

    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        message.SetText(message + " " + text);
    }
}
