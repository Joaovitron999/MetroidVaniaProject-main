using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public AudioClip music;
    public MusicController musicController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            musicController.ChangeTrack(music);
        }
    }

    private void Start()
    {
        musicController = FindObjectOfType<MusicController>();
    }
}
   