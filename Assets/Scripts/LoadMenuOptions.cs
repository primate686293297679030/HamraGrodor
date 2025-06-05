using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

public class LoadMenuOptions : MonoBehaviour
{
    public TextMeshProUGUI countdownText;

    private Coroutine countdownCoroutine;
    bool isPaused = false;

    [SerializeField] private Image _StartGame;
    [SerializeField] private Image _LeaderBoard;
    [SerializeField] private Image _About;
    [SerializeField] private Image _Settings;
    [SerializeField] private Image _logoMenu;
    [SerializeField] private Image _backbutton;

    [SerializeField] RectTransform rectTransformSettings;
    [SerializeField] RectTransform rectTransformSettings2;
    [SerializeField] RectTransform rectTransformMenu;
    [SerializeField] RectTransform rectTransformMenu2;
    [SerializeField] RectTransform rectTransformAbout;
    [SerializeField] RectTransform RTcountDownText;
    public Image StoryScreenPanel;

    [SerializeField] private Canvas gameBoard;
    [SerializeField] Canvas canvasMenu;
    public static event Action settingsButton;
    public static event Action settingsBackButton;

    public Image InGameImage;
    

    public List<TextMeshProUGUI> menuTexts;


    [SerializeField] private Image aboutimage;
    [SerializeField] private GameObject Game;
    [SerializeField] private Image settingsimage;


    [SerializeField] private Image bg;
    [SerializeField] private Image sand;
    [SerializeField] private Image shark;
    [SerializeField] private Image fish;
    [SerializeField] private TextMeshProUGUI score ;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private TextMeshProUGUI tidsgrans;
    [SerializeField] private TextMeshProUGUI sekunder;
    [SerializeField] private TextMeshProUGUI timelimitValue;
    [SerializeField] private Image seagrass;
    [SerializeField] private Image seagrass2;
    [SerializeField] private Image _GameTime;
    [SerializeField] private Image _GameTimeShadow;

    [SerializeField] private Image music;
    [SerializeField] private Image audio;

    [SerializeField] private Image increase;
    [SerializeField] private Image decrease ;

    [SerializeField] SpriteRenderer fishh;
    [SerializeField] SpriteRenderer sharkk;

    [SerializeField] private Image Timer;
    [SerializeField] private Image FrogCounter;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _frogCounter;
    [SerializeField] private Image _pauseButton;

    [SerializeField] private Image _settingsBackButton;

    [SerializeField] private GameObject _menu;


    [SerializeField] private Button play;
    [SerializeField] private Button about;
    [SerializeField] private  Button settings;
    List<Image> pausePageImages=new List<Image>();
    public TextMeshProUGUI pauseText1;
    public TextMeshProUGUI pauseText2;

    public GameObject pauseGameObject;
    Image pauseBGimage;
    public GameObject touchBlocker;

    public BubbleSpawner bubbleSpawner;
    Vector3 rectmenuScale;

   private void OnEnable()
    {
        //countdownText.DOFade(0, 1).SetEase(Ease.InSine);
    }
    private void Start()
    {
        if (pauseGameObject.TryGetComponent<Image>(out Image imgbg))
        {
            pauseBGimage = imgbg;
        }
        for (int i = 0; i < pauseGameObject.transform.childCount; i++)
        {
            Image img = pauseGameObject.transform.GetChild(i).GetComponent<Image>();
            if (img != null)
            {
                img.DOFade(0, 0);
                pausePageImages.Add(img);
            }


        }


        bg.DOFade(0, 0);
        sand.DOFade(0, 0);
        shark.DOFade(0, 0);
        fish.DOFade(0, 0);
        score.DOFade(0, 0);
        highscore.DOFade(0, 0);
        tidsgrans.DOFade(0, 0);
        timelimitValue.DOFade(0, 0);
        sekunder.DOFade(0, 0);
        seagrass.DOFade(0, 0);
        seagrass2.DOFade(0, 0);
        music.DOFade(0, 0);
        audio.DOFade(0, 0);
        _GameTimeShadow.DOFade(0, 0f);
        _GameTime.DOFade(0, 0f);
        increase.DOFade(0, 0);
        decrease.DOFade(0, 0);
        sharkk.DOFade(0, 0);
        fishh.DOFade(0, 0);
    }

