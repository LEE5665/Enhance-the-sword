using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider volumeMasterSlider;
    public Slider volumeMusicSlider;
    public Slider volumeSFXSlider;
    public GameObject MainPanel;
    public GameObject SettingPanel;

    public AudioMixer mixer;

    private Resolution[] resolutions;

    void Start()
    {
        
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        
        fullscreenToggle.isOn = Screen.fullScreen;
        float masterVol = PlayerPrefs.GetFloat("Master", 1.0f);
        float musicVol  = PlayerPrefs.GetFloat("Music", 1.0f);
        float sfxVol    = PlayerPrefs.GetFloat("SFX", 1.0f);

        volumeMasterSlider.value = masterVol;
        volumeMusicSlider.value  = musicVol;
        volumeSFXSlider.value    = sfxVol;
        mixer.SetFloat("Master", Mathf.Log10(masterVol) * 20f);
        mixer.SetFloat("Music", Mathf.Log10(musicVol) * 20f);
        mixer.SetFloat("SFX", Mathf.Log10(sfxVol) * 20f);
    }

    public void SetVolume(string parameter, float volume)
    {
        mixer.SetFloat(parameter, Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat(parameter, volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void ApplySettings()
    {
        SetResolution(resolutionDropdown.value);
        SetFullscreen(fullscreenToggle.isOn);
        SetVolume("Master", volumeMasterSlider.value);
        SetVolume("Music", volumeMusicSlider.value);
        SetVolume("SFX", volumeSFXSlider.value);
        PlayerPrefs.Save();
    }

    public void SettingMenu()
    {
        MainPanel.SetActive(false);
        SettingPanel.SetActive(true);
    }

    public void Back()
    {
        MainPanel.SetActive(true);
        SettingPanel.SetActive(false);
    }
}
