using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_WEBGL
    using UnityEngine.Windows.Speech; // 
#endif

public class TestSpeechRec : MonoBehaviour
{
    #if !UNITY_WEBGL
        private DictationRecognizer dictationRecognizer; //
    #endif
    // Start is called before the first frame update
    void Start()
    {
    #if !UNITY_WEBGL
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
    #endif
    }

    #if !UNITY_WEBGL
        private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence) //
        {
            Debug.Log("Dictation Result: " + text);
        }
    #endif
    private void DictationRecognizer_DictationHypothesis(string text)
    {
        //Debug.Log("Dictation Hypothesis: " + text);
    }
    #if !UNITY_WEBGL
        private void DictationRecognizer_DictationComplete(DictationCompletionCause cause) //
        {
            //Debug.Log("Dictation Complete: " + cause.ToString());
        }
    #endif
    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        //Debug.LogError("Dictation Error: " + error);
    }

    private void OnDestroy()
    {
        #if !UNITY_WEBGL
            if (dictationRecognizer != null)
            {
                dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
                dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
                dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
                dictationRecognizer.DictationError -= DictationRecognizer_DictationError;

                dictationRecognizer.Dispose();
            }
        #endif
    }
}
