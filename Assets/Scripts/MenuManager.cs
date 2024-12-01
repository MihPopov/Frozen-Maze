using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainPanel;
    public GameObject levelPanel;
    public GameObject levelContainer;
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject hearts;
    public GameObject tutorialFrame;

    private void Start()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        }
    }
    public void PlayGame()
    {
        mainPanel.SetActive(false);
        levelPanel.SetActive(true);

        Button[] buttons = levelContainer.GetComponentsInChildren<Button>();
        int lastLevel = PlayerPrefs.GetInt("LastCompletedLevel", -1);
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i <= lastLevel) buttons[i].GetComponent<Image>().color = Color.green;
            else if (i > lastLevel + 1)
            {
                buttons[i].GetComponent<Image>().color = Color.red;
                buttons[i].enabled = false;
            }
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void SettingsPanel()
    {
        settingsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
    public void ExitPanel()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    public void ExitLevel()
    {
        mainPanel.SetActive(true);
        levelPanel.SetActive(false);
    }

    public void LoadScene(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }

    public void Pause()
    {
        mainPanel.SetActive(false);
        pauseButton.SetActive(false);
        hearts.SetActive(false);
        settingsPanel.SetActive(false);
        if (levelPanel != null) levelPanel.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        mainPanel.SetActive(true);
        pauseButton.SetActive(true);
        hearts.SetActive(true);
        settingsPanel.SetActive(true);
        if (levelPanel != null) levelPanel.SetActive(true);
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowAndHideTutorial()
    {
        if (Time.timeScale > 0f)
        {
            mainPanel.SetActive(false);
            pauseButton.SetActive(false);
            hearts.SetActive(false);
            settingsPanel.SetActive(false);
            tutorialFrame.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            mainPanel.SetActive(true);
            pauseButton.SetActive(true);
            hearts.SetActive(true);
            settingsPanel.SetActive(true);
            tutorialFrame.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
