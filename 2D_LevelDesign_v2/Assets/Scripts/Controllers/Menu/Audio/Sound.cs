using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound{

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]          //Rang Max i Min de volum
    public float volume;

    [Range(1f, 3f)]          //Rang Max i Min de pitch
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
