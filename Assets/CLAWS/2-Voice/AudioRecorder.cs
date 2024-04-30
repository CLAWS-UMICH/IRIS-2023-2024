using UnityEngine;
using NAudio.Wave;
using System.IO;
using System;

public class AudioRecorder : MonoBehaviour
{
    // Settings
    public string filePath = "Assets/CLAWS/Audio/recorded_audio.wav"; // Path to save the recorded WAV file
    public float silenceThreshold = 0.01f; // Threshold for detecting silence
    public float minSilenceDuration = 1.0f; // Minimum duration of silence before stopping recording

    private WaveInEvent waveIn;
    private WaveFileWriter waveWriter;
    private MemoryStream recordedStream;
    private bool isRecording = false;
    private bool isSilent = false;
    private float silenceDuration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize NAudio for audio recording
        waveIn = new WaveInEvent();
        waveIn.DataAvailable += OnDataAvailable;

        // Set wave format (16-bit PCM, 16kHz, Mono)
        waveIn.WaveFormat = new WaveFormat(16000, 16, 1);

        // Start recording
        StartRecording();
    }

    // Start recording
    void StartRecording()
    {
        isRecording = true;
        recordedStream = new MemoryStream();
        waveIn.StartRecording();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRecording)
        {
            // If currently silent, increment silence duration
            if (isSilent)
            {
                silenceDuration += Time.deltaTime;

                // Check if silence duration exceeds minimum required silence
                if (silenceDuration >= minSilenceDuration)
                {
                    //StopRecording();
                    Debug.Log("Test");
                }
            }
        }
    }

    // Stop recording and save the WAV file
    void StopRecording()
    {
        isRecording = false;
        waveIn.StopRecording();
        SaveRecording();
    }

    // Callback function for audio data availability
    void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        // Write audio data to stream if recording
        if (isRecording)
        {
            recordedStream.Write(e.Buffer, 0, e.BytesRecorded);

            // Check for silence
            float rmsAmplitude = GetRMSAmplitude(e.Buffer, e.BytesRecorded);
            Debug.Log(rmsAmplitude + " < " + silenceThreshold);
            isSilent = rmsAmplitude < silenceThreshold;

            // If not silent, reset silence duration
            if (!isSilent)
            {
                silenceDuration = 0f;
            }
        }
    }

    // Calculate RMS amplitude of audio data
    float GetRMSAmplitude(byte[] buffer, int length)
    {
        // Convert byte array to float array
        float[] samples = new float[length / 2];
        for (int i = 0; i < length / 2; i++)
        {
            short sample = (short)((buffer[i * 2 + 1] << 8) | buffer[i * 2]);
            samples[i] = sample / 32768f;
        }

        // Calculate RMS amplitude
        double sumOfSquares = 0;
        foreach (float sample in samples)
        {
            sumOfSquares += sample * sample;
        }
        double rms = Math.Sqrt(sumOfSquares / samples.Length);

        return (float)rms;
    }

    // Save recorded audio to WAV file
    void SaveRecording()
    {
        // Create a new WaveFileWriter to write audio data to the specified file path
        waveWriter = new WaveFileWriter(filePath, waveIn.WaveFormat);

        // Write audio data from stream to the WAV file
        recordedStream.Position = 0;
        recordedStream.CopyTo(waveWriter);

        // Close writer and stream
        waveWriter.Dispose();
        recordedStream.Dispose();
    }

    // Cleanup resources
    void OnDestroy()
    {
        waveIn?.Dispose();
        waveWriter?.Dispose();
        recordedStream?.Dispose();
    }
}
