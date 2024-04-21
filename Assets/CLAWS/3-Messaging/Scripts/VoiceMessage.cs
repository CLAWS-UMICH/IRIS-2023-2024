using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if !UNITY_WEBGL
using UnityEngine.Windows.Speech;
using System.Linq;

public class VoiceMessage : MonoBehaviour
{
    private TextMeshPro message_TMP;
    private DictationRecognizer dictationRecognizer;
    private KeywordRecognizer keywordRecognizer;
    private bool dictationActive = false;

    // Start is called before the first frame update
    void Start()
    {
        message_TMP = transform.Find("Placeholder").GetComponent<TextMeshPro>();
        keywordRecognizer = new KeywordRecognizer(new string[] { "Vega", "Vega Stop" });
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.Start();
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

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string keyword = args.text;
        if (keyword == "Vega")
        {
            if (!dictationActive)
            {
                dictationActive = true;
                dictationRecognizer.Start();
                Debug.Log("Dictation started...");
            }
        }
        else if (keyword == "Vega Stop")
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
        Debug.Log("Dictation result: " + text);
        message_TMP.text = text;
    }

    private void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }

        if (dictationRecognizer != null)
        {
            dictationRecognizer.Stop();
            dictationRecognizer.Dispose();
        }
    }
}

#endif