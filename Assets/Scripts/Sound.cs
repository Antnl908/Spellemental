using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public class Sound 
{
    public string name;
    public AudioClip clip;
    [Range(0f, 10f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop;

    [HideInInspector]
    public AudioSource audioSource;
}
