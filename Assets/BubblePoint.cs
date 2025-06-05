using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BubbleSpawner : MonoBehaviour
{
    public Image bubblePrefab; // Reference to the bubble prefab (UI Image)
    public Sprite bubbleSprite; // Sprite for the bubble
    public float spawnInterval = 1f; // Time interval between spawning bubbles
    public float bubbleMoveDuration = 2f; // Duration for bubble movement from bottom to top
    public float bubbleMovepos = 2f;
    // Duration for bubble size increase
    // Delay before destroying the bubble after it's created

    bool isSpawning = false; // Flag to check if the bubbles are spawning
    private void Start()
    {
        // Start spawning bubbles
     
    }

    private void SpawnBubble()
    {
        if(!isSpawning)
        {
            CancelInvoke("SpawnBubble");
            return;
        }
        // Instantiate a new bubble and set its sprite
        Image bubble = Instantiate(bubblePrefab, transform);
        bubble.sprite = bubbleSprite;

        // Set the initial position of the bubble at the bottom of the screen
        RectTransform rectTransform = bubble.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(Random.Range(-200f, 200f), -100f);

        // Apply DoTween animations
        rectTransform.DOScale(new Vector3(2f, 2f, 2f), bubbleMoveDuration).SetEase(Ease.Linear);
       
        rectTransform.DOAnchorPosY(bubbleMovepos, bubbleMoveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(bubble.gameObject);

        });

        // Destroy the bubble after a delay
       
    }
    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void StartSpawning()
    {
      
        isSpawning = true;
        InvokeRepeating("SpawnBubble", 0f, spawnInterval);
    }
}


