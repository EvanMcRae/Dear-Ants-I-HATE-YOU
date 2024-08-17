using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static Settings currentSettings = null;
    public const string fileName = "Settings.txt";

    void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/" + fileName) && currentSettings == null)
            LoadSettings();
        else if (currentSettings == null)
        {
            currentSettings = new Settings();
            SaveSettings();
        }
    }

    public static void LoadSettings()
    {
        currentSettings = JsonUtility.FromJson<Settings>(File.ReadAllText(Application.persistentDataPath + "/" + fileName));
        currentSettings ??= new Settings();
        UpdateFullscreen();

        QualitySettings.SetQualityLevel(currentSettings.quality);
        if (currentSettings.vSync)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }

    public static void UpdateFullscreen()
    {
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

    public static void SaveSettings()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        string settingsJSON = JsonUtility.ToJson(currentSettings, true);

        File.WriteAllText(path, settingsJSON);
        Debug.Log("Saved settings to: " + path);
    }
}
