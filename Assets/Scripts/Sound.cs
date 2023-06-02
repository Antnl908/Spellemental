using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Made following Brackeys tutorial
/// https://www.youtube.com/watch?v=6OT43pvUyfY
/// Handles audioclip and relevent values
/// </summary>
[System.Serializable]
public class Sound 
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop;

    [HideInInspector]
    public AudioSource audioSource;
}
