using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static bool paused=false;
    public static bool frogShield=false;
    public static bool pulsatingTouch=false;
    public static bool doubleScore = false;

    public static Action OnDoubleScore;
    public static Action OnFrogShield;
    public Image nightPanel;
    public Image fadePanel;

    public static Action<int> buffTrigger; // --> Used in buffmanager.cs
    public static GameManager instance;
    [SerializeField] private TextMeshProUGUI Score;
    int score = 0;
    public Sprite s_frog;
    public Sprite s_buffFrog;
    [SerializeField] private TextMeshProUGUI timerText;

    private int timeSelectorIndex;
    public float _gameTime=30;
    [SerializeField] private RectTransform maxBound;
    
    List<frogObject> frogs;
  
    float timeValue=0f;
    [SerializeField] private RectTransform minimumDistancePoint;
    float distance;
    float totalDistance=545;
    float meterDistance=6;
    float adjustedDistance;
    List<RectTransform> oldPosY = new List<RectTransform>();
    List<RectTransform> frogPositions = new List<RectTransform>();
 
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 3f;
    public int frogCount = 10;
    public float timeBetweenFrogSpawns = 0.5f;

    private bool isGameRunning = true;
    public bool spawnFrogs;

    [Header ("Game Settings")]
    public bool scaleByDistance;
    public bool movement;
    public bool timer=true;

    public static Action BuffFrogClicked;


    [Header("Frog Animation Parameters")]
    public float jumpSpeed=1f;
    public float jumpAmount = 1f;
    public float lilyPadSpeed=1f;

    //pixels per second
    public float _frogspeed = 640f;    // in km/h

    public float Frogspeed
    {
        get { return _frogspeed; }
        set { _frogspeed = value; }
    }
    public int scoreAmount=1;
    public float frogSpawnFrequency=1;

    private void Reset()
    {
        
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void OnDestroy()
    {
        Settings.OnTimeSelectorIndex -= OnTimeSelectorIndex;
        GameLoopManager.GameStates -= OnGameState;
        BuffFrogClicked -= OnBuffFrogClicked;
        OnDoubleScore -= DoubleScore;
        DOTween.KillAll();

    }
    private void Start()
    {
        Settings.OnTimeSelectorIndex += OnTimeSelectorIndex;
        GameLoopManager.GameStates += OnGameState;
        BuffFrogClicked += OnBuffFrogClicked;
        OnDoubleScore += DoubleScore;

        frogs = FrogFactory.instance.InstansiateFrogs(frogCount);

        for (int i = 0; i < frogs.Count; i++)
            frogPositions.Add(frogs[i].rectTransform);

        foreach (var frog in frogs)
            if (scaleByDistance)
            {

                distance = Mathf.Abs(frog.rectTransform.anchoredPosition.y);
                adjustedDistance = (distance / totalDistance);
                frog.rectTransform.localScale = new Vector3(frog.rectTransform.localScale.x * adjustedDistance, frog.rectTransform.localScale.y * adjustedDistance, frog.rectTransform.localScale.z);

            }
        frogs.Sort((a, b) => b.rectTransform.anchoredPosition.y.CompareTo(a.rectTransform.anchoredPosition.y));
        for (int i = 0; i < frogs.Count; i++)
        {
            frogs[i].rectTransform.gameObject.transform.SetSiblingIndex(i);

        }

        foreach (var frog in frogs)

        {   frog.sequence = DOTween.Sequence();
            //frog.frogRectTransform.DOAnchorPosY(frog.frogRectTransform.anchoredPosition.y - 20, 0.10f).SetLoops(1,LoopType.Yoyo).SetEase(Ease.InOutSine);
            frog.tweenerInDown = frog.frogRectTransform.DOAnchorMax(new Vector2(1,  0.5f), 0.50f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                if (frog.frogScript.clicked)
                {
                    frog.sequence.Pause();
                }
            }); 
            frog.tweenerInUp = frog.frogRectTransform.DOAnchorMax(new Vector2(1, 1f), 0.33f).SetEase(Ease.InSine).OnComplete(() =>
            {
                if (frog.frogScript.clicked)
                {
                    frog.sequence.Pause();
                }
            }); 
            frog.tweenerUp = frog.frogRectTransform.DOAnchorPosY(frog.frogRectTransform.anchoredPosition.y + 25, 0.5f).SetEase(Ease.OutSine);
            frog.tweenerDown = frog.frogRectTransform.DOAnchorPosY(frog.frogRectTransform.anchoredPosition.y, 0.5f).SetEase(Ease.InSine).OnComplete(() =>
            {
                if(frog.frogScript.clicked)
                {
                    frog.sequence.Pause();
                }
            });
            
           
            frog.sequence.Append(frog.tweenerInDown);
            frog.sequence.Append(frog.tweenerInUp);
            frog.sequence.Append(frog.tweenerUp);
            frog.sequence.Append(frog.tweenerDown);
      
            frog.sequence.SetLoops(-1);
            frog.sequence.Pause();



            frog.LillypadSequence = DOTween.Sequence();
            frog.LillypadDown = frog.rectTransform.DOAnchorPosY(frog.rectTransform.anchoredPosition.y - 5f, 0.915f);
            frog.LillypadUp = frog.rectTransform.DOAnchorPosY(frog.rectTransform.anchoredPosition.y + 5f, 0.915f);
           
            frog.LillypadSequence.Append(frog.LillypadUp);
            frog.LillypadSequence.Append(frog.LillypadDown);
            //frog.LillypadSequence.Append(frog.rectTransform.DOAnchorPosY(frog.rectTransform.anchoredPosition.y , 0.915f););
            frog.LillypadSequence.SetLoops(-1);
            frog.LillypadSequence.Pause();
         
        }
    }
    
    private void OnPulsatingTouch()
    {

    }
    async private void OnBuffFrogClicked()
    {
        TriggerBuffEffect();
       await Awaitable.WaitForSecondsAsync(5f, destroyCancellationToken);
        
        GenerateNewBuffFrog();
    }

    private void TriggerBuffEffect()
    {
        int buffId = UnityEngine.Random.Range(0, 8);

        StartBuff(buffId);
        Score.text = score.ToString();
    }

    private void StartBuff(int buffId)
    {
        buffTrigger?.Invoke(buffId);
       

       
    }

    private void GenerateNewBuffFrog()
    {
        
        for(int i =0;i < frogs.Count;i++)
        {
            var rndm = UnityEngine.Random.Range(0, frogs.Count);
            if (!frogs[rndm].isActive)
            {
               
                frogs[rndm].frogScript.IsBuffFrog = true;
                frogs[rndm].frogScript.image.sprite = s_buffFrog;
                break;
            }
        }

    }
    public void FrogShield()
    {
        if (frogShield == true)
        {
            for (int i = 0; i < frogs.Count - 1; i++)
            {

               
            }

        }
        else if (frogShield == false)
        {
            for (int i = 0; i < frogs.Count - 1; i++)
            {
                //   frogs[i].frogScript.ChangeToGreenFrog();

            }
        }
    }
    public void DoubleScore()
    {
        if (doubleScore == true)
        {
            for(int i=0;i< frogs.Count - 1;i++)
            {

           // frogs[i].frogScript.ChangeToGoldenFrog();
            }

        }
        else if (doubleScore == false)
        {
            for (int i = 0; i < frogs.Count - 1; i++)
            {
             //   frogs[i].frogScript.ChangeToGreenFrog();
                
            }
        }

    }



    [BurstCompile]
    public void UpdateGame(GameState currentState)
    { 
    if (currentState == GameState.Playing&&spawnFrogs==true)
    {   
    
        for (int i=0; i<frogs.Count;i++)
        {
            if (frogs[i].isActive)
            {
                if (scaleByDistance)
                {
                    distance = Mathf.Abs(frogs[i].rectTransform.anchoredPosition.y);
                    adjustedDistance = (distance / totalDistance);
                    frogs[i].rectTransform.localScale = new Vector3(1.5f * adjustedDistance, 1.5f * adjustedDistance, frogs[i].rectTransform.localScale.z);
                }
                if (movement == true)
                {
                        // 1 pixel per second * time.deltaTime*speed
                        frogs[i].rectTransform.anchoredPosition += new Vector2(1f, 0) * Time.deltaTime  * Frogspeed;
                    frogs[i].rectTransform.localPosition = new UnityEngine.Vector3(frogs[i].rectTransform.localPosition.x, frogs[i].rectTransform.localPosition.y, 1);
                }
                if (frogs[i].frogScript.clicked)
                {
                    frogs[i].isAnimating = false;
               
                        if (pulsatingTouch)
                        {
                            //GetNeighbourFrog(2);
                            //if (frogs[i])
                            //frogs[i].rectTransform.anchoredPosition.y
                            //     frogs[i].rectTransform.anchoredPosition.x
                       
                        }

               
               
                }
               
                if (frogs[i].rectTransform.anchoredPosition.x > maxBound.anchoredPosition.x)
                {
                       
                            frogs[i].isActive = false;
                        frogs[i].frogScript.transform.DOKill();
                        frogs[i].frogScript.image.DOKill();
                            frogs[i].rectTransform.anchoredPosition = new UnityEngine.Vector3(0, frogs[i].rectTransform.anchoredPosition.y, 1);
                            frogs[i].frogScript.Reset();
                        frogs[i].frogScript.isOutsideBounds = true;
                        frogs[i].frogScript.image.sprite = s_frog;
                            frogs[i].frogScript.IsBuffFrog = false;
                            frogs[i].frogScript.clicked = false;

                        
                    

                        if (frogs[i].isAnimating)
                        {
                            frogs[i].isAnimating = false;
                            frogs[i].LillypadSequence.Restart();
                            frogs[i].LillypadSequence.Pause();
                            frogs[i].sequence.Restart();
                            frogs[i].sequence.Pause();

                        }
             
                       
                }
            }

        }
    }
    }

    public void pauseAnimations()
    {
       
    }
    async public void StartPulsatingTouch(GameObject frogObj)
    {
        var frogPos = frogObj.GetComponent<RectTransform>();
     
        List<frogObject> neighbours = new List<frogObject>();
        List<frogObject> larger = new List<frogObject>();
        List<frogObject> lesser = new List<frogObject>();

        for (int i =0; i<frogs.Count; i++)
        {
            if (frogs[i].isActive && frogs[i].frogScript.clicked==false)
            {
                if (frogs[i].rectTransform.position.x >minimumDistancePoint.position.x)
                {
                    neighbours.Add(frogs[i]);
                }
               
            }
        }
        
        neighbours.Sort((a, b) => b.rectTransform.position.x.CompareTo(a.rectTransform.position.x));

        for(int i =0; i<neighbours.Count;i++)
        {
            if (neighbours[i].rectTransform.position.x>frogPos.position.x)
            {
                larger.Add(neighbours[i]);
            }
            else
            {
                lesser.Add(neighbours[i]);
            }
        }
        // for (int i = 0; i < neighbours.Count; i++)
        //     {
        //         neighbours[i].frogScript.onClicked();
        //     await Awaitable.WaitForSecondsAsync(0.1f, destroyCancellationToken);
        //     }
        larger.Sort((a, b) => a.rectTransform.position.x.CompareTo(b.rectTransform.position.x));
        //lesser.Sort((a, b) => a.rectTransform.anchoredPosition.x.CompareTo(b.rectTransform.anchoredPosition.x));

        var count= (larger.Count>lesser.Count)? larger.Count: lesser.Count;
        for(int i = 0; i< count; i++)
        {
            if(i<larger.Count)
            {
                larger[i].frogScript.onClicked();

            }
            if (i<lesser.Count)
            {
                lesser[i].frogScript.onClicked();

            }
            await Awaitable.WaitForSecondsAsync(0.1f, destroyCancellationToken);
        }
        






        //-155.7142857142857f, -311.4285714285714f, -467.1428571428571f,-622.8571428571428f
    }
    private void FixedUpdate()
    {

        if (spawnFrogs == true)
        {

            for (int i = 0; i < frogs.Count; i++)
            {
                if (frogs[i].isActive)
                {
                    // frogs[i].frogRectTransform.localScale = new UnityEngine.Vector3(1f, frogs[i].frogRectTransform.localScale.y+((Mathf.Sin(Time.time * frogs[i].speed * 0.0001f )+1) * 0.5f), frogs[i].frogRectTransform.localScale.z);
                    // frogs[i].frogRectTransform.offsetMax -=  new UnityEngine.Vector2(0, (((Mathf.Sin(Time.time )-1)+1/2)) );
                    // //frogs[i].frogRectTransform.offsetMin -= new UnityEngine.Vector2(0, ((Mathf.Sin(Time.time * frogs[i].speed * 0.0001f) - 1) + 1) / 2);
                    // frogs[i].frogRectTransform.anchoredPosition -= new UnityEngine.Vector2(0, (((Mathf.Sin(Time.time * jumpSpeed) -1)+1 / 2 )  ) );
                    //                                                                             //(((Mathf.Sin(Time.time * frogs[i].speed * 0.0001f) - 1) + 1 / 2) * 3
                    // frogs[i].rectTransform.anchoredPosition -= new UnityEngine.Vector2(0, ((Mathf.Sin(Time.time * lilyPadSpeed) + 1) - 1) );
                    // Create a sequence for moving up


                 //   Sequence animSequenceUp = DOTween.Sequence();
                 //  frogs[i].frogRectTransform.DOAnchorPosY(frogs[i].frogRectTransform.anchoredPosition.y + 1f, 0.2f).OnComplete(() =>
                 //   {
                 //       frogs[i].frogRectTransform.DOAnchorPosY(frogs[i].frogRectTransform.anchoredPosition.y - 1f, 0.2f);
                 //
                 //   });

                    

                     //Create a sequence for moving down
                     //Sequence animSequenceDown = DOTween.Sequence();
                     //animSequenceDown.Append(frogs[i].frogRectTransform.DOAnchorPosY(frogs[i].rectTransform.anchoredPosition.y, 0.5f)).SetDelay(0.50f);

                    // Play both sequences
                
                  

                    //   animSequenceDown.Play();

                    // frogs[i].rectTransform.DOAnchorPosY(frogs[i].rectTransform.anchoredPosition.y + 0.1f, 0.1f);

                }

            }

        }
    }
    private void OnGameState( GameState gameState)
    {
        if(gameState==GameState.Start)
        {
        
        }
    if(gameState==GameState.Playing)
    {
        

            score= 0;
            spawnFrogs = true;
         ActivateFrog();

            GenerateNewBuffFrog();
            GenerateNewBuffFrog();
            GenerateNewBuffFrog();

            if (timer==true)
         StartTimer();
    }
   
    }
    async void ActivateFrog()
    {
        List<frogObject> frogsCpy = new List<frogObject>(frogs);
        
        int n = frogs.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            frogObject value = frogsCpy[k];
            frogsCpy[k] = frogsCpy[n];
            frogsCpy[n] = value;
        }
        int i=0;
        try
        {

            while (!destroyCancellationToken.IsCancellationRequested&& spawnFrogs)
            {

              

                if (frogsCpy[i].isActive == false)
                    {
                         frogsCpy[i].isActive=true;
                    frogsCpy[i].isAnimating = true;
                    frogsCpy[i].LillypadSequence.Play();
                    frogsCpy[i].sequence.Play();
                       
                    }
                
                
      

                await Awaitable.WaitForSecondsAsync(timeBetweenFrogSpawns*frogSpawnFrequency, destroyCancellationToken);
                i++;
                if (i >= frogs.Count)
                {
                    i = 0;
                }
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("destroyCancellationToken was cancelled");
        }
    }
    async public void StartTimer()
    {
        while(!destroyCancellationToken.IsCancellationRequested&&spawnFrogs)
        {
            
            if (_gameTime > 0f)
            {
               
                _gameTime -= 1f;
                    if (_gameTime <= 0f)
                    {
                        EndGame();

                    }
                    // Update the UI Text component to display the remaining time
                    UpdateTimerDisplay();

                
                await Awaitable.WaitForSecondsAsync(1f, destroyCancellationToken);
            }
            
        }
   
    }
    public void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = FormatTime(_gameTime);
        }
    }
    string FormatTime(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60);  // Beräkna antalet minuter
        int seconds = Mathf.FloorToInt(totalSeconds % 60);  // Räkna ut antalet sekunder

        // Returnera strängen i formatet "MM:SS", där sekunder alltid visas med två siffror
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void EndGame()
    {
        
        _gameTime = 0;
        // GameLoopManager.instance.ChangeState(GameState.MainMenu);
        foreach (var frog in frogs)
        {
            frog.rectTransform.anchoredPosition = new UnityEngine.Vector3(0, frog.rectTransform.anchoredPosition.y, 1);
            frog.isActive = false;
            frog.frogRectTransform.DOScale(1.0f, 1.0f);
            frog.frogScript.image.DOFade(1.0f, 1.0f);
            frog.frogScript.clicked = false;
            frog.frogScript.IsBuffFrog = false;
            frog.frogScript.Reset();
            frog.frogScript.image.sprite = s_frog;
           

        }
        switch (GameLoopManager.progress._timeLimitIndex)
        {
            case 0:

                if (score > GameLoopManager.progress.Highscore30)
                {
                    UIManager.isHighScore = true;
                    GameLoopManager.progress.Highscore30 = score;
                }
                break;
                case 1:
                if (score > GameLoopManager.progress.Highscore60)
                {
                    GameLoopManager.progress.Highscore60 = score;
                    UIManager.isHighScore = true;
                }
                break;
                case 2:
                if (score > GameLoopManager.progress.Highscore90)
                {
                    GameLoopManager.progress.Highscore90 = score;
                    UIManager.isHighScore = true;
                }
                break;
                case 3:
                if (score > GameLoopManager.progress.Highscore120)
                {
                    GameLoopManager.progress.Highscore120 = score;
                    UIManager.isHighScore = true;
                }
                break;

        }
        GameLoopManager.progress._score = score;
        ProgressManager.SaveGameProgress("savedProgress", GameLoopManager.progress);
        score = 0;
        Score.text = score.ToString();

        BuffManager.instance.turnOffAllBuffs();
        BuffManager.instance.Reset();
        spawnFrogs = false;
        GameLoopManager.GameStates?.Invoke(GameState.GameOver);


    }
    public void resetPause()
    {
        _gameTime = 0;
        // GameLoopManager.instance.ChangeState(GameState.MainMenu);
        foreach (var frog in frogs)
        {
            frog.rectTransform.anchoredPosition = new UnityEngine.Vector3(0, frog.rectTransform.anchoredPosition.y, 1);
            frog.isActive = false;
            frog.frogScript.image.sprite = s_frog;
            frog.frogScript.clicked = false;
            frog.frogScript.IsBuffFrog = false;
            frog.frogScript.Reset();
            frog.frogScript.image.sprite = s_frog;


        }
        score = 0;
        Score.text = score.ToString();

        BuffManager.instance.Reset();
        spawnFrogs = false;

        GameLoopManager.GameStates?.Invoke(GameState.MainMenu);


    }


    private void OnTimeSelectorIndex(int index)
    {
        int[] gameTimes = { 30, 60, 90, 120 };
        if (index >= 0 && index < gameTimes.Length)
        {
            _gameTime = gameTimes[index];
        }
    }
    public void addScore()
    {
        score= score + scoreAmount;
        Score.text = score.ToString();
    }




}
