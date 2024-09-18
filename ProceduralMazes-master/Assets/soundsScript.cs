using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundsScript : MonoBehaviour
{
    public AudioClip audioDamage;
    public AudioSource audioSource;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            audioSource.clip = audioDamage;
            audioSource.Play();
        }
    }
}
