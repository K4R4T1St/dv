using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public AudioSource audioSource;

    public void Start()
    {
        bool soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        float volume = PlayerPrefs.GetFloat("Volume", 0.25f);
        audioSource.mute = !soundOn;
        audioSource.volume = volume;

    }

    public void Load()
    {
        SceneManager.LoadScene("Menu");
    }
}
