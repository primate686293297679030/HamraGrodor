using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
using System;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<RectTransform> UIButtonsList;
    private float minWaitTime = 5f; // Minimum wait time in seconds
    private float maxWaitTime = 15f; // Maximum wait time in seconds
    private float buttonIndex = 0;

    float xPos;
    float yPos;
    float zRot;
    public static bool isHighScore = false;

    public Vector3 targetRotation = new Vector3(90f, 0f, 180f); // The target rotation in degrees
    public Vector3 targetScale = new Vector3(2f, 2f, 2f); // The target scale

    public float shakeStrength = 10f; // The strength of the shake effect
    public float duration = 1f; // Duration of the animations

    

    int randomIndex;
    [Header("ScorePage")]
    public List<RectTransform> HighscoreObjects;
    public List<Sprite> scorePageImages;
    public TextMeshProUGUI scorePageNumber;
    public Button highscorePageBackButton;
    public Image scorePageBackground;
    public Image timeLimitImage;
    public List<Sprite> timeLimitImages;
    [SerializeField] private GameObject scorePage;

    List<Image> i_scorePageImages=new List<Image>();
    public List<Image> usedBuffImages;

    private bool isCountdownRunning = false;
    public static UIManager instance;

    RectTransform scorepageTransform;

    public Sprite transparentSprite;
    [SerializeField]
    private GameObject _pauseButton;
    [SerializeField]
    private Image _pauseButtonImage;
    public Image BlurPanel;
    [SerializeField] private Image Timer;
    [SerializeField] private Image FrogCounter;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _frogCounter;


    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
      
        scorepageTransform=scorePageBackground.gameObject.GetComponent<RectTransform>();
        for (int i = 0; i < scorePageBackground.gameObject.transform.childCount; i++)
        {
            Image img = scorePageBackground.gameObject.transform.GetChild(i).GetComponent<Image>();
            if (img != null)
            {
                img.DOFade(0, 0);
                i_scorePageImages.Add(img);
            }
       
           
        }
        scorePageBackground.DOFade(0, 1);

        GameLoopManager.GameStates += OnGameState;
    }
    private void OnDestroy()
    {
        GameLoopManager.GameStates -= OnGameState;
    
    }
    async private void OnGameState(GameState state)
    {
        if(state==GameState.MainMenu)
        {
           GetComponent<Canvas>().sortingOrder=-1;
             OnMainMenuAnimation(state);
          
        }
        else if(state==GameState.Playing)
        {
         

        }


        if(state== GameState.GameOver)
        {
          //  BlurPanel.gameObject.SetActive(true);
            //BlurPanel.DOFade(0.43f, 1).SetEase(Ease.InOutSine);

            Timer.DOFade(0, 0.5f);
            FrogCounter.DOFade(0, 0.5f);
            _timer.DOFade(0, 0.5f);
            _frogCounter.DOFade(0, 0.5f);

            switch (GameLoopManager.progress._timeLimitIndex)
            {
                case 0:
                    timeLimitImage.sprite = timeLimitImages[0];
                    UpdateScorePage(GameLoopManager.progress.Highscore30, isHighScore);
                    

                    break;
                case 1:
                    timeLimitImage.sprite = timeLimitImages[1];
                    UpdateScorePage(GameLoopManager.progress.Highscore60, isHighScore);
                    break;
                case 2:
                    
                    timeLimitImage.sprite = timeLimitImages[2];
                    UpdateScorePage(GameLoopManager.progress.Highscore90, isHighScore);
                    break;
                case 3:
                    timeLimitImage.sprite = timeLimitImages[3];
                    UpdateScorePage(GameLoopManager.progress.Highscore120, isHighScore);
                    break;
            }
            for(int i =0; i< BuffManager.buffsUsed.Count;i++)
            {
                usedBuffImages[i].sprite = BuffManager.buffsUsed[i];
            }
            BuffManager.buffsUsed.Clear();
            
        }
    }
    void UpdateScorePage(int score, bool isHighScore)
    {

        scorePageNumber.text = GameLoopManager.progress._score.ToString();

        scorePage.gameObject.SetActive(true);
        scorepageTransform.DOAnchorPosY(52f, 1).SetEase(Ease.OutBack);

        if (isHighScore)
        {
            scorePageBackground.sprite = scorePageImages[1];
        }
        else
        {
            scorePageBackground.sprite = scorePageImages[0];
        }

      for(int i=0; i < i_scorePageImages.Count;i++)
        {


            i_scorePageImages[i].DOFade(1, 2);
            
            
        }
        scorePageBackground.DOFade(1, 2).OnComplete(() =>{
            if (isHighScore)
            {
                HighscoreObjects[0].DOScale(Vector3.one, 1.5f).SetEase(Ease.OutBack);
                HighscoreObjects[1].DOScale(Vector3.one, 1.5f).SetEase(Ease.OutBack);

            }
        });

        
            
       
       


        highscorePageBackButton.gameObject.SetActive(true);
        highscorePageBackButton.image.DOFade(1, 2);
        _pauseButtonImage.DOFade(0, 1).OnComplete(() =>
            { _pauseButton.gameObject.SetActive(false); });
        



    }
    public void UnloadScorePage()
    {
        BlurPanel.DOFade(0, 1).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            BlurPanel.gameObject.SetActive(false);
        });
     
        isHighScore = false;
       // scorePage.transform.DOScale(new Vector3(0.0f, 0.0f, 1), 1).SetEase(Ease.OutFlash);


        for (int i = 0; i < i_scorePageImages.Count; i++)
        {


            i_scorePageImages[i].DOFade(0, 1);


        }
        scorePageBackground.DOFade(0, 1);


        highscorePageBackButton.image.DOFade(0, 0.5f).OnComplete(() =>
        {
            highscorePageBackButton.gameObject.SetActive(false);
            scorePage.gameObject.SetActive(false);
            scorepageTransform.DOAnchorPosY(1088, 1);
        });
        HighscoreObjects[0].DOScale(Vector3.zero, 0f);
        HighscoreObjects[1].DOScale(Vector3.zero, 0f);
        
        foreach(var spritething in usedBuffImages)
        {
            spritething.sprite = transparentSprite;
        }
        


    }
    async public void UpdateUI(GameState currentState)
    {

        if(currentState==GameState.MainMenu)
        {
            UIButtonsList[randomIndex].anchoredPosition = new Vector2( xPos,yPos);
            UIButtonsList[randomIndex].rotation = Quaternion.Euler(0, 0, zRot);
        }
        if (currentState == GameState.GameOver)
        {

        }



    }
 



    public async void OnMainMenuAnimation(GameState state)
    {
        while (!destroyCancellationToken.IsCancellationRequested)
        {
            if (state == GameState.MainMenu)
            {
                Debug.Log("MenuAnimation is running");
               


              randomIndex  = UnityEngine.Random.Range(0, UIButtonsList.Count);
                
                DOVirtual.Float(UIButtonsList[randomIndex].anchoredPosition.x-1, UIButtonsList[randomIndex].anchoredPosition.x+1, 0.05f, x => XAnimation(x)).SetLoops(5,LoopType.Yoyo);
                DOVirtual.Float(UIButtonsList[randomIndex].anchoredPosition.y + 1, UIButtonsList[randomIndex].anchoredPosition.y - 1, 0.05f, x => YAnimation(x)).SetLoops(5, LoopType.Yoyo);
                DOVirtual.Float(UIButtonsList[randomIndex].rotation.z-10, UIButtonsList[randomIndex].rotation.z + 10, 0.05f, z => ZAnimation(z)).SetLoops(5, LoopType.Yoyo).OnComplete(() =>
                {
                    zRot = 0;
                    xPos = UIButtonsList[randomIndex].anchoredPosition.x - 1;
                    yPos = UIButtonsList[randomIndex].anchoredPosition.y + 1;
                });

                // UIButtons[a].gameObject.transform.DOShakePosition(1, 100, 10, 90);
                // UIButtons[a].gameObject.transform.DOShakeRotation(1, 100, 10, 90);

                await Awaitable.WaitForSecondsAsync(UnityEngine.Random.Range(8, 16), destroyCancellationToken);
            }
            else
            {
                break;
            }
   
         
    
        }
    
    
    }

    private void XAnimation(float v)
    {
        xPos = v;

    }
    private void YAnimation(float v)
    {
        yPos = v;
    }
    private void ZAnimation(float v)
    {
        zRot = v;
    }




}
