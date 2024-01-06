using UnityEngine;
using System.Collections.Generic;
using System;

public class Piano : MonoBehaviour
{
    public List<AudioClip> clips; // Assign this in the Unity Editor
    Dictionary<string, (string, float)> keyMappings;
    Dictionary<string, AudioSource> audioSources;
    float octaveShift = 1.0f;

    void Start()
    {
        // Initialize the dictionary with key mappings
        keyMappings = new Dictionary<string, (string, float)>
        {
            {"a", ("C", 1.0f)},
            {"s", ("D", 1.12246f)},
            {"d", ("E", 1.25992f)},
            {"f", ("F", 1.33484f)},
            {"g", ("G", 1.49831f)},
            {"h", ("A", 1.68179f)},
            {"j", ("B", 1.88775f)},
            // Add more mappings as needed
        };

        // Initialize the audio sources
        audioSources = new Dictionary<string, AudioSource>();
        foreach (var mapping in keyMappings)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clips[0];
            source.pitch = mapping.Value.Item2;
            source.playOnAwake = false;
            source.loop = true;
            audioSources[mapping.Key] = source;
        }
    }

    void Update()
    {
        foreach (var mapping in keyMappings)
        {
            HandleKeyPress(mapping.Key);
        }

        // Check for octave changes
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeOctave(2.0f); // Increase octave
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeOctave(0.5f); // Decrease octave
        }
    }

    void HandleKeyPress(string key)
    {
        if (Input.GetKeyDown(key) && !audioSources[key].isPlaying)
        {
            audioSources[key].Play();
        }
        else if (Input.GetKeyUp(key))
        {
            audioSources[key].Stop();
        }
    }

    void ChangeOctave(float change)
    {       
        octaveShift *= change;
        foreach (var mapping in keyMappings)
        {
            audioSources[mapping.Key].pitch = mapping.Value.Item2 * octaveShift;
        }
    }

    public void HandleDropdown(int val)
    {
        foreach (var mapping in keyMappings)
        {
            audioSources[mapping.Key].clip = clips[val % clips.Count];
        }
    }

    public void ChangePitchCorrection(float correction)
    {
        try
        {
            foreach (var mapping in keyMappings)
            {
                audioSources[mapping.Key].pitch = mapping.Value.Item2 * correction;
            }
        }
        catch (NullReferenceException) { }
    }
}