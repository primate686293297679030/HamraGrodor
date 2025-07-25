using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEditor;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

[Serializable]
public class BubbleState
{
    public Vector2 initialPosition;
    public Vector3 initialScale;
}
[BurstCompile]
internal struct CubePositionJob : IJobParallelFor
{
    public NativeArray<float3> Positions;
    [ReadOnly] public NativeArray<float> YOffsets;
    public NativeArray<Matrix4x4> Matrices;
    public float Time;

    public void Execute(int index)
    {
   
    }
}

public class BubbleManager : MonoBehaviour
{
    public float animationDuration = 1f; 
    public float BubbleDelay;
    public float BubbleDelayBack;
    public float minimumSpeed;
    public float maximumSpeed;
    public float _speedY = 1544;
    public int numberOfBubbles;
    private bool isMoving = false;
 
    private RectTransform canvasRect;

    public GameObject bubblePrefab;
    public static Action finishBubbel;
    public static Action finishBubble;

    private List<RectTransform> bubblesRect = new List<RectTransform>();
    private List<BubbleState> bubbleStates = new List<BubbleState>();

    public static BubbleManager instance;

    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button settings;

    // Array to store unique random values for each instance
    List<float> randomValues;

    private void Awake()
    {
        if (instance == null)
        { instance = this; }
        else if (instance != this)
        { Destroy(gameObject);}
        
    }
    void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed
        LoadMenuOptions.settingsButton -= StartBubbleMovement;
        LoadMenuOptions.settingsBackButton -= StartBubbleMovement;
        LoadMenuOptions.settingsButton -= settingsButtonn;
        LoadMenuOptions.settingsBackButton -= settingsBackButtonn;

    }
    private void Start()
    {
        BubbleDelayBack =BubbleDelay;
        LoadMenuOptions.settingsButton += StartBubbleMovement;
        LoadMenuOptions.settingsButton += settingsButtonn;
        LoadMenuOptions.settingsBackButton += StartBubbleMovement;
        LoadMenuOptions.settingsBackButton += settingsBackButtonn;
        canvasRect = GetComponent<RectTransform>();      
        InstantiateBubbles();
   
    }
    // Function to generate a list of unique random values within a specified range
    List<float> GenerateUniqueRandoms(float minValue, float maxValue, int count)
    {
        List<float> uniqueRandoms = new List<float>();

        for (int i = 0; i < count; i++)
        {
            float randomValue;
            do
            {
                randomValue = UnityEngine.Random.Range(minValue, maxValue);
            } while (uniqueRandoms.Contains(randomValue));

            uniqueRandoms.Add(randomValue);
        }

        return uniqueRandoms;
    }
    void settingsBackButtonn()
    {
        BubbleDelay = BubbleDelayBack;
        settings.enabled = false;
        settingsBackButton.enabled = false;
    }
    void settingsButtonn()
    {
        BubbleDelay = 0.01f;
        settings.enabled = false;
        settingsBackButton.enabled = false;

    }

    private void InstantiateBubbles()
    {      
        for (int j = 0; j < numberOfBubbles; j++)
        {
            GameObject bubbleGO = Instantiate(bubblePrefab, transform);
            RectTransform bubbleRect = bubbleGO.GetComponent<RectTransform>();
            float randomX = UnityEngine.Random.Range(-canvasRect.rect.width * 0.5f, canvasRect.rect.width * 0.5f);
            float randomY = UnityEngine.Random.Range(-canvasRect.rect.height * 1.75f, -canvasRect.rect.height*0.75f);
            bubbleRect.anchoredPosition = new Vector2(randomX, randomY);
            bubbleRect.localScale = Vector3.zero;              
            float randomScale = UnityEngine.Random.Range(0.5f, 1.5f);
            bubbleRect.localScale = new Vector3(randomScale, randomScale, 1f);

            // Store the initial state of the bubble
            BubbleState bubbleState = new BubbleState
            {
                initialPosition = bubbleRect.anchoredPosition,
                initialScale = bubbleRect.localScale
            };
            bubbleStates.Add(bubbleState);
            bubblesRect.Add(bubbleRect);
        }
    
            randomValues = GenerateUniqueRandoms(minimumSpeed, maximumSpeed, bubblesRect.Count);

    }

    private void Update()
    {
        if (isMoving)
        {
            // Wrap the bubble around the screen vertically
            bool tmp = false;
            for (int i = 0; i < bubblesRect.Count; i++)
            {
                float time = (Time.time * randomValues[i]) % animationDuration; // Scale time by random value
                float v = Mathf.Lerp(-3f, 3f, Mathf.PingPong(time / animationDuration, 1f));
                sineAnim(v, i);
                MoveUpwards(randomValues[i],i);
                if (bubblesRect[i].anchoredPosition.y < canvasRect.rect.height*0.75f)
                {

                    tmp = true;
                }
            }
            if (!tmp)
            {
            settingsBackButton.enabled = true;
            settings.enabled=true;
            StopWrapping();
                for(int i= 0; i <bubblesRect.Count; i++)
                ResetBubblesToInitialPositions(i);
            }
           

        }
    }
    void sineAnim(float v, int index)
    {

        bubblesRect[index].anchoredPosition += new Vector2(v, 0) * Time.deltaTime * 100;

    }
    private void MoveUpwards(float v, int index)
    {
        // Move the bubble upwards
       
        bubblesRect[index].anchoredPosition += new Vector2(0,v) * Time.deltaTime * _speedY; 
        
    }

    public void ResetBubblesToInitialPositions(int index)
    {
        bubblesRect[index].anchoredPosition = bubbleStates[index].initialPosition;
        bubblesRect[index].localScale = bubbleStates[index].initialScale;
    }


    async void StartBubbleMovement()
    {
        await Awaitable.WaitForSecondsAsync(BubbleDelay);
        gameObject.SetActive(true);
        _speedY = 1544;
        isMoving = true;

    }

    void StopWrapping()
    {
        isMoving = false;
        _speedY = 0;
        gameObject.SetActive(false);
    }








}