    public void onGameBoardBackButton()
    {
    

        GameLoopManager.GameStates?.Invoke(GameState.MainMenu);
       
        onBackButton(0);
        _menu.SetActive(true);

    }
    public  void UnloadMenu()
    {
        var time = 0.5f;
        for (int i = 0; i < menuTexts.Count; i++)
           {
        
               menuTexts[i].DOColor(new Color(1, 1, 1, 0f), time).SetEase(Ease.InSine);
           }
           _logoMenu.DOFade(0, time).SetEase(Ease.InSine);
          _StartGame.DOFade(0, time).SetEase(Ease.InSine);
          _LeaderBoard.DOFade(0, time).SetEase(Ease.InSine);
           _About.DOFade(0, time).SetEase(Ease.InSine);
           _Settings.DOFade(0, time).SetEase(Ease.InSine).OnComplete(() => { _menu.SetActive(false); });

 


    }
    public void UnloadMenuSettings()
    {
        var time = 1;
        for (int i = 0; i < menuTexts.Count; i++)
        {

            menuTexts[i].DOColor(new Color(1, 1, 1, 0f), time).SetEase(Ease.InSine);
        }
        _logoMenu.DOFade(0, time).SetEase(Ease.InSine);
        _StartGame.DOFade(0, time).SetEase(Ease.InSine);
        _LeaderBoard.DOFade(0, time).SetEase(Ease.InSine);
        _About.DOFade(0, time).SetEase(Ease.InSine);
        _Settings.DOFade(0, time).SetEase(Ease.InSine).OnComplete(() => { _menu.SetActive(false); });

      //  var rectmenu = rectTransformMenu2.localScale;
      //  rectTransformMenu2.DOScale(new Vector3(0.0f, 0.0f, 0), 1).SetEase(Ease.OutFlash);
      //  rectTransformMenu2.DOJumpAnchorPos(new Vector2(0, 200), 100f, 1, 2.5f);
      //
      //
      //  rectmenuScale = rectmenu;


    }
    public void FadeInMenuSettings()
    {
        _menu.SetActive(true);
        _logoMenu.DOFade(1, 1.5f).SetEase(Ease.InSine);
        _StartGame.DOFade(1, 1.5f).SetEase(Ease.InSine);
        _LeaderBoard.DOFade(1, 1.5f).SetEase(Ease.InSine);
        _About.DOFade(1, 1.5f).SetEase(Ease.InSine);
        _Settings.DOFade(1, 1.5f).SetEase(Ease.InSine);
        _pauseButton.DOFade(0, 1.5f).SetEase(Ease.InSine);

        for (int i = 0; i < menuTexts.Count; i++)
        {

            menuTexts[i].DOColor(new Color(1, 1, 1, 1f), 1.5f).SetEase(Ease.InSine);
        }

       // rectTransformMenu2.DOScale(rectmenuScale, 1.5f).SetEase(Ease.InFlash);
        //rectTransformMenu2.DOJumpAnchorPos(new Vector2(0, 0), 200f, 1, 1.5f).SetEase(Ease.InFlash);
    }
    public void FadeInMenu()
    {
        var time = 0.5f;
        _menu.SetActive(true);
        _logoMenu.DOFade(1, time).SetEase(Ease.InSine);
        _StartGame.DOFade(1, time).SetEase(Ease.InSine);
        _LeaderBoard.DOFade(1, time).SetEase(Ease.InSine);
        _About.DOFade(1, time).SetEase(Ease.InSine);
        _Settings.DOFade(1, time).SetEase(Ease.InSine);
        _pauseButton.DOFade(0, time).SetEase(Ease.InSine);
   
        for (int i = 0; i < menuTexts.Count; i++)
        {
   
            menuTexts[i].DOColor(new Color(1, 1, 1, 1f), time).SetEase(Ease.InSine);
        }

 
    }

    public void onPlayButton()
    {

        Game.gameObject.SetActive(true);
        play.enabled = false;
        about.enabled = false;
        settings.enabled = false;
        countdownText.DOFade(1, 1).SetEase(Ease.InSine).OnComplete(() =>
        {
            countdownCoroutine = StartCoroutine(StartCountdown());
        });


    }

