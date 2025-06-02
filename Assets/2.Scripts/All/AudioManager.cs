using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource Master;
    public AudioSource Music;
    public AudioSource SFX;
    public AudioClip Click;
    public AudioMixer mixer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ClickSound()
    {
        SFX.PlayOneShot(Click);
    }

    public void SetMasterVolume(float value)
    {
        Master.volume = Mathf.Clamp01(value);
        mixer.SetFloat("Master", Mathf.Log10(value <= 0 ? 0.0001f : value) * 20f); 
    }

    public void SetMusicVolume(float value)
    {
        Music.volume = Mathf.Clamp01(value);
        mixer.SetFloat("Music", Mathf.Log10(value <= 0 ? 0.0001f : value) * 20f);
    }

    public void SetSFXVolume(float value)
    {
        SFX.volume = Mathf.Clamp01(value);
        mixer.SetFloat("SFX", Mathf.Log10(value <= 0 ? 0.0001f : value) * 20f);
    }

    
    public void SaveMasterVolume(float value)
    {
        PlayerPrefs.SetFloat("Master", value);
        PlayerPrefs.Save();
    }
    public void SaveMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("Music", value);
        PlayerPrefs.Save();
    }
    public void SaveSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFX", value);
        PlayerPrefs.Save();
    }
}
