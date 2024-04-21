using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if !UNITY_WEBGL
using UnityEngine.Windows.Speech;
using System.Linq;

public class VoiceMessage : MonoBehaviour
{
    private TextMeshProUGUI message_TMP;
    private TextMeshProUGUI ph_TMP;
    private DictationRecognizer dictationRecognizer;
    private KeywordRecognizer keywordRecognizer;
    private bool dictationActive = false;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Started the VM script");

        message_TMP = transform.Find("Message_TMP").GetComponent<TextMeshProUGUI>();
        ph_TMP = transform.Find("Placeholder").GetComponent<TextMeshProUGUI>();
        keywordRecognizer = new KeywordRecognizer(new string[] { "Start", "Stop" });
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;

        Debug.Log("Made it to the end of the VM script");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string keyword = args.text;
        if (keyword == "Start")
        {
            if (!dictationActive)
            {
                dictationActive = true;
                dictationRecognizer.Start();
                Debug.Log("Dictation started...");
            }
        }
        else if (keyword == "Stop")
        {
            if (dictationActive)
            {
                dictationActive = false;
                dictationRecognizer.Stop();
                Debug.Log("Dictation stopped...");

            }
        }
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        ph_TMP.gameObject.SetActive(false);
        Debug.Log("Dictation result: " + text);
        message_TMP.text = text;
    }

}

#endif