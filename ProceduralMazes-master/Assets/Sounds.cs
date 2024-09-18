using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sounds;

    private AudioSource audioSource => GetComponent<AudioSource>();

    public void PlaySound(AudioClip clip, float volume = 1f, bool destroed = false)
    {
        if(destroed)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        }
        else
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
