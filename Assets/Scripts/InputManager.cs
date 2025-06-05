using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
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