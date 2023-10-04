using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    private GameState currentState;
    private GameManager gameManager;
    private InputManager inputManager;
    private UIManager uiManager;

    private void Start()
    {
        // Initialize components
        gameManager = new GameManager();
        inputManager = new InputManager();
        uiManager = new UIManager();

        // Set initial game state
        ChangeState(GameState.MainMenu);
    }

    private void Update()
    {
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

    private void ChangeState(GameState newState)
    {
        // Handle exiting the current state (e.g., clean up)
        // Transition to the new state
        currentState = newState;
        // Handle entering the new state (e.g., setup)
    }
}

public class UIManager:MonoBehaviour
{
    public void UpdateUI(GameState currentState)
    {


    }
}

public class InputManager:MonoBehaviour
{
    // Singleton instance for the InputManager
    private static InputManager instance;

    // Ensure only one instance of InputManager exists
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                
                instance = FindObjectOfType<InputManager>();
                if (instance == null)
                {
                    GameObject inputManagerObject = new GameObject("InputManager");
                    instance = inputManagerObject.AddComponent<InputManager>();
                }
            }
            return instance;
        }
    }

    // Define touch-related variables
    private bool isTouching = false;
    private Vector2 touchStartPosition;

    private void Update()
    {
        // Handle mobile touch input
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isTouching = true;
                    touchStartPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    // Handle touch drag/move
                    Vector2 touchDelta = touch.position - touchStartPosition;
                    // Implement your logic for touch drag here
                    break;

                case TouchPhase.Ended:
                    isTouching = false;
                    // Handle touch release
                    // Implement your logic for touch release here
                    break;
            }
        }
    }

    // Public method to check if the screen is currently being touched
    public bool IsTouching()
    {
        return isTouching;
    }

    // Public method to get the touch start position
    public Vector2 GetTouchStartPosition()
    {
        return touchStartPosition;
    }
    public void HandleInput(GameState currentState)
    {


    }
}

public class GameManager:MonoBehaviour
{
  public void UpdateGame(GameState currentState)
    {

    }
}

public enum GameState
{
    MainMenu,
    Start,
    Playing,
    Finish,
    Paused
}
