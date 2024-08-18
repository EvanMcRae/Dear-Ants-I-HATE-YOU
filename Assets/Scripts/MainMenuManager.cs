using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

//holds functions of the main menu and sub menus
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayButton, InstructionsPanel, SettingsPanel, CreditsPanel;
    [SerializeField] private ScreenWipe screenWipe;
    [SerializeField] private AK.Wwise.Event MenuBack, MenuSelect, MenuNav;
    private GameObject currentSelection;
    public static bool firstopen = false, quitting = false, playing = false;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
        firstopen = true;
        screenWipe.gameObject.SetActive(true);
        screenWipe.WipeOut();
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