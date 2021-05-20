using UnityEngine.Audio;
using System;
using UnityEngine;

[System.Serializable]
public class Sounds //Sounds class that holds varible inforamtion for sound clips. Used by audio manager for creating new sound clips.
{
    public string name;
    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(.5f,3.5f)]
    public float pitch;
    [HideInInspector]
    public AudioSource source;

    public bool loop;
}
