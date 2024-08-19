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
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private Sprite PauseClicked, PauseNormal;
    [SerializeField] private TextMeshProUGUI WinText;
    [SerializeField] private GameObject globalWwise;
    [SerializeField] private AK.Wwise.Event PauseMusic, ResumeMusic, StopMusic, StartMusic, MenuSelect, GameOver;
    public clickToSpawnManager spawnManager;
    public SaveManager saveManager; 

    //number of ants that need to get to the goal in order for the player to lose
    public int maxPlayerHealth = 3;
    public int currPlayerHealth;
    //points used to buy/maintain towers
    public int resourcePoints;
    int startingResourcePoints = 10;

    // [SerializeField] private AK.Wwise.State calm, mediate, intense, silent, none;
    // private enum MusicState { CALM, MEDIATE, INTENSE };
    // private MusicState currentState;
    public static bool won = false, lost = false, playingAgain = false, quit = false;

    void Awake()
    {
        if (!SaveManager.loadingFromSave)
            screenWipe.PostUnwipe += AutoSave;
    }

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        // IntroSequence();
        won = false;
        lost = false;
        playingAgain = false;
        quit = false;

        currPlayerHealth = maxPlayerHealth;
        resourcePoints = startingResourcePoints;
        HUDManager.main.UpdateEXP();
        HUDManager.main.UpdateHealth();
    }

    public static void AutoSave()
    {
        main.screenWipe.PostUnwipe -= AutoSave;
        main.saveManager.SaveGame(true);
    }

    // Update is called once per frame
    void Update()
    {
        //pause/unpause the game when pause button pressed, but only if screen wipe is over
        if (Input.GetKeyDown(KeyCode.Escape) && ScreenWipe.over)
        {
            if (!paused && !PopupPanel.open && !won && !lost && !pauseOpen)
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

    public void Pause(bool user = false)
    {
        if (!paused && !PopupPanel.open && !won && !lost && !pauseOpen)
        {
            PauseMusic.Post(globalWwise);
            Time.timeScale = 0;
            paused = true;
            StartCoroutine(StartPause());
            InterferenceEffect.time = 0.5f;
            ScreenEffects(true);
            if (user)
            {
                PauseButton.GetComponent<Image>().sprite = PauseClicked;
                PauseButton.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void UnPause()
    {
        if (!PopupPanel.open) return;
        MenuSelect?.Post(gameObject);
        ResumeMusic.Post(globalWwise);
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
        PauseButton.GetComponent<Image>().sprite = PauseNormal;
        PauseButton.GetComponent<Button>().interactable = true;
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
        won = false;
        lost = false;
        pauseOpen = false;
        saveManager.SaveGame();
        clickToSpawnManager.placedTowers.Clear();
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
        SceneManager.LoadScene("SampleScene");
    }

    public void ReloadSave()
    {
        screenWipe.PostWipe -= ReloadSave;
        SaveManager.loadingFromSave = true;
        SaveManager.loadingFromAutoSave = true;
        SceneManager.LoadScene("SampleScene");
        playingAgain = false;
    }

    public void Win()
    {
        if (paused) return;
        won = true;
        // DialogController.main.StopTalk();
        StopMusic.Post(globalWwise);
        WinScreen.SetActive(true);
        // WinText.SetText("You made it through without laughing!\n\nFinal Grade: " + meter.calculateGrade());
    }

    public void Lose()
    {
        if (paused) return;
        lost = true;
        // DialogController.main.StopTalk();
        GameOver?.Post(globalWwise);
        if (LoseScreen != null)
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
        pauseOpen = false;
        screenWipe.WipeIn();
        StopMusic.Post(globalWwise);
        screenWipe.PostWipe += ReloadGame;
    }

    //resets game
    public void RetryStage()
    {
        if (playingAgain) return;
        playingAgain = true;

        MenuSelect?.Post(gameObject);
        ClosePanels();
        won = false;
        lost = false;

        Time.timeScale = 1;
        paused = false;
        pauseOpen = false;
        screenWipe.WipeIn();
        StopMusic.Post(globalWwise);
        screenWipe.PostWipe += ReloadSave;
    }

    //player takes damage from enemy reaching base
    void takeDamage(){
        currPlayerHealth -= 1;
        if(currPlayerHealth <= 0){
            Lose();
        }
        else if(currPlayerHealth > maxPlayerHealth){
            currPlayerHealth = maxPlayerHealth;
        }
        HUDManager.main.UpdateHealth();
    }

    //reset player health to maximum, likely at start of stage
    void resetHealth(){
        currPlayerHealth = maxPlayerHealth;
        HUDManager.main.UpdateHealth();
    }

    //add single resource point
    void addResource(){
        resourcePoints += 1;
        HUDManager.main.UpdateEXP();
    }
    //adds an amount of resource pounts
    void addResource(int amount){
        resourcePoints += amount;
        HUDManager.main.UpdateEXP();
    }
    void spendResource(int amount){
        resourcePoints -= amount;
        HUDManager.main.UpdateEXP();
    }
    void resetResource(){
        resourcePoints = startingResourcePoints;
        HUDManager.main.UpdateEXP();
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