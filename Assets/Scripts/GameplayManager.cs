using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager main;
    public ScreenWipe screenWipe;
    public bool startedSequence = false;
    public bool suspendSequence = false;
    public static bool paused, pauseOpen;
    [SerializeField] private GameObject PauseMenu, WinScreen, WinGameScreen, LoseScreen, SettingsPanel, DoctorsNote, WorkshopMenu;
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private Sprite PauseClicked, PauseNormal;
    //[SerializeField] private TextMeshProUGUI WinText;
    [SerializeField] public GameObject globalWwise;
    [SerializeField] private AK.Wwise.Event PauseMusic, ResumeMusic, StopMusic, StartMusic, MenuSelect, GameOver, WinSound, TakeDamage;
    public clickToSpawnManager spawnManager;
    public SaveManager saveManager;
    private static bool nextLevel = false;

    //number of ants that need to get to the goal in order for the player to lose
    public int maxPlayerHealth = 3;
    public int currPlayerHealth;
    //points used to buy/maintain towers
    public int resourcePoints;
    int startingResourcePoints = 10;

    // [SerializeField] private AK.Wwise.State calm, mediate, intense, silent, none;
    // private enum MusicState { CALM, MEDIATE, INTENSE };
    // private MusicState currentState;
    public static bool won = false, wonGame = false, lost = false, playingAgain = false, quit = false;
    public static int beginning = 0;
    [SerializeField] private GameObject DoctorsNoteButton, DoctorsNoteImage;

    void Awake()
    {
        MainMenuManager.firstopen = true;
        if (!SaveManager.loadingFromSave)
            screenWipe.PostUnwipe += AutoSave;
    }

    // Start is called before the first frame update
    void Start()
    {
        main = this;

        if (!nextLevel)
        {
            if (!SaveManager.loadingFromAutoSave && !SaveManager.loadingFromSave)
                StartIntroSequence();
            else
                StartMusic.Post(globalWwise);
        }
        else
        {
            StartMusic.Post(globalWwise);
            nextLevel = false;
        }

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

        if (beginning == 2)
        {
            var btnPos = DoctorsNoteButton.transform.localPosition;
            DoctorsNoteButton.transform.localPosition = new Vector3(btnPos.x, Mathf.Lerp(btnPos.y, -1400, 0.02f), btnPos.z);
            var imgPos = DoctorsNoteImage.transform.localPosition;
            DoctorsNoteImage.transform.localPosition = new Vector3(imgPos.x, Mathf.Lerp(imgPos.y, 1100, 0.02f), imgPos.z);
        }
        else if (beginning == 1)
        {
            var btnPos = DoctorsNoteButton.transform.localPosition;
            DoctorsNoteButton.transform.localPosition = new Vector3(btnPos.x, Mathf.Lerp(btnPos.y, -400, 0.02f), btnPos.z);
            var imgPos = DoctorsNoteImage.transform.localPosition;
            DoctorsNoteImage.transform.localPosition = new Vector3(imgPos.x, Mathf.Lerp(imgPos.y, 100, 0.02f), imgPos.z);
            if (EventSystem.current.currentSelectedGameObject != DoctorsNoteButton)
            {
                MenuButton.noSound = true;
                EventSystem.current.SetSelectedGameObject(DoctorsNoteButton);
            }
        }

        // if (suspendSequence)
        //     return;

        // if (startedSequence && (/*gameSequence.GetCurrentAnimatorStateInfo(0).IsName("NoCurrentSequence") ||*/ !dialog.reading))
        //     FinishedSequence();

        // if (!startedSequence && !dialog.reading && Input.GetKeyDown(KeyCode.Mouse0) && (!won && !lost))
        //     StartSequence();
    }

    public void StartIntroSequence()
    {
        DoctorsNote.SetActive(true);
        Time.timeScale = 0;
        paused = true;
        beginning = 1;
        StartCoroutine(StartButtonCooldown());
    }

    IEnumerator StartButtonCooldown()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        MenuButton.noSound = true;
        EventSystem.current.SetSelectedGameObject(DoctorsNoteButton);
    }

    public void EndIntroSequence()
    {
        DoctorsNoteButton.GetComponent<Button>().interactable = false;
        MenuSelect.Post(gameObject);
        beginning = 2;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(0.75f);
        paused = false;
        beginning = 0;
        Time.timeScale = 1;
        StartMusic.Post(globalWwise);
        DoctorsNote.SetActive(false);
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

    public void UnPause(bool user = false)
    {
        if (!PopupPanel.open) return;
        paused = false;
        if (user)
            MenuSelect.Post(gameObject);
        StartCoroutine(FinishPause());
        PauseMenu.GetComponent<PopupPanel>().Close();
        PauseMenu.GetComponentsInChildren<Button>();
        SettingsManager.SaveSettings();
        MenuButton.pleaseNoSound = true;
        MenuButton.noSound = true;
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
        ResumeMusic.Post(globalWwise);
        MenuButton.pleaseNoSound = false;
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
            if (WinGameScreen != null && WinGameScreen.activeSelf == true)
            {
                WinGameScreen.GetComponent<PopupPanel>().Close();
            }
            if (LoseScreen != null && LoseScreen.activeSelf == true)
            {
                LoseScreen.GetComponent<PopupPanel>().Close();
            }
            if (WorkshopMenu != null && WorkshopMenu.activeSelf == true){
                WorkshopMenu.GetComponent<PopupPanel>().Close();
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
        if (wonGame)
        {
            wonGame = false;
            saveManager.DeleteSave();
        }
        else
            saveManager.SaveGame();
        lost = false;
        pauseOpen = false;
        clickToSpawnManager.placedTowers.Clear();
        WaveManager.CurrentWave = 0;
        stageManager.levelToLoad = 1;
        StopMusic?.Post(globalWwise);
        screenWipe.WipeIn();
        screenWipe.PostWipe += LoadMenu;
    }

    public void LoadMenu()
    {
        screenWipe.PostWipe -= LoadMenu;
        PopupPanel.numPopups = 0;
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadGame()
    {
        screenWipe.PostWipe -= ReloadGame;
        SceneManager.LoadScene("SampleScene");
    }

    public void ReloadLevel()
    {
        screenWipe.PostWipe -= ReloadLevel;
        stageManager.levelToLoad = stageManager.level;
        nextLevel = true;
        SceneManager.LoadScene("SampleScene");
        playingAgain = false;
    }

    public void Win()
    {
        ClosePanels();
        won = true;
        StopMusic.Post(globalWwise);
        WinSound.Post(globalWwise);
        WinScreen.SetActive(true);
    }

    public void WinGame()
    {
        ClosePanels();
        won = true;
        wonGame = true;
        StopMusic.Post(globalWwise);
        WinSound.Post(globalWwise);
        WinGameScreen.SetActive(true);
    }

    public void Lose()
    {
        ClosePanels();
        lost = true;
        GameOver?.Post(globalWwise);
        if (LoseScreen != null)
            LoseScreen.SetActive(true);
    }

    //resets game
    public void PlayAgain()
    {
        if (playingAgain) return;
        playingAgain = true;
        Time.timeScale = 1;

        ClosePanels();
        MenuSelect?.Post(gameObject);
        won = false;
        lost = false;

        paused = false;
        won = false;
        lost = false;
        pauseOpen = false;
        clickToSpawnManager.placedTowers.Clear();
        WaveManager.CurrentWave = 0;
        stageManager.levelToLoad = 1;
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
        screenWipe.PostWipe += ReloadLevel;
    }

    public void NextLevel()
    {
        if (playingAgain) return;
        playingAgain = true;

        MenuSelect?.Post(gameObject);
        ClosePanels();
        won = false;
        lost = false;

        Time.timeScale = 1;
        paused = false;
        won = false;
        lost = false;
        pauseOpen = false;
        clickToSpawnManager.placedTowers.Clear();
        screenWipe.WipeIn();
        StopMusic.Post(globalWwise);
        nextLevel = true;
        screenWipe.PostWipe += LoadNextLevel;
    }

    public void LoadNextLevel()
    {
        if (stageManager.level + 1 > 3)
        {
            WinGame();
        }

        screenWipe.PostWipe -= LoadNextLevel;
        stageManager.levelToLoad = stageManager.level + 1;
        SceneManager.LoadScene("SampleScene");
        playingAgain = false;
    }

    //player takes damage from enemy reaching base
    public void takeDamage(){
        currPlayerHealth -= 1;
        if(currPlayerHealth <= 0){
            Lose();
        }
        else if(currPlayerHealth > maxPlayerHealth){
            currPlayerHealth = maxPlayerHealth;
        }
        TakeDamage?.Post(globalWwise);
        HUDManager.main.HurtEffect();
        HUDManager.main.UpdateHealth();
    }

    //reset player health to maximum, likely at start of stage
    void resetHealth(){
        currPlayerHealth = maxPlayerHealth;
        HUDManager.main.UpdateHealth();
    }

    //add single resource point
    public void addResource(){
        resourcePoints += 1;
        HUDManager.main.UpdateEXP();
    }
    //adds an amount of resource pounts
    public void addResource(int amount){
        resourcePoints += amount;
        HUDManager.main.UpdateEXP();
    }
    public void spendResource(int amount){
        resourcePoints -= amount;
        HUDManager.main.UpdateEXP();
    }
    public void resetResource(){
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