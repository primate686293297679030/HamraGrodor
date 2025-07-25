using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Threading.Tasks;

public class FrogScript : MonoBehaviour, IPointerDownHandler
{
    public Sprite smashedSprite; // Assign the designated image sprite in the Inspector
    public Sprite poffSprite;
    public Sprite bufffrogSprite;

    public Sprite DefaultSprite;
    public Image image;
    Image img;
    public int frogListIndex;
    Material mat;
    public bool clicked = false;
    public bool IsBuffFrog = false;
    public int touchCounter = 0;
    public int maxTouches = 1;
    public Material golden;
    public Material defaultMaterial;
    public GameObject frog;
    private bool isFrogSmashed = false;
    public bool isOutsideBounds = false;
    public bool swap = false;


    void OnEnable()
    {
        // if (!isInitialized)
        // {
        //     Initialize();
        // }
    }
    void Start()
    {

        img = gameObject.GetComponent<Image>();
        mat = img.material;
        frog = gameObject;
        image.sprite = DefaultSprite;

    }



    public async void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.pulsatingTouch == true && !IsBuffFrog && !clicked)
        {

            GameManager.instance.StartPulsatingTouch(frog);
        }
        else

        {


            if (GameManager.frogShield == true)
            {

                maxTouches = 2;
            }
            else if (GameManager.frogShield == false)
            {
                maxTouches = 1;
            }
            touchCounter++;
            if (touchCounter >= maxTouches)
            {


                if (clicked == false && IsBuffFrog)
                {
                    clicked = true;
                    ChangeSprite();

                    GameManager.BuffFrogClicked?.Invoke();
                    swap = true;

                    touchCounter = 0;
                }
                else if (clicked == false && !IsBuffFrog)
                {
                    isFrogSmashed = true;
                    img.color = new Color(1f, 1f, 1f, 1f);
                    clicked = true;
                    ChangeSprite();
                    GameManager.instance.addScore();
                    touchCounter = 0;
               //     await Task.Delay(500);
                   
                    if (clicked)
                    {
                        ChangeSprite();
                    }
                }

            }
            else if (GameManager.frogShield == true && touchCounter < maxTouches && isFrogSmashed == false)
            {

                MakeFrogFaded();
            }

        }



    }
    // for Pulsating touch buff effect
    public void onClicked()
    {

        if (GameManager.frogShield == true)
        {

            maxTouches = 2;
        }
        else if (GameManager.frogShield == false)
        {
            maxTouches = 1;
        }
        touchCounter++;

        if (touchCounter >= maxTouches)
        {


            if (clicked == false && !IsBuffFrog)
            {
                isFrogSmashed = true;
                img.color = new Color(1f, 1f, 1f, 1f);
                clicked = true;
                ChangeSprite();
                GameManager.instance.addScore();
                touchCounter = 0;


            }

        }
        else if (GameManager.frogShield == true && touchCounter < maxTouches && isFrogSmashed == false)
        {

            MakeFrogFaded();
        }


    }
    public void ChangeSprite()
    {
        if (IsBuffFrog)
        {
            image.sprite = poffSprite;
            transform.DOScale(0.0f, 1.0f);
            image.DOFade(0.0f, 1.0f);


        }
        else
        {
            if (image.sprite.name == smashedSprite.name)
            {
                image.sprite = poffSprite;
                transform.DOScale(0.0f, 1.0f);
                image.DOFade(0.0f, 1.0f);
            }
            else
            {
                image.sprite = smashedSprite;
            }
        }
    }

    public void Reset()
    {

        transform.DOKill();
        image.DOKill();
        transform.DOScale(0.85f, 0f);
        image.DOFade(1.0f, 0f);
        clicked = false;
        touchCounter = 0;
        
        isFrogSmashed = false;
        img.color = new Color(1f, 1f, 1f, 1f);
        isOutsideBounds = true;
        
        ChangeToNormalFrog();
    }



    public void MakeFrogFaded()
    {

        img.color = new Color(0.5943396f, 0.5943396f, 0.5943396f, 0.4f);
    }
    public void ChangeToBuffFrog()
    {
        image.sprite = bufffrogSprite;
        IsBuffFrog = true;
    }
    public void ChangeToNormalFrog()
    {
        image.sprite = DefaultSprite;
        IsBuffFrog = false;
    }
}

