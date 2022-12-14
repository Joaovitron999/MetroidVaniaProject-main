using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] music;
    public AudioSource audioSource;
    public int currentTrack = 0;

    void Start()
    {
        audioSource.clip = music[currentTrack];
        audioSource.Play();
    }

    public void ChangeTrack(AudioClip newTrack)
    {
        audioSource.Stop();
        audioSource.clip = newTrack;
        audioSource.Play();
    }

    public void ChangeTrack(int newTrack)
    {
        audioSource.Stop();
        audioSource.clip = music[newTrack];
        audioSource.Play();
    }

    public void NextTrack()
    {
        if (currentTrack < music.Length - 1)
        {
            currentTrack++;
        }
        else
        {
            currentTrack = 0;
        }
        ChangeTrack(currentTrack);
    }

    
}
    
   