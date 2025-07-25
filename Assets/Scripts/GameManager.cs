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
using Unity.VisualScripting.Antlr3.Runtime;

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
  

    [SerializeField] private RectTransform minimumDistancePoint;
    float distance;
    float totalDistance=545;
 
    float adjustedDistance;
    List<RectTransform> oldPosY = new List<RectTransform>();
    List<RectTransform> frogPositions = new List<RectTransform>();
 
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 3f;
    public int frogCount = 10;
    public float timeBetweenFrogSpawns = 0.5f;

    GameState _gameState;

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
    public float _frogspeed = 640f;    

    public float Frogspeed
    {
        get { return _frogspeed; }
        set { _frogspeed = value; }
    }
    public int scoreAmount=1;
    public float frogSpawnFrequency=1;
    private CancellationTokenSource destroyCancellationTokenGM;

    private void Reset()
    {
        
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Initialize the cancellation token source
            destroyCancellationTokenGM = new CancellationTokenSource();
        }
        else
        {
            Destroy(this);
        }
        Settings.OnTimeSelectorIndex += OnTimeSelectorIndex;
        GameLoopManager.GameStates += OnGameState;
        BuffFrogClicked += OnBuffFrogClicked;
        OnDoubleScore += DoubleScore;

        frogs = FrogFactory.instance.InstansiateFrogs(frogCount);
        List<frogObject> frogsCpyOriginal = new List<frogObject>(frogs);

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
        {
            float originalY = frog.rectTransform.anchoredPosition.y;
            frog.sequence = DOTween.Sequence();

            frog.tweenerInDown = frog.frogRectTransform.DOAnchorMax(new Vector2(1, 0.5f), 0.50f).SetEase(Ease.OutSine).OnComplete(() =>
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
                if (frog.frogScript.clicked)
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

            // Define the three tweens
            Tween up = frog.rectTransform.DOAnchorPosY(originalY + 10f, 1.5f);
            Tween down = frog.rectTransform.DOAnchorPosY(originalY - 10f, 1.5f);
            Tween reset = frog.rectTransform.DOAnchorPosY(originalY, 1.5f);

            // Create the sequence
            // frog.LillypadSequence = DOTween.Sequence();
            // frog.LillypadSequence.Append(down);
            // frog.LillypadSequence.Append(reset);
            // frog.LillypadSequence.Append(up);
            // frog.LillypadSequence.SetLoops(-1);
            // frog.LillypadSequence.Pause();
        }
    }
    
    private void OnDestroy()
    {
        Settings.OnTimeSelectorIndex -= OnTimeSelectorIndex;
        GameLoopManager.GameStates -= OnGameState;
        BuffFrogClicked -= OnBuffFrogClicked;
        OnDoubleScore -= DoubleScore;
        
        // Cancel all async operations before destroying
        destroyCancellationTokenGM?.Cancel();
        destroyCancellationTokenGM?.Dispose();
        
        DOTween.KillAll();
    }
    
    private void Start()
    {
       
    }

    // Cancel current operations and create new cancellation token for new game iteration
    private void ResetCancellationToken()
    {
        destroyCancellationTokenGM?.Cancel();
        destroyCancellationTokenGM?.Dispose();
        destroyCancellationTokenGM = new CancellationTokenSource();
    }

    async private void OnBuffFrogClicked()
    {
        // Do not trigger a buff if the game is no longer in the 'Playing' state.
        if (_gameState != GameState.Playing)
        {
            return;
        }
        TriggerBuffEffect();
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
            }
        }
    }
    
    public void DoubleScore()
    {
        if (doubleScore == true)
        {
            for(int i=0;i< frogs.Count - 1;i++)
            {
            }
        }
        else if (doubleScore == false)
        {
            for (int i = 0; i < frogs.Count - 1; i++)
            {
            }
        }
    }



    public void UpdateGame(GameState currentState)
    {
        if(_gameState==GameState.Playing)
        { 
            for (int i = 0; i < frogs.Count; i++)
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
                        frogs[i].rectTransform.anchoredPosition += new Vector2(1f, 0) * Time.deltaTime * Frogspeed;
                        frogs[i].rectTransform.localPosition = new UnityEngine.Vector3(frogs[i].rectTransform.localPosition.x, frogs[i].rectTransform.localPosition.y, 1);
                    }
                    
                    if (frogs[i].frogScript.clicked)
                    {
                        frogs[i].isAnimating = false;

                        if (pulsatingTouch)
                        {
                        }
                    }

                    if (frogs[i].rectTransform.anchoredPosition.x > maxBound.anchoredPosition.x)
                    {
                        frogs[i].isActive = false;
                        frogs[i].rectTransform.anchoredPosition = new UnityEngine.Vector3(0, frogs[i].rectTransform.anchoredPosition.y, 1);

                        if (frogs[i].frogScript.IsBuffFrog == true)
                        {
                            if (frogs[i].frogScript.swap)
                            {
                                frogs[i].frogScript.swap = false;
                                GenerateNewBuffFrog();
                            }
                            frogs[i].frogScript.ChangeToNormalFrog();
                        }
                        else if (frogs[i].frogScript.IsBuffFrog != true)
                        {
                            frogs[i].frogScript.ChangeToNormalFrog();
                        }
                        
                        frogs[i].frogScript.Reset();

                        if (frogs[i].isAnimating)
                        {
                            frogs[i].isAnimating = false;
                         //  frogs[i].LillypadSequence.Restart();
                         //  frogs[i].LillypadSequence.Pause();
                            frogs[i].sequence.Restart();
                            frogs[i].sequence.Pause();
                        }
                     
                    }
                }
            }
        }
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

        larger.Sort((a, b) => a.rectTransform.position.x.CompareTo(b.rectTransform.position.x));

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
            await Awaitable.WaitForSecondsAsync(0.1f);
        }
    }

 async private void OnGameState(GameState gameState)
    {
        _gameState = gameState;

        if (_gameState == GameState.Start)
        {
            score = 0;
            // Reset cancellation token for new game iteration
            ResetCancellationToken();
        }
        
        if (_gameState == GameState.Playing)
        {
            _frogspeed = 640;
            ActivateFrog();
            GenerateNewBuffFrog();
            GenerateNewBuffFrog();
            GenerateNewBuffFrog();
            StartTimer();
        }
        
        if(_gameState==GameState.GameOver)
        {
            // Centralize all game over logic into a coroutine to prevent race conditions
            StartCoroutine(GameOverCleanupCoroutine());
        }
    }

    private void GenerateNewBuffFrog()
    {
        for (int i = 0; i < frogs.Count; i++)
        {
            var rndm = UnityEngine.Random.Range(0, frogs.Count);
            if (!frogs[rndm].isActive && frogs[rndm].frogScript.IsBuffFrog!=true)
            {
                frogs[rndm].frogScript.IsBuffFrog = true;
                frogs[rndm].frogScript.ChangeToBuffFrog();
                break;
            }
        }
    }

    async void ActivateFrog()
    {
        List<frogObject> frogsCpy = new List<frogObject>(frogs);
        // Shuffle the frog list
        int n = frogs.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            frogObject value = frogsCpy[k];
            frogsCpy[k] = frogsCpy[n];
            frogsCpy[n] = value;
        }
        
        int i = 0;
        try
        {
            while (_gameState == GameState.Playing && !destroyCancellationTokenGM.Token.IsCancellationRequested)
            {
                if (!frogsCpy[i].isActive)
                {
                    frogsCpy[i].isActive = true;
                    frogsCpy[i].isAnimating = true;
                   // frogsCpy[i].LillypadSequence.Play();
                    frogsCpy[i].sequence.Play();
              
                    await Awaitable.WaitForSecondsAsync(0.5f, destroyCancellationTokenGM.Token);
                }

                i++;
                if (i >= frogs.Count)
                {
                    i = 0;
                }
            }
        }
        catch (OperationCanceledException)
        {
            // The exception is expected on cancellation.
            // The FinalFrogResetCoroutine will handle all cleanup.
 
        }
        finally
        {
            frogsCpy.Clear();
        }
    }

    async public void StartTimer()
    {
        try
        {
            while (_gameTime > 0f && _gameState == GameState.Playing && !destroyCancellationTokenGM.Token.IsCancellationRequested)
            {
                _gameTime -= 1f;

                if (_gameTime <= 0f)
                {
                    await Awaitable.WaitForSecondsAsync(0.5f, destroyCancellationTokenGM.Token);
                    if (_gameState != GameState.GameOver)
                        GameLoopManager.GameStates?.Invoke(GameState.GameOver);
                }

                UpdateTimerDisplay();
                await Awaitable.WaitForSecondsAsync(1f, destroyCancellationTokenGM.Token);
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when cancellation is requested
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
        int minutes = Mathf.FloorToInt(totalSeconds / 60);  
        int seconds = Mathf.FloorToInt(totalSeconds % 60);  
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void resetPause()
    {
        _gameTime = 0;
        GameLoopManager.GameStates?.Invoke(GameState.GameOver);
        
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

    private IEnumerator GameOverCleanupCoroutine()
    {
        // 1. Cancel all async tasks like ActivateFrog and StartTimer
        destroyCancellationTokenGM?.Cancel();
        _gameTime = 0;

        // 2. Wait for the end of the frame. This is crucial to let the cancellation propagate
        // and stop the ActivateFrog loop before we proceed with cleanup.
        yield return new WaitForEndOfFrame();

        // 3. Now, perform the definitive reset on all frogs.
        foreach (var frog in frogs)
        {
            frog.isActive = false;
            frog.isAnimating = false;
            frog.rectTransform.anchoredPosition = new UnityEngine.Vector3(0, frog.rectTransform.anchoredPosition.y, 1);
            frog.frogScript.transform.DOKill();
            frog.frogScript.image.DOKill();
            frog.frogScript.maxTouches= 1;

           // frog.LillypadSequence.Restart();
           // frog.LillypadSequence.Pause();
            frog.sequence.Restart();
            frog.sequence.Pause();
            
            frog.frogScript.Reset();
            frog.frogScript.ChangeToNormalFrog();
        }

        // 4. Handle scoring and highscore saving
        if (paused != true)
        {
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
        }

        // 5. Wait a short moment before clearing buffs, as you requested.
        // This ensures any last-second click events are processed safely.
        yield return new WaitForSeconds(0.2f);
        
        // 6. Now clear the buffs
        
            BuffManager.instance.turnOffAllBuffs();
            BuffManager.instance.Reset();
           
        

        // 7. Finally, transition to the highscore screen
        if (paused != true)
        {
            GameLoopManager.GameStates?.Invoke(GameState.Highscore);
        }

        // 8. Reset score and pause state for the next round
        score = 0;
        Score.text = score.ToString();
        if (paused) { BuffManager.instance.buffsUsed.Clear(); }
        paused = false;
    }
}