    private void FadeInGameboard()
    {
        Timer.DOFade(1, 0.5f);
        FrogCounter.DOFade(1, 0.5f);
        _timer.DOFade(1, 0.5f);
        _frogCounter.DOFade(1, 0.5f);

    }
    public void FadeOutGameboard()
    {
        Timer.DOFade(0, 0.5f);
        FrogCounter.DOFade(0, 0.5f);
        _timer.DOFade(0, 0.5f);
        _frogCounter.DOFade(0, 0.5f);

    }
    private IEnumerator StartCountdown()
    {
        RTcountDownText.DOScale(new Vector3(0.25f, 0.25f, 1), 0f);
        countdownText.text = "3";
        countdownText.DOFade(0, 1f);
        RTcountDownText.DOScale(new Vector3(1f, 1f, 1), 1f);
        yield return new WaitForSeconds(1f);
        countdownText.DOFade(1, 0.0f);
        RTcountDownText.DOScale(new Vector3(0.25f, 0.25f, 1), 0f);
        countdownText.text = "2";
        countdownText.DOFade(0, 1f);
        RTcountDownText.DOScale(new Vector3(1f, 1f, 1), 1f);
        yield return new WaitForSeconds(1f);
        countdownText.DOFade(1, 0.0f);
        RTcountDownText.DOScale(new Vector3(0.25f, 0.25f, 1), 0f);
        countdownText.text = "1";
        countdownText.DOFade(0, 1f);
        RTcountDownText.DOScale(new Vector3(1f, 1f, 1), 1f);
        yield return new WaitForSeconds(1f);
        countdownText.DOFade(1, 0f);
        RTcountDownText.DOScale(new Vector3(0.25f, 0.25f, 1), 0f);
        countdownText.text = "Kör";
        countdownText.DOFade(0, 2f);
        RTcountDownText.DOScale(new Vector3(1f, 1f, 1), 2f);
        yield return new WaitForSeconds(2f);

        StoryScreenPanel.DOFade(0, 1).SetEase(Ease.InSine).OnComplete(() => { StoryScreenPanel.gameObject.SetActive(false);

            //GameManager.instance.StartCoroutinefunc();
            RTcountDownText.DOScale(new Vector3(0.25f, 0.25f, 1), 0f);

        });

        FadeInGameboard();
        countdownText.text = " ";
        countdownText.DOFade(0, 1).SetEase(Ease.InSine);
        _pauseButton.gameObject.SetActive(true);
        _pauseButton.DOFade(1, 1f).SetEase(Ease.InSine);
        Game.gameObject.SetActive(false);
        StopCoroutine(countdownCoroutine);
        GameLoopManager.GameStates?.Invoke(GameState.Playing);
        // Countdown completed, you can perform any other actions here
    }


   public void onAboutButton()
    {
        play.enabled = false;
        about.enabled = false;
        settings.enabled = false;
        gameBoard.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        canvasMenu.renderMode = RenderMode.WorldSpace;
        Camera.main.transform.DOMove(rectTransformAbout.transform.position, 1.75f).SetEase(Ease.InOutSine);
        _backbutton.DOFade(1, 1f).SetEase(Ease.InSine);
      //  aboutimage.gameObject.SetActive(true);
       aboutimage.DOFade(1, 0.5f).SetEase(Ease.InSine);
       
        

    }

