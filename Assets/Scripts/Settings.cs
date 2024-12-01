using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Slider volumeSlider;

    Resolution[] resolutions;
    void Start()
    {
        resolutionDropdown.ClearOptions();
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
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
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
        PlayerPrefs.SetInt("ResolutiongPreference", resolutionDropdown.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QulitySettingPreference"))
            qualityDropdown.value = PlayerPrefs.GetInt("QulitySettingPreference");
        else
            qualityDropdown.value = 6;
        if (PlayerPrefs.HasKey("ResolutiongPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutiongPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;
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
}