using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameLoopManager.GameStates+= OnGameOver;
    }
    private void OnDestroy()
    {
        GameLoopManager.GameStates -= OnGameOver;
    }
    void OnGameOver(GameState gameState)
    {

    }
    // Update is called once per frame
    
}
