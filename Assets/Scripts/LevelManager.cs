using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
    [SerializeField] private Image _loadingScreenLogo;
    [SerializeField] private GameObject _Logo;
    private float _target;
    bool initDelay = true;
    private float time;
    Task g;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    // Start is called before the first frame update
    async void Start()
    {   //_Logo.transform.DOScale(0.5f, 2).SetEase(Ease.OutQuad);
        
       // _loadingScreenLogo.DOFade(0, 2);
      await Task.Delay(1000);
        try
        {
           // await Awaitable.WaitForSecondsAsync(3, destroyCancellationToken);
            await LoadLevel("MainMenuScene");


        }
        catch
        {
        Debug.Log("Destroy token was cancelled.");
        }
        // SceneManager.LoadScene(1);
    }
    public async Task LoadLevel(string sceneName)
    {
        _target = 0;
        _progressBar.fillAmount = 0;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        _loaderCanvas.SetActive(true);
        do {  _target = scene.progress;  } while (scene.progress < 0.9f);

        while(!destroyCancellationToken.IsCancellationRequested)
        {
            await Awaitable.WaitForSecondsAsync(9, destroyCancellationToken);
            //scene.allowSceneActivation = true;
            //_loaderCanvas.SetActive(false);
          
          
        }

      
    }

    private async Task ProgressbarAnimSpeed() {    
    
       
        _progressBar.fillAmount = Mathf.Lerp(_progressBar.fillAmount, _progressBar.fillAmount + 3 * 0.01f, Time.deltaTime * 2);
    }
    private async Task delay()
    {
        Task.Delay(1000);
        initDelay = false;
    }
    // Update is called once per frame
    async void Update()
    {
        time += Time.deltaTime;
        if (initDelay == true)
        {
            delay();
        }
        else
        {
            ProgressbarAnimSpeed();
        }

        

    
      //  var b=  ProgressbarAnimSpeed();
      
   
        if (time == 3.0f)
        {
            
           // gameObject.SetActive(false);

        }

    }
}
