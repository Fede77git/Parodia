using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip background;
    public AudioClip ballNet;

    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }


    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    
    public void OnMusic()
    {
        musicSource.Play();
    }

    public void OffMusic()
    {
        musicSource.Stop();

    }

}
