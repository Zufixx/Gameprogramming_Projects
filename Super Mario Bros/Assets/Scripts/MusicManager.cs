using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {

    [SerializeField]
    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}

    public void PlayMusic()
    {
        audioSource.Play();
    }

    public void PlayMusic(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlayMusic(AudioClip audioClip, bool loop)
    {
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void SetClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void ToggleLoop()
    {
        audioSource.loop = !audioSource.loop;
    }
}
