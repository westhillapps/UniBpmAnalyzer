/*
UniBpmAnalyzer
Copyright (c) 2016 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UniBpmAnalyzer
{
    #region CONST

    // BPM search range
    private const int MIN_BPM = 60;
    private const int MAX_BPM = 400;
    // Base frequency (44.1kbps)
    private const int BASE_FREQUENCY = 44100;
    // Base channels (2ch)
    private const int BASE_CHANNELS = 2;
    // Base split size of sample data (case of 44.1kbps & 2ch)
    private const int BASE_SPLIT_SAMPLE_SIZE = 2205;

    #endregion

    public struct BpmMatchData
    {
        public int bpm;
        public float match;
    }

    private static BpmMatchData[] bpmMatchDatas = new BpmMatchData[MAX_BPM - MIN_BPM + 1];

    /// <summary>
    /// Analyze BPM from an audio clip
    /// </summary>
    /// <param name="clip">target audio clip</param>
    /// <returns>bpm</returns>
    public static int AnalyzeBpm(AudioClip clip)
    {
        for (int i = 0; i < bpmMatchDatas.Length; i++)
        {
            bpmMatchDatas[i].match = 0f;
        }
        if (clip == null)
        {
            return -1;
        }
        Debug.Log("AnalyzeBpm audioClipName : " + clip.name);

        int frequency = clip.frequency;
        Debug.Log("Frequency : " + frequency);

        int channels = clip.channels;
        Debug.Log("Channels : " + channels);

        int splitFrameSize = Mathf.FloorToInt(((float)frequency / (float)BASE_FREQUENCY) * ((float)channels / (float)BASE_CHANNELS) * (float)BASE_SPLIT_SAMPLE_SIZE);

        // Get all sample data from audioclip
        var allSamples = new float[clip.samples * channels];
        clip.GetData(allSamples, 0);

        // Create volume array from all sample data
        var volumeArr = CreateVolumeArray(allSamples, frequency, channels, splitFrameSize);

        // Search bpm from volume array
        int bpm = SearchBpm(volumeArr, frequency, splitFrameSize);
        Debug.Log("Matched BPM : " + bpm);

        var strBuilder = new StringBuilder("BPM Match Data List\n");
        for (int i = 0; i < bpmMatchDatas.Length; i++)
        {
            strBuilder.Append("bpm : " + bpmMatchDatas[i].bpm + ", match : " + Mathf.FloorToInt(bpmMatchDatas[i].match * 10000f) + "\n");
        }
        Debug.Log(strBuilder.ToString());

        return bpm;
    }

    /// <summary>
    /// Create volume array from all sample data
    /// </summary>
    private static float[] CreateVolumeArray(float[] allSamples, int frequency, int channels, int splitFrameSize)
    {
        // Initialize volume array
        var volumeArr = new float[Mathf.CeilToInt((float)allSamples.Length / (float)splitFrameSize)];
        int powerIndex = 0;

        // Sample data analysis start
        for (int sampleIndex = 0; sampleIndex < allSamples.Length; sampleIndex += splitFrameSize)
        {
            float sum = 0f;
            for (int frameIndex = sampleIndex; frameIndex < sampleIndex + splitFrameSize; frameIndex++)
            {
                if (allSamples.Length <= frameIndex)
                {
                    break;
                }
                // Use the absolute value, because left and right value is -1 to 1
                float absValue = Mathf.Abs(allSamples[frameIndex]);
                if (absValue > 1f)
                {
                    continue;
                }

                // Calculate the amplitude square sum
                sum += (absValue * absValue);
            }

            // Set volume value
            volumeArr[powerIndex] = Mathf.Sqrt(sum / splitFrameSize);
            powerIndex++;
        }

        // Representing a volume value from 0 to 1
        float maxVolume = volumeArr.Max();
        for (int i = 0; i < volumeArr.Length; i++)
        {
            volumeArr[i] = volumeArr[i] / maxVolume;
        }

        return volumeArr;
    }

    /// <summary>
    /// Search bpm from volume array
    /// </summary>
    private static int SearchBpm(float[] volumeArr, int frequency, int splitFrameSize)
    {
        // Create volume diff list
        var diffList = new List<float>();
        for (int i = 1; i < volumeArr.Length; i++)
        {
            diffList.Add(Mathf.Max(volumeArr[i] - volumeArr[i - 1], 0f));
        }

        // Calculate the degree of coincidence in each BPM
        int index = 0;
        float splitFrequency = (float)frequency / (float)splitFrameSize;
        for (int bpm = MIN_BPM; bpm <= MAX_BPM; bpm++)
        {
            float sinMatch = 0f;
            float cosMatch = 0f;
            float bps = (float)bpm / 60f;

            if (diffList.Count > 0)
            {
                for (int i = 0; i < diffList.Count; i++)
                {
                    sinMatch += (diffList[i] * Mathf.Cos(i * 2f * Mathf.PI * bps / splitFrequency));
                    cosMatch += (diffList[i] * Mathf.Sin(i * 2f * Mathf.PI * bps / splitFrequency));
                }

                sinMatch *= (1f / (float)diffList.Count);
                cosMatch *= (1f / (float)diffList.Count);
            }

            float match = Mathf.Sqrt((sinMatch * sinMatch) + (cosMatch * cosMatch));

            bpmMatchDatas[index].bpm = bpm;
            bpmMatchDatas[index].match = match;
            index++;
        }

        // Returns a high degree of coincidence BPM
        int matchIndex = Array.FindIndex(bpmMatchDatas, x => x.match == bpmMatchDatas.Max(y => y.match));

        return bpmMatchDatas[matchIndex].bpm;
    }
}
