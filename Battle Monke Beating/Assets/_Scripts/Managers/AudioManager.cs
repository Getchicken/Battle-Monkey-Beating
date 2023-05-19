using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public string currentClipName;
    public static AudioManager Instance;
    [SerializeField] private float musicVolume;
    [SerializeField] private float musicPitch;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("Theme", musicVolume, musicPitch);
    }

    public void Play(string name, float randomVolume, float randomPitch)
    {
        // check if there are sounds in the array to avoid bugs
        if (sounds == null)
        {
            Debug.LogWarning("Sounds array is null.");
            return;
        }

        Sound s = Array.Find(sounds, sounds => sounds.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }

        s.source.volume = randomVolume;
        s.source.pitch = randomPitch;
        s.source.Play();
        currentClipName = name;
    }

    public void Stop(string name)
    {
        // check if their are sounds in the array to avoid bugs
        if (sounds == null)
        {
            Debug.LogWarning("Sounds array is null.");
            return;
        }

        Sound s = Array.Find(sounds, sounds => sounds.name == name);

        if (s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }

        s.source.Stop();
    }

    // This function is called when the pitch slider value changes
    public void OnPitchSliderChanged(float sliderVolumeValue)
    {
        // Loop through all the audio clips in the array and set their pitch to the new value
        foreach (Sound s in sounds)
        {
            s.volume = sliderVolumeValue;
            s.source.volume = s.volume;
        }
    }
}
