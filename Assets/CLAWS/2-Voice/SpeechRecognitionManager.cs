using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechRecognitionManager : MonoBehaviour
{
    private static DictationRecognizer dictationRecognizer;
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
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;

        // Deactivate dictation and enable phrase systems initially
        DeactivateDictation();
        ActivatePhraseRecognition();
    }

    // Activate and Deactivate methods for Dictation
    public static void ActivateDictation()
    {
        result = "";
        dictationOn = true;
        dictationRecognizer.Start();
        // Start or restart the timer coroutine
        if (dictationTimerCoroutine != null)
            instance.StopCoroutine(dictationTimerCoroutine);
        dictationTimerCoroutine = instance.StartCoroutine(DictationTimer());
    }

    public static void DeactivateDictation()
    {
        dictationUser = -1;
        dictationOn = false;
        dictationRecognizer.Stop();
        // Stop the timer coroutine
        if (dictationTimerCoroutine != null)
            instance.StopCoroutine(dictationTimerCoroutine);
    }

    // Activate and Deactivate methods for Phrase Recognition
    public static void ActivatePhraseRecognition()
    {
        if (PhraseRecognitionSystem.Status != SpeechSystemStatus.Running)
        {
            PhraseRecognitionSystem.Restart();
        }

        phraseOn = true;
    }

    public static void DeactivatePhraseRecognition()
    {
        if (PhraseRecognitionSystem.Status == SpeechSystemStatus.Running)
        {
            PhraseRecognitionSystem.Shutdown();
        }

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

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        //Debug.Log("Dictation Result: " + text);
        result += " " + text;
        EventBus.Publish(new VEGASpeechToTextCommand(result, false, dictationUser));
    }

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

    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        //Debug.Log("Dictation Complete: " + cause.ToString());
    }

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
