using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject soundButton;
    public Sprite spriteOn;
    public Sprite spriteOff;
    public AudioSource audioSource;
    public Slider volumeSlider;

    private bool soundOn = true;

    private void Start()
    {
        soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.25f);
        audioSource = FindObjectOfType<AudioSource>();
        audioSource.mute = !soundOn;
        audioSource.volume = volumeSlider.value;
        soundButton.GetComponent<Image>().sprite = soundOn ? spriteOn : spriteOff;
    }

    private void Update()
    {
        audioSource.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("SoundOn", soundOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        SceneManager.LoadScene("Maze2D");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        settingsMenu.SetActive(true);
    }

    public void MainMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void SoundOn()
    {
        if(soundOn)
        {
            soundOn = false;
            soundButton.GetComponent<Image>().sprite = spriteOff;
            audioSource.mute = true;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite=spriteOn;
            soundOn = true;
            audioSource.mute = false;
        }
    }
}