  async public void onSettingsButton()
    {
        
        

        play.enabled = false;
        about.enabled = false;
        settings.enabled = false;
        gameBoard.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        canvasMenu.renderMode = RenderMode.WorldSpace;
        UnloadMenuSettings();
       // StoryScreenPanel.DOFade(0.43f, 0.0f).SetEase(Ease.InSine).OnComplete(() =>
       // {
       //    
       // });
      await Task.Delay(350);
        bubbleSpawner.StartSpawning();
            settingsButton();
            Camera.main.transform.DOMove(rectTransformSettings.transform.position, 2f);
            Camera.main.DOOrthoSize(3f, 2f).SetEase(Ease.InOutSine).OnComplete(() =>
            {

                fadeSettingsMenu();


            });

        
       
      
        //Camera.main.DOOrthoSize(1.095521f, 2f).SetEase(Ease.InOutSine);
        // settingsimage.gameObject.SetActive(true);
        //settingsimage.DOFade(1, 0.5f).SetEase(Ease.InSine);
    }
    public void fadeSettingsMenu()
    {
        var time = 1f;
        rectTransformSettings2.DOScale(new Vector3(0.04255f, 0.04255f, 1), 0).SetEase(Ease.OutFlash);
        StoryScreenPanel.DOFade(0f, time).SetEase(Ease.InSine);
        _settingsBackButton.DOFade(1, time).SetEase(Ease.InSine);
         bg.DOFade(1, time);
         sand.DOFade(1, time);
         shark.DOFade(1, time);
         fish.DOFade(1, time);
         score.DOFade(1,time);
         highscore.DOFade(1, time);
         tidsgrans.DOFade(1, time);
         timelimitValue.DOFade(1, time);
         sekunder.DOFade(1, time);
         seagrass.DOFade(1, time);
         seagrass2.DOFade(1, time);
         music.DOFade(1, time);
         audio.DOFade(1, time);     
         increase.DOFade(1, time);
         decrease.DOFade(1, time);
         _GameTime.DOFade(1, time);
        _GameTimeShadow.DOFade(1, time);
        sharkk.DOFade(1, time);
         fishh.DOFade(1, time);

    }
    public void hideSettingsMenu()
    {
        var time = 1f;
        // rectTransformSettings2.DOScale(new Vector3(0f, 0f, 1), 1).SetEase(Ease.OutFlash);
        _settingsBackButton.DOFade(0, time).SetEase(Ease.InOutSine);
        StoryScreenPanel.DOFade(0.43f, time).SetEase(Ease.InOutSine);
      
       bg.DOFade(0, time).SetEase(Ease.InOutSine);
       sand.DOFade(0, time).SetEase(Ease.InOutSine); 
       shark.DOFade(0, time).SetEase(Ease.InOutSine); 
       fish.DOFade(0, time).SetEase(Ease.InOutSine); 
       score.DOFade(0, time).SetEase(Ease.InOutSine); 
       highscore.DOFade(0, time).SetEase(Ease.InOutSine); 
       tidsgrans.DOFade(0, time).SetEase(Ease.InOutSine); 
       timelimitValue.DOFade(0,time).SetEase(Ease.InOutSine); 
       sekunder.DOFade(0, time).SetEase(Ease.InOutSine); 
       seagrass.DOFade(0, time).SetEase(Ease.InOutSine); 
       seagrass2.DOFade(0, time).SetEase(Ease.InOutSine); 
       music.DOFade(0, time).SetEase(Ease.InOutSine); 
       audio.DOFade(0, time).SetEase(Ease.InOutSine);      
       increase.DOFade(0, time).SetEase(Ease.InOutSine); 
       decrease.DOFade(0, time).SetEase(Ease.InOutSine); 
       _GameTime.DOFade(0, time).SetEase(Ease.InOutSine); 
       _GameTimeShadow.DOFade(0, time).SetEase(Ease.InOutSine); 
       sharkk.DOFade(0, time).SetEase(Ease.InOutSine); 
       fishh.DOFade(0, time).SetEase(Ease.InOutSine); 
    }

