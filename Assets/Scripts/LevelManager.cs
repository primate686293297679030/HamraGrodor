using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour
{
  

    [SerializeField] private Camera _mainCamera;    
    [SerializeField] private Slider _progressBarObj;    
    //  [SerializeField] private Image _progressBar;

    [Header("Loading Screen")]

    [SerializeField] private RectTransform _hamra;
    [SerializeField] private RectTransform _grodor;
    [SerializeField] private RectTransform _bambinoGame;
    [SerializeField] private GameObject _Menu;
    [SerializeField] private Image img_hamra;
    [SerializeField] private Image img_grodor;
    [SerializeField] private Image _bambinoObject;
    [SerializeField] private GameObject _sunrays;
    [SerializeField] private Image _fog;
    [SerializeField] private GameObject _waterShadow2;
    [SerializeField] private GameObject _waterShadow1;
    [SerializeField] private GameObject _waterDarkSpots;

    [Header("Story Screen")]
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _panelFilter;
    [SerializeField] private Image _loadingScreenLogo;
    [SerializeField] private Image _loadingBarBackground;
    [SerializeField] private Image _loadingBarFill;
    [SerializeField] private Button _continueButton;
    [SerializeField] private TextMeshProUGUI _continueButtonText;


    [Header("Menu Screen")]

    [SerializeField] private Image _StartGame;
    [SerializeField] private Image _LeaderBoard;
    [SerializeField] private Image _About;
    [SerializeField] private Image _Settings;
    [SerializeField] private Image _logoMenu;

    [Header("Game Board")]

   // [SerializeField] private Image BackButton;
    [SerializeField] GameObject GameBoard;
    [SerializeField] RectTransform rectTransformSunRay;


    private float _target;
    private bool initDelay = true;
    private float time;
    private Task g;
    private float targetOrthographicSize;
    private Vector3 _targetPosition1 = new Vector3(0, 0, 0);
    private Vector3 _targetPosition2 = new Vector3(73, -700f, 0);

    public List<TextMeshProUGUI> menuTexts;
    public Action NextAnimationSequence;
    public static LevelManager instance;
    public GameObject blockTouchPanel;

    private void Awake()
    {
        _hamra.transform.localScale=new Vector3(0,0,1);
        _grodor.transform.localScale=new Vector3(0,0,1);
        _bambinoGame.localScale=new Vector3(0,0,1);
        _continueButton.gameObject.SetActive(false);

        if (instance == null)
        {
            instance = this;
          //  DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    
    }

    async void Start()
    {
        await ProgressbarAnimSpeed();       
    }
  

    private async Task ProgressbarAnimSpeed() {

        _hamra.transform.DOScale(new Vector3(0.5f, 0.5f, 1), 1.33f).SetEase(Ease.OutBack);
        _grodor.transform.DOScale(new Vector3(0.5f, 0.5f, 1), 1.33f).SetEase(Ease.OutBack);
        _bambinoGame.transform.DOScale(new Vector3(0.5f, 0.5f, 1), 1.33f).SetEase(Ease.OutBack);
        await DOVirtual.Float(0f,3f, 5f, v => SetFillAmount(v)).AsyncWaitForCompletion();

        _loadingBarBackground.DOFade(0, 1).SetEase(Ease.OutSine);
        _fog.DOFade(0, 1).SetEase(Ease.OutSine);
        _loadingBarFill.DOFade(0, 1).SetEase(Ease.OutSine);
        _bambinoObject.DOFade(0, 1).SetEase(Ease.OutSine);

        _sunrays.gameObject.GetComponent<WaveAnimation>().enabled=false;
       
        _waterShadow1.transform.DOScale(new Vector3(1, 1, 1), 1);
        _waterShadow1.gameObject.GetComponent<Image>().DOFade(0, 1);
        _sunrays.transform.DOScale(new Vector3(2.5f, 2, 1), 1f);
        _panelFilter.DOFade(0.43f, 1).SetEase(Ease.InSine);

        _waterShadow2.SetActive(false);

        _hamra.DOLocalMove(new Vector3(0, 313.4f, 0), 2f).SetEase(Ease.OutBack);
        _grodor.DOLocalMove(new Vector3(205f, 220f, 0), 2f).SetEase(Ease.OutBack);
    
        _hamra.transform.DOScale(new Vector3(0.4f, 0.4f, 1), 1f).SetEase(Ease.OutQuad);
        _grodor.transform.DOScale(new Vector3(0.33f, 0.33f, 1), 1f).SetEase(Ease.OutQuad);
        _text.DOColor(new Color(1, 1, 1, 1f), 3f).SetEase(Ease.InQuint);
        rectTransformSunRay.DOAnchorPos(_targetPosition2, 1f).SetEase(Ease.InOutSine);
  
       
     
        await GameBoard.transform.DOScale(new Vector3(1f, 1f, 1f), 2f).SetEase(Ease.OutSine).AsyncWaitForCompletion();
        
        _continueButton.gameObject.SetActive(true);
        _continueButtonText.DOColor(new Color(1, 1, 1, 1f), 1f).SetEase(Ease.InOutSine);
        _continueButton.GetComponent<Image>().DOFade(1, 1f).SetEase(Ease.InOutSine).OnComplete(() => { _continueButton.enabled = true; });
        _sunrays.gameObject.GetComponent<WaveAnimation>().enabled = enabled;
   

    }
    async void SetFillAmount(float v)
    {
       _progressBarObj.value = v;       
    }
    public async void OnContinue()
    {
        _text.DOColor(new Color(1, 1, 1, 0f), 0.5f).SetEase(Ease.InSine);
        _continueButton.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.InSine);
        _continueButtonText.DOColor(new Color(1, 1, 1, 0f), 0.5f).SetEase(Ease.InSine);
        img_hamra.DOFade(0, 0.5f).SetEase(Ease.InSine);
        img_grodor.DOFade(0,0.5f).SetEase(Ease.InSine);      
        await img_hamra.DOFade(0, 0.5f).AsyncWaitForCompletion();
        LoadMenu();

    }

    public async void LoadMenu()
    {
        _Menu.SetActive(true);
        blockTouchPanel.SetActive(true);
        _panelFilter.DOFade(0.33f, 0.9f).SetEase(Ease.InSine);
        
        _logoMenu.DOFade(1, 1f).SetEase(Ease.InCubic);
        _StartGame.DOFade(1, 1f).SetEase(Ease.InCubic);
        menuTexts[0].DOColor(new Color(1, 1, 1, 1f), 1f).SetEase(Ease.InCubic);
        _LeaderBoard.DOFade(1, 1f).SetEase(Ease.InCubic);
        menuTexts[1].DOColor(new Color(1, 1, 1, 1f), 1f).SetEase(Ease.InCubic);
        menuTexts[2].DOColor(new Color(1, 1, 1, 1f), 1f).SetEase(Ease.InCubic);
        _About.DOFade(1, 1f).SetEase(Ease.InCubic);
        menuTexts[3].DOColor(new Color(1, 1, 1, 1f), 1f).SetEase(Ease.InCubic);
        _Settings.DOFade(1, 1f).SetEase(Ease.InCubic);
        menuTexts[4].DOColor(new Color(1, 1, 1, 1f), 1f).SetEase(Ease.InCubic).OnComplete(() => { blockTouchPanel.SetActive(false); });
      
       // await BackButton.DOFade(1, 0.5f).SetEase(Ease.InCubic).AsyncWaitForCompletion();

        GameLoopManager.GameStates?.Invoke(GameState.MainMenu);

       
    }

}
