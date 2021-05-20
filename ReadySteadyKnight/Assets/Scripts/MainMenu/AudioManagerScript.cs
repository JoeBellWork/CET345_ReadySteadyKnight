using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public Sounds[] sounds; //used to hold sound effects
    public static AudioManagerScript instance; //varible used to track single instance of object holding the script
    public static AudioManagerScript getInstance()
    {
        return instance;
    }

    // awake function checks for instance of audio manager and destroys compies of audio manager in later scenes. Industry recommended method for audio controller.
    // foreach loop assigns varible values for audio control to each  audioSource componant.
    private void Awake()
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



        foreach(Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }





    // a function that plays a sound from the audio manager sounds array based on the name being provided.
    public void soundPlay(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }


    // a function that stops a sound clip playing based on the name provided.
    public void soundStop(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }
}