    public void onLeaderBoardButton()
    {


    }
    public void unloadMenu()
    {
        StoryScreenPanel.DOFade(0, 0.5f).SetEase(Ease.InSine);

    }
    public async void onBackButton(int index)
    {
        if(index==2)
        {
            // Camera.main.transform.DOMove(new Vector3(0, 0, -10), 3f).SetEase(Ease.InOutSine);
            // Camera.main.DOOrthoSize(10.80384f, 3f).SetEase(Ease.InOutSine);
            
            hideSettingsMenu();

           
            
                await Task.Delay(250);
                settingsBackButton?.Invoke();



            await Task.Delay(750);


            Camera.main.transform.DOMove(rectTransformMenu.transform.position, 2f).SetEase(Ease.InOutSine);
                    Camera.main.DOOrthoSize(70f, 2f).SetEase(Ease.InSine).OnComplete(() =>
                    {
                        play.enabled = true;
                        about.enabled = true;



                    });
               

            await Task.Delay(1750);
            FadeInMenuSettings();






        }
        else if (index==1)
        {
            Camera.main.transform.DOMove(rectTransformMenu.transform.position, 1.5f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                // aboutimage.gameObject.SetActive(false);
                play.enabled = true;
                about.enabled = true;
                settings.enabled = true;

            }); 
       // aboutimage.DOFade(0, 0.5f).SetEase(Ease.InSine).OnComplete(() =>
       // {
       //     aboutimage.gameObject.SetActive(false);
       //
       // });
          
        }
        else if(index==0)
        {


            FadeOutGameboard();

            FadeInMenu();


                play.enabled = true;
                about.enabled = true;
                settings.enabled = true;
            
            StoryScreenPanel.gameObject.SetActive(true);
            StoryScreenPanel.DOFade(0.43f, 0.5f).SetEase(Ease.InSine);
        }
        else if (index==3)
        {
             UIManager.instance.UnloadScorePage();
          await  Awaitable.WaitForSecondsAsync(1f, destroyCancellationToken);
            onGameBoardBackButton();
        }
        else if(index ==4)
        {
            if(!isPaused)
            {
                
                touchBlocker.SetActive(true);
                isPaused = true;
           
                pauseGameObject.SetActive(true);
                for (int i = 0; i < pausePageImages.Count; i++)
                {

                    pausePageImages[i].DOFade(1, 0.1f);


                }
                pauseText1.DOFade(1, 0.1f);
                pauseText2.DOFade(1, 0.1f);
                GameManager.instance.fadePanel.gameObject.SetActive(true);
                GameManager.instance.fadePanel.DOFade(0.4f, 0.1f);
                pauseBGimage.DOFade(1f, 0.1f).OnComplete(() =>
                {
                    Time.timeScale = 0;
                });

            }
            else if(isPaused)
            {
                Time.timeScale = 1;
                touchBlocker.SetActive(false);
                isPaused = false;
               
                pauseGameObject.SetActive(false);
                for (int i = 0; i < pausePageImages.Count; i++)
                {

                    pausePageImages[i].DOFade(0, 0.0f);


                }
                pauseText1.DOFade(0, 0.1f);
                pauseText2.DOFade(0, 0.1f);
                GameManager.instance.fadePanel.DOFade(0, 0.1f);
                GameManager.instance.fadePanel.gameObject.SetActive(false);

                pauseBGimage.DOFade(0f, 0.0f);
            }
          
            
        }
        else if(index ==5)

        {
            Time.timeScale = 1;
            touchBlocker.SetActive(false);
            isPaused = false;

            for (int i = 0; i < pausePageImages.Count; i++)
            {

                pausePageImages[i].DOFade(0, 0.5f);


            }
            pauseText1.DOFade(0, 0.5f);
            pauseText2.DOFade(0, 0.5f);

            _pauseButton.DOFade(0, 0.5f).SetEase(Ease.InSine);
            pauseBGimage.DOFade(0f, 0.5f);
            GameManager.instance.resetPause();
            GameManager.instance.fadePanel.DOFade(0.4f, 0.5f).OnComplete(() =>
            {
                
                _pauseButton.gameObject.SetActive(false);
                pauseGameObject.SetActive(false);
        
                Timer.DOFade(0, 0.5f);
                FrogCounter.DOFade(0, 0.5f);
                _timer.DOFade(0, 0.5f);
                _frogCounter.DOFade(0, 0.5f);

                FadeInMenu();


                play.enabled = true;
                about.enabled = true;
                settings.enabled = true;
                GameManager.instance.fadePanel.DOFade(0.0f, 0.5f).SetEase(Ease.InSine);
                StoryScreenPanel.gameObject.SetActive(true);
                StoryScreenPanel.DOFade(0.43f, 0.5f).SetEase(Ease.InSine).OnComplete(() => {  GameManager.instance.fadePanel.gameObject.SetActive(false); });


            }); 
          
           
         
            
  
          

            


        }






        // await Gameimage.DOFade(0, 1f).SetEase(Ease.InSine).AsyncWaitForCompletion();

        //Gameimage.gameObject.SetActive(false);
        //settingsimage.gameObject.SetActive(false);
    }
}
