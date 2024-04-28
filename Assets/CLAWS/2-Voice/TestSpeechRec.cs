using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class TestSpeechRec : MonoBehaviour
{
    private DictationRecognizer dictationRecognizer;

    // Start is called before the first frame update
    void Start()
    {
        // Stop any existing phrase recognition system
        if (PhraseRecognitionSystem.Status == SpeechSystemStatus.Running)
        {
            PhraseRecognitionSystem.Shutdown();
        }

        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;

        dictationRecognizer.Start();
    }


    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log("Dictation Result: " + text);
    }

    private void DictationRecognizer_DictationHypothesis(string text)
    {
        //Debug.Log("Dictation Hypothesis: " + text);
    }

    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        //Debug.Log("Dictation Complete: " + cause.ToString());
    }

    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        //Debug.LogError("Dictation Error: " + error);
    }

    private void OnDestroy()
    {
        if (dictationRecognizer != null)
        {
            dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
            dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
            dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
            dictationRecognizer.DictationError -= DictationRecognizer_DictationError;

            dictationRecognizer.Dispose();
        }
    }
}
