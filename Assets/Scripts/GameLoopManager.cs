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

        Application.targetFrameRate = 60;
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
        // Check for state transitions
        HandleStateTransitions();

        // Update game logic based on the current state
        gameManager.UpdateGame(currentState);

        // Handle player input
        inputManager.HandleInput(currentState);

        // Update UI
        uiManager.UpdateUI(currentState);
    }

    private void HandleStateTransitions()
    {

        // Implement state transitions logic here
        // For example, when the player clicks "Play" in the main menu, transition to Playing state.
    }

    public void ChangeState(GameState newState)
    {
        // Handle exiting the current state (e.g., clean up)
        // Transition to the new state
        currentState = newState;
        
        // Handle entering the new state (e.g., setup)
    }
}







public enum GameState
{
    LoadingScreen,
    MainMenu,
    Start,
    Playing,
    GameOver,
    Paused
}
