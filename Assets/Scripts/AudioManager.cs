using UnityEngine.Audio;
using UnityEngine;
using System;

/// <summary>
/// Made following Brackeys tutorial
/// https://www.youtube.com/watch?v=6OT43pvUyfY
/// Handles and plays audioclips
/// </summary>
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    /// <summary>
    /// Play audioclip from Sound class
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning($"Sound: {name} :could not be found.");
            return;
        }
        s.audioSource.Play();
    }

    private void Start()
    {
        Play("Theme");
    }
}
