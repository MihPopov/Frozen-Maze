using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public Dropdown qualityDropdown;
    public Slider volumeSlider;
    public Image buttonsVar1;
    public Image buttonsVar2;
    Resolution[] resolutions;
    void Start()
    {
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].height + "X" + resolutions[i].width  + " " + resolutions[i].refreshRateRatio + "Hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        LoadSettings(currentResolutionIndex);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        if (PlayerPrefs.GetInt("MoveButtonsVariant", 1) == 1)
        {
            buttonsVar1.color = Color.green;
            buttonsVar2.color = Color.white;
        }
        else
        {
            buttonsVar1.color = Color.white;
            buttonsVar2.color = Color.green;
        }
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.height, resolution.width, Screen.fullScreen);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QulitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        if (buttonsVar1.color == Color.green) {
            PlayerPrefs.SetInt("MoveButtonsVariant", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MoveButtonsVariant", 2);
        }
    }
    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QulitySettingPreference"))
            qualityDropdown.value = PlayerPrefs.GetInt("QulitySettingPreference");
        else
            qualityDropdown.value = 6;
        if (PlayerPrefs.HasKey("FullScreenPreference"))
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullScreenPreference"));
        else
            Screen.fullScreen = true;
    }
    public void OnSoundChange()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = volumeSlider.value;
        }
    }

    public void MoveButtonsVariantChange(int variant)
    {
        if (variant == 1)
        {
            buttonsVar1.color = Color.green;
            buttonsVar2.color = Color.white;
        }
        else 
        {
            buttonsVar1.color = Color.white;
            buttonsVar2.color = Color.green;
        }
    }
}