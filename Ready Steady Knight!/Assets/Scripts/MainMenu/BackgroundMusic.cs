using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;  //varible used to track single instance of object holding the script.

    public AudioClip[] tracks; // array to hold the simulations soundtrack.
    private AudioSource backgroundMusic; //reference to audiosource componant on the background music object.
    private int i; //integer used for indexing soundtrack.



    // awake function checks for instance of backgroundMusic object and destroys compies of background music object in later scenes. Industry recommended method for audio controller.
    // second half of function sets the index to 0 and plays the soundtrack from the beginning.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        i = 0;
        backgroundMusic = this.GetComponent<AudioSource>();
        backgroundMusic.clip = tracks[i];
        backgroundMusic.Play();
    }


    // played in the LateUpdate function, called every end of frame, checks to see if the audiosource componant is currently playing music.
    // if it isnt, either by being stopped or having a clip end, the index increases by 1 and plays the next track in the list. If the index becomes larger than the length of the sountrack, the cycle repeats.
    private void LateUpdate()
    {
        if(!backgroundMusic.isPlaying)
        {
            i += 1;
            if (i <= tracks.Length-1)
            {
                backgroundMusic.clip = tracks[i];

                backgroundMusic.Play();
            }
            else
            {
                i = 0;
                backgroundMusic.clip = tracks[i];

                backgroundMusic.Play();
            }
        }
    }
}
