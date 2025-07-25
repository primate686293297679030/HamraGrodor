using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

public class GameLoopManager : MonoBehaviour
{
    private GameState currentState;
 
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private UIManager uiManager;
    public static GameProgress progress;
   public static GameLoopManager instance;
    public static Action<GameState> GameStates;
    public GameObject developerMenu;
    private void Awake()
    {
        if (instance == null)
        { instance = this; }
        else if (instance != this)
        { Destroy(gameObject); }

       // Application.targetFrameRate = 60;
        initSave();
    }
    private void Start()
    {
       

        GameStates += ChangeState;
        // Initialize components
       //gameManager = new GameManager();
       //inputManager = new InputManager();
       //uiManager = new UIManager();

        // Set initial game state
        ChangeState(GameState.LoadingScreen);
    }

    private void initSave()
    {
        if (ProgressManager.LoadGameProgress("savedProgress")==null)
        {
            progress = new GameProgress();
            progress._timeLimitIndex = 0;
            progress.Highscore30 = 0;
            progress.Highscore60 = 0;
            progress.Highscore90 = 0;
            progress.Highscore120 = 0;
            progress._score = 0;
            progress.audio = true;
            progress.music = true;
            progress.speed = 640f;
           progress.SpeedDecreaseValue = 200f;
            progress.SpeedBoostValue = 200f;
            progress.NightModeLength = 5f;

            ProgressManager.SaveGameProgress("savedProgress", progress);
        }
        else
        {
            progress = ProgressManager.LoadGameProgress("savedProgress");
        }
      
    }

    private void OnDestroy()
    {
        GameStates -= ChangeState;
      
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ProgressManager.SaveGameProgress("savedProgress", progress);
            developerMenu.SetActive(!developerMenu.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            ProgressManager.DeleteGameProgress();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Application.Quit();
        }
  
        HandleStateTransitions();

     
        gameManager.UpdateGame(currentState);

 
        inputManager.HandleInput(currentState);

        
        uiManager.UpdateUI(currentState);
    }

    private void HandleStateTransitions()
    {

 
    }

    public void ChangeState(GameState newState)
    {
      
        currentState = newState;

    }
}







public enum GameState
{
    LoadingScreen,
    MainMenu,
    Start,
    Playing,
    GameOver,
    Highscore,
    Paused
}
