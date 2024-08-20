using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static Settings currentSettings = null;
    public const string fileName = "Settings.txt";
    [SerializeField] private Slider masterSlider, musicSlider, soundSlider, qualitySlider;
    [SerializeField] private TextMeshProUGUI masterValue, musicValue, soundValue, qualityValue;
    [SerializeField] private Toggle fullScreenToggle, vSyncToggle;
    [SerializeField] private AK.Wwise.Event MenuAdjust, MenuAdjustStop, MenuClick;

    void Awake()
    {
        MenuButton.pleaseNoSound = true;
        LoadSettings();
        gameObject.SetActive(false);
        MenuButton.pleaseNoSound = false;
    }

    public void LoadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/" + fileName))
            currentSettings = JsonUtility.FromJson<Settings>(File.ReadAllText(Application.persistentDataPath + "/" + fileName));
        currentSettings ??= new Settings();
        UpdateFullScreen(false);
        UpdateVSync(false);
        UpdateQuality(false);
        UpdateMasterVolume(false);
        UpdateMusicVolume(false);
        UpdateSoundVolume(false);
    }

    public static void SaveSettings()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        string settingsJSON = JsonUtility.ToJson(currentSettings, true);

        File.WriteAllText(path, settingsJSON);
        Debug.Log("Saved settings to: " + path);
    }

    public void UpdateMasterVolume(bool user)
    {
        if (user)
            currentSettings.masterVolume = masterSlider.value;
        else
            masterSlider.value = currentSettings.masterVolume;

        AkSoundEngine.SetRTPCValue("masterVolume", currentSettings.masterVolume);
        masterValue.text = (int)masterSlider.value + "";
    }

    public void StartAdjust()
    {
        if (MenuButton.pleaseNoSound) return;
        MenuClick.Post(gameObject);
        MenuAdjust.Post(gameObject);
    }

    public void StopAdjust()
    {
        if (MenuButton.pleaseNoSound) return;
        MenuAdjustStop.Post(gameObject);
        MenuClick.Post(gameObject);
    }

    public void UpdateMusicVolume(bool user)
    {
        if (user)
            currentSettings.musicVolume = musicSlider.value;
        else
            musicSlider.value = currentSettings.musicVolume;

        AkSoundEngine.SetRTPCValue("musicVolume", currentSettings.musicVolume);
        musicValue.text = (int)musicSlider.value + "";
    }

    public void UpdateSoundVolume(bool user)
    {
        if (user)
            currentSettings.soundVolume = soundSlider.value;
        else
            soundSlider.value = currentSettings.soundVolume;
        AkSoundEngine.SetRTPCValue("soundVolume", currentSettings.soundVolume);
        soundValue.text = (int)soundSlider.value + "";
    }

    public void UpdateQuality(bool user)
    {
        if (user)
            currentSettings.quality = (int)qualitySlider.value;
        else
            qualitySlider.value = currentSettings.quality; 

        QualitySettings.SetQualityLevel(currentSettings.quality);
        qualityValue.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void UpdateFullScreen(bool user)
    {
        if (user)
        {
            currentSettings.fullScreen = fullScreenToggle.isOn;
            if (!MenuButton.pleaseNoSound)
                MenuClick?.Post(gameObject);
        }
        else
            fullScreenToggle.isOn = currentSettings.fullScreen;

        if (currentSettings.fullScreen)
        {
            currentSettings.xRes = (float)Screen.width / Display.main.systemWidth;
            currentSettings.yRes = (float)Screen.height / Display.main.systemHeight;
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
        }
        else
        {
            currentSettings.xRes = Mathf.Clamp(currentSettings.xRes, 0.1f, 1.0f);
            currentSettings.yRes = Mathf.Clamp(currentSettings.yRes, 4 / 9f, 1.0f);
            Screen.SetResolution((int)(currentSettings.xRes * Display.main.systemWidth), (int)(currentSettings.yRes * Display.main.systemHeight), false);
            Screen.SetResolution((int)(currentSettings.xRes * Display.main.systemWidth), (int)(currentSettings.yRes * Display.main.systemHeight), false);
        }
    }

    public void UpdateVSync(bool user)
    {
        if (user)
        {
            currentSettings.vSync = vSyncToggle.isOn;
            if (!MenuButton.pleaseNoSound)
                MenuClick?.Post(gameObject);
        }
        else
            vSyncToggle.isOn = currentSettings.vSync;

        if (currentSettings.vSync)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }
}