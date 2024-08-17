using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;

//holds functions of the main menu and sub menus
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayButton, InstructionsPanel, SettingsPanel, CreditsPanel;
    [SerializeField] private ScreenWipe screenWipe;
    [SerializeField] private AK.Wwise.Event MenuBack, MenuSelect, MenuNav, MenuAdjust;
    [SerializeField] private Slider musicSlider, soundSlider, qualitySlider;
    [SerializeField] private Toggle fullScreenToggle, vSyncToggle;
    private GameObject currentSelection;
    public static bool firstopen = false, quitting = false, playing = false;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
        firstopen = true;
        screenWipe.gameObject.SetActive(true);
        screenWipe.WipeOut();
        musicSlider.value = SettingsManager.currentSettings.musicVolume;
        soundSlider.value = SettingsManager.currentSettings.soundVolume;
        qualitySlider.maxValue = QualitySettings.names.Length;
        fullScreenToggle.isOn = SettingsManager.currentSettings.fullScreen;
        vSyncToggle.isOn = SettingsManager.currentSettings.vSync;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PopupPanel.open)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                currentSelection = EventSystem.current.currentSelectedGameObject;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(currentSelection);
            }
        }

        // TODO save file detection and conditional load button nav behavior (ref daylight)
    }

    public void NewRun()
    {
        if (playing) return;
        SettingsManager.SaveSettings();
        playing = true;
        MenuSelect?.Post(gameObject);
        screenWipe.WipeIn();
        screenWipe.PostWipe += StartRun;
    }

    public void ResumeRun()
    {
        if (playing) return;
        SettingsManager.SaveSettings();
        playing = true;
        MenuSelect?.Post(gameObject);
        screenWipe.WipeIn();
        screenWipe.PostWipe += LoadRun;
    }

    void StartRun()
    {
        screenWipe.PostWipe -= StartRun;
        firstopen = false;
        playing = false;
        SceneManager.LoadScene("SampleScene"); //TODO change this whenever
    }
    
    void LoadRun()
    {
        screenWipe.PostWipe -= LoadRun;
        firstopen = false;
        playing = false;
        SceneManager.LoadScene("SampleScene"); //TODO change this whenever
        // TODO load save data
    }

    public void Instructions()
    {
        if (!PopupPanel.open && !playing && !quitting)
        {
            InstructionsPanel.SetActive(true);
            MenuSelect?.Post(gameObject);
        }
    }

    public void Settings()
    {
        if (!PopupPanel.open && !playing && !quitting)
        {
            SettingsPanel.SetActive(true);
            MenuSelect?.Post(gameObject);
        }
    }

    public void ToggleFullscreen()
    {
        SettingsManager.currentSettings.fullScreen = fullScreenToggle.isOn;
        SettingsManager.UpdateFullscreen();
        MenuAdjust?.Post(gameObject);
        Debug.Log(SettingsManager.currentSettings.fullScreen);
    }

    public void ToggleVsync()
    {
        SettingsManager.currentSettings.vSync = vSyncToggle.isOn;
        if (SettingsManager.currentSettings.vSync)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
        MenuAdjust?.Post(gameObject);
        Debug.Log("vsync: " + SettingsManager.currentSettings.vSync);
    }

    public void UpdateQuality()
    {
        SettingsManager.currentSettings.quality = (int) qualitySlider.value;
        QualitySettings.SetQualityLevel(SettingsManager.currentSettings.quality);
        Debug.Log(SettingsManager.currentSettings.quality);
    }

    public void UpdateMusicVolume()
    {
        SettingsManager.currentSettings.musicVolume = musicSlider.value;
        MenuAdjust?.Post(gameObject);
        AkSoundEngine.SetRTPCValue("musicVolume", SettingsManager.currentSettings.musicVolume);
        Debug.Log(SettingsManager.currentSettings.musicVolume);
    }

    public void UpdateSoundVolume()
    {
        SettingsManager.currentSettings.soundVolume = soundSlider.value;
        MenuAdjust?.Post(gameObject);
        AkSoundEngine.SetRTPCValue("soundVolume", SettingsManager.currentSettings.soundVolume);
        Debug.Log(SettingsManager.currentSettings.soundVolume);
    }

    public void Credits()
    {
        if (!PopupPanel.open && !playing && !quitting)
        {
            CreditsPanel.SetActive(true);
            MenuSelect?.Post(gameObject);
        }
    }

    public void Quit()
    {
        if (quitting || playing) return;
        quitting = true;
        MenuSelect?.Post(gameObject);
        screenWipe.WipeIn();
        screenWipe.PostWipe += ExitGame;
    }

    public void ExitGame()
    {
        screenWipe.PostWipe -= ExitGame;
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnApplicationQuit()
    {
        SettingsManager.SaveSettings();
    }
}