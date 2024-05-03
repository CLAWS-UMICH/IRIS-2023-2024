using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_WEBGL
    using UnityEngine.Windows.Speech; // 
#endif

public class SpeechRecognitionManager : MonoBehaviour
{
    #if !UNITY_WEBGL
        private static DictationRecognizer dictationRecognizer; //
    #endif
    private static bool dictationOn;
    private static bool phraseOn;
    private static int dictationUser;

    private static Coroutine dictationTimerCoroutine;
    private static SpeechRecognitionManager instance;
    private static string result;

    void Awake()
    {
        instance = this;
        dictationUser = -1;

        // Initialize speech recognition systems
        #if !UNITY_WEBGL
            dictationRecognizer = new DictationRecognizer();
            dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
            dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
            dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
            dictationRecognizer.DictationError += DictationRecognizer_DictationError;
        #endif

        // Deactivate dictation and enable phrase systems initially
        DeactivateDictation();
        ActivatePhraseRecognition();
    }

    // Activate and Deactivate methods for Dictation
    public static void ActivateDictation()
    {
        result = "";
        dictationOn = true;
        #if !UNITY_WEBGL
            dictationRecognizer.Start();
        #endif
        // Start or restart the timer coroutine
        if (dictationTimerCoroutine != null)
            instance.StopCoroutine(dictationTimerCoroutine);
        dictationTimerCoroutine = instance.StartCoroutine(DictationTimer());
    }

    public static void DeactivateDictation()
    {
        dictationUser = -1;
        dictationOn = false;
        #if !UNITY_WEBGL
            dictationRecognizer.Stop();
        #endif
        // Stop the timer coroutine
        if (dictationTimerCoroutine != null)
            instance.StopCoroutine(dictationTimerCoroutine);
    }

    // Activate and Deactivate methods for Phrase Recognition
    public static void ActivatePhraseRecognition()
    {
        #if !UNITY_WEBGL
            if (PhraseRecognitionSystem.Status != SpeechSystemStatus.Running)
            {
                PhraseRecognitionSystem.Restart();
            }
        #endif

        phraseOn = true;
    }

    public static void DeactivatePhraseRecognition()
    {
        #if !UNITY_WEBGL
            if (PhraseRecognitionSystem.Status == SpeechSystemStatus.Running)
            {
                PhraseRecognitionSystem.Shutdown();
            }
        #endif
        phraseOn = false;
    }

    // Switching between Dictation and Phrase Recognition
    public static void SwitchToDictation(int userID)
    {
        dictationUser = userID;
        DeactivatePhraseRecognition();
        ActivateDictation();
    }

    public static void SwitchToPhraseRecognition()
    {
        DeactivateDictation();
        instance.StartCoroutine(RestartPhraseRecognitionAfterDelay());
        //ActivatePhraseRecognition();
    }

    #if !UNITY_WEBGL
        private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence) //
        {
            //Debug.Log("Dictation Result: " + text);
            result += " " + text;
            EventBus.Publish(new VEGASpeechToTextCommand(result, false, dictationUser));
        }
    #endif

    private void DictationRecognizer_DictationHypothesis(string text)
    {
        // Reset the timer if hypothesis is received
        if (dictationOn)
        {
            if (dictationTimerCoroutine != null)
                instance.StopCoroutine(dictationTimerCoroutine);
            dictationTimerCoroutine = instance.StartCoroutine(DictationTimer());
        }
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
        Debug.LogError("Dictation Error: " + error);
    }

    // Coroutine to handle the dictation timer
    private static IEnumerator DictationTimer()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        // If dictation is still on after 3 seconds, deactivate it
        if (dictationOn)
        {
            DictationIsDone();
        }
    }

    private static void DictationIsDone()
    {
        Debug.Log(result);
        EventBus.Publish(new VEGASpeechToTextCommand(result, true, dictationUser));

        SwitchToPhraseRecognition();
    }

    // Coroutine to restart Phrase Recognition after a short delay
    private static IEnumerator RestartPhraseRecognitionAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        ActivatePhraseRecognition();
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
