using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingSlider : MonoBehaviour, IPointerUpHandler
{
    public enum VolumeType { Master, Music, SFX }
    public VolumeType type;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();

        
        slider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float value)
    {
        switch (type)
        {
            case VolumeType.Master:
                AudioManager.Instance.SetMasterVolume(value);
                break;
            case VolumeType.Music:
                AudioManager.Instance.SetMusicVolume(value);
                break;
            case VolumeType.SFX:
                AudioManager.Instance.SetSFXVolume(value);
                break;
        }
    }

    
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("볼륨 저장");
        float value = slider.value;
        switch (type)
        {
            case VolumeType.Master:
                AudioManager.Instance.SaveMasterVolume(value);
                break;
            case VolumeType.Music:
                AudioManager.Instance.SaveMusicVolume(value);
                break;
            case VolumeType.SFX:
                AudioManager.Instance.SaveSFXVolume(value);
                break;
        }
    }
}