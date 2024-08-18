using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager main;
    public ScreenWipe screenWipe;
    public bool startedSequence = false;
    public bool suspendSequence = false;
    public static bool paused, pauseOpen;
    [SerializeField] private GameObject PauseMenu, WinScreen, LoseScreen, SettingsPanel;
    [SerializeField] private TextMeshProUGUI WinText;
    [SerializeField] private GameObject globalWwise;
    [SerializeField] private AK.Wwise.Event PauseMusic, ResumeMusic, StopMusic, StartMusic, MenuSelect, GameOver;

    // [SerializeField] private AK.Wwise.State calm, mediate, intense, silent, none;
    // private enum MusicState { CALM, MEDIATE, INTENSE };
    // private MusicState currentState;
    public static bool won = false, lost = false, playingAgain = false, quit = false;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        // IntroSequence();
        won = false;
        lost = false;
        playingAgain = false;
        quit = false;
    }

    // Update is called once per frame
    void Update()
    {
        //pause/unpause the game when pause button pressed, but only if screen wipe is over
        if (Input.GetKeyDown(KeyCode.Escape) && ScreenWipe.over)
        {
            if (!paused && !PopupPanel.open && !won && !lost)
                Pause();
            else if (paused && pauseOpen && !SettingsPanel.activeSelf)
                UnPause();
        }

        // if (suspendSequence)
        //     return;

        // if (startedSequence && (/*gameSequence.GetCurrentAnimatorStateInfo(0).IsName("NoCurrentSequence") ||*/ !dialog.reading))
        //     FinishedSequence();

        // if (!startedSequence && !dialog.reading && Input.GetKeyDown(KeyCode.Mouse0) && (!won && !lost))
        //     StartSequence();
    }

    public void IntroSequence()
    {
        // dialog.setSource(new DialogSource("[ss, .025]Left click to start! [exit]"));
        // dialog.reading = true;
    }

    public void Pause()
    {
        PauseMusic?.Post(globalWwise);
        Time.timeScale = 0;
        paused = true;
        StartCoroutine(StartPause());
        InterferenceEffect.time = 0.5f;
        ScreenEffects(true);
    }

    public void UnPause()
    {
        if (!PopupPanel.open) return;
        MenuSelect?.Post(gameObject);
        ResumeMusic?.Post(globalWwise);
        paused = false;
        StartCoroutine(FinishPause());
        PauseMenu.GetComponent<PopupPanel>().Close();
        SettingsManager.SaveSettings();
    }

    IEnumerator StartPause()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        pauseOpen = true;
        PauseMenu.SetActive(true);
    }

    IEnumerator FinishPause()
    {
        yield return new WaitForSecondsRealtime(0.35f);
        Time.timeScale = 1;
        ScreenEffects(false);
        pauseOpen = false;
    }

    public void ScreenEffects(bool enable)
    {
        ImageEffect[] comps = Camera.main.GetComponents<ImageEffect>();
        foreach (ImageEffect effect in comps)
        {
            effect.enabled = enable;
        }
    }

    public void Settings()
    {
        SettingsPanel.gameObject.SetActive(true);
        MenuSelect?.Post(gameObject);
        PauseMenu.GetComponent<PopupPanel>().ClosableOverride = true;
    }

    //close currently opened popup panels
    public void ClosePanels()
    {
        if (PopupPanel.open)
        {
            if (PauseMenu.activeSelf == true)
            {
                PauseMenu.GetComponent<PopupPanel>().Close();
            }
            if (WinScreen != null && WinScreen.activeSelf == true)
            {
                WinScreen.GetComponent<PopupPanel>().Close();
            }
            if (LoseScreen != null && LoseScreen.activeSelf == true)
            {
                LoseScreen.GetComponent<PopupPanel>().Close();
            }
        }
    }

    public void QuitToMenu()
    {
        if (quit) return;
        quit = true;
        MenuSelect?.Post(gameObject);
        ClosePanels();
        Time.timeScale = 1;
        paused = false;
        StopMusic?.Post(globalWwise);
        screenWipe.WipeIn();
        screenWipe.PostWipe += LoadMenu;
    }

    public void LoadMenu()
    {
        screenWipe.PostWipe -= LoadMenu;
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadGame()
    {
        screenWipe.PostWipe -= ReloadGame;
        SceneManager.LoadScene("SampleScene"); // TODO CHANGE
    }

    public void Win()
    {
        if (paused) return;
        won = true;
        // DialogController.main.StopTalk();
        StopMusic?.Post(globalWwise);
        WinScreen.SetActive(true);
        // WinText.SetText("You made it through without laughing!\n\nFinal Grade: " + meter.calculateGrade());
    }

    public void Lose()
    {
        if (paused) return;
        lost = true;
        // DialogController.main.StopTalk();
        GameOver?.Post(globalWwise);
        LoseScreen.SetActive(true);
        // button.stopInputs = true;
        // dialog.reading = false;
    }

    //resets game
    public void PlayAgain()
    {
        if (playingAgain) return;
        playingAgain = true;

        MenuSelect?.Post(gameObject);
        ClosePanels();
        won = false;
        lost = false;

        Time.timeScale = 1;
        paused = false;
        screenWipe.WipeIn();
        StopMusic?.Post(globalWwise);
        screenWipe.PostWipe += ReloadGame;
    }

    // //syncs clocks for start of gameplay
    // public void StartSequence()
    // {
    //     startedSequence = true;
    //     calm.SetValue(); // start calm music

    //     //dialog.setSource(new DialogSource("[c] Blah blah blah."));
    //     dialog.setSource(new DialogSource("[lf,WormMartEmployee.txt]"));
    //     dialog.reading = true;
    //     //gameSequence.Play("24hrEmployee");


    //     meter.responseQueue.Add(("It's ok!", "It's about time.", "16!?"));
    //     button.stopInputs = false;
    // }

    // public void FinishedSequence()
    // {
    //     startedSequence = false;
    //     Debug.Log("Finished sequence");
    //     //gameSequence.Play("NoCurrentSequence");
    //     while (!won)
    //     {
    //         Win();
    //     }
    // }
}