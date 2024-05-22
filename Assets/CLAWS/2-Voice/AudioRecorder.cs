using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NAudio.Wave;
using System;
using System.IO;
using TMPro;

public class AudioRecorder : MonoBehaviour
{
    bool isRecording = false;
    WaveInEvent waveIn;
    WaveFileWriter writer;

    // List to store recent audio samples
    List<float> recentSamples = new List<float>();

    // Number of recent samples to consider for the moving average
    [SerializeField] int numRecentSamples = 100;

    // Threshold for silence detection (percentage of the average amplitude)
    [SerializeField] float silenceThreshold = 0.005f;

    // Duration of silence required to stop recording (in seconds)
    [SerializeField] float silenceDuration = 3f;
    [SerializeField] float durationLeft = 3f;

    bool useModel;


    private string destination = "Assets/CLAWS/Audio/recorded_audio.wav";

    WebsocketDataHandler wdh;

    VEGAScreenHandler s;

    HardCodedVoiceCommandsHandler h;


    void Start()
    {
        wdh = GameObject.Find("Controller").transform.GetComponent<WebsocketDataHandler>();
        s = transform.GetComponent<VEGAScreenHandler>();
        h = GameObject.Find("HardcodedVoiceManager").GetComponent<HardCodedVoiceCommandsHandler>();
        InitializeRecorder();
    }

    private void InitializeRecorder()
    {
        durationLeft = silenceDuration;
        waveIn = new WaveInEvent();
        waveIn.WaveFormat = new WaveFormat(16000, 16, 1);
        waveIn.DataAvailable += WaveInDataAvailable;
    }

    void WaveInDataAvailable(object sender, WaveInEventArgs e)
    {
        if (isRecording)
        {
            writer.Write(e.Buffer, 0, e.BytesRecorded);
            // Convert bytes to floats for amplitude calculation
            float[] buffer = new float[e.BytesRecorded / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = BitConverter.ToInt16(e.Buffer, i * 2) / 32768f;
            }
            // Add recent samples to the list
            recentSamples.AddRange(buffer);
            // Keep only the most recent samples
            if (recentSamples.Count > numRecentSamples)
            {
                recentSamples.RemoveRange(0, recentSamples.Count - numRecentSamples);
            }
        }
    }

    public void VEGARecord()
    {
        useModel = true;
        StartRecording();
    }

    public void TranscribeRecord()
    {
        useModel = false;
        StartRecording();
    }

    public void StartRecording()
    {
        if (!isRecording)
        {
            h.TurnOffAllScreens();
            s.onVoice();
            InitializeRecorder();
            Debug.Log("Started Recording...");
            isRecording = true;
            waveIn.StartRecording();
            // Create a new WAV file for recording
            writer = new WaveFileWriter(destination, waveIn.WaveFormat);

            

        }
    }

    public void StopRecording()
    {
        if (isRecording)
        {
            s.onStartVegaCommand();
            Debug.Log("Stopping Recording");
            isRecording = false;
            waveIn.StopRecording();
            writer.Close();
            writer.Dispose();

            // Send recorded audio over websocket
            SendAudioOverWebsocket();

            h.SwitchScreen(StateMachine.CurrScreen);
        }
    }

    void SendAudioOverWebsocket()
    {
        if (File.Exists(destination))
        {
            // Read the WAV file as bytes
            byte[] audioBytes = File.ReadAllBytes(destination);

            // Encode the audio bytes as Base64 string
            string base64Audio = Convert.ToBase64String(audioBytes);

            VegaAudio va = new VegaAudio(base64Audio, "", false);

            if (useModel)
            {
                va.classify = true;
            }

            wdh.SendAudio(va);

        }
    }

    void Update()
    {
        if (isRecording && IsSilenceDetected())
        {
            StopRecording();
        }
    }

    bool IsSilenceDetected()
    {
        // Calculate the average amplitude of recent samples
        float sum = 0f;
        foreach (float sample in recentSamples)
        {
            sum += Mathf.Abs(sample);
        }
        float averageAmplitude = sum / recentSamples.Count;

        //Debug.Log(averageAmplitude);

        // Check if the amplitude falls below the threshold for the duration
        if (averageAmplitude < silenceThreshold)
        {
            durationLeft -= Time.deltaTime;
        }
        else
        {
            // Reset silence duration if amplitude is above the threshold
            durationLeft = silenceDuration;
        }

        // If silence duration is met, return true
        return durationLeft <= 0f;
    }
}
