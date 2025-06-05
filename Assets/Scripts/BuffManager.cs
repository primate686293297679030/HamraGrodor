using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{


    public float SpeedBoostValue;
    public float TimeBoostValue;
    public float DoubleScoreValue;
    public float PulsatingTouchValue;
    public float NightModeValue;
    public float TimeDecreaseValue;
    public float SpeedDecreaseValue;
    public float FrogShieldValue;

    public float SpeedBoostLength;
    public float TimeBoostLength;
    public float DoubleScoreLength;
    public float PulsatingTouchLength;
    public float NightModeLength;
    public float TimeDecreaseLength;
    public float SpeedDecreaseLength;
    public float FrogShieldLength;

    public Buff speedBoostBuff;
    public List<Image> buffPanelList;

    public List<Image> buffCircleTimer;
    public static BuffManager instance;
    public List<Buff> buffObjectList;
    private List<int> buffIndexList= new List<int>();
    public static Action<int> OnBuffFinnished;
    // Start is called before the first frame update
    public static List<Sprite> buffsUsed=new List<Sprite>();
    public List<Slider> sliders;
    
    void Awake()
    {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    void Start()
    {
        SpeedBoostValue = GameLoopManager.progress.SpeedBoostValue;
       SpeedDecreaseValue = GameLoopManager.progress.SpeedDecreaseValue;
        NightModeLength = GameLoopManager.progress.NightModeLength;
        DeveloperMenu.OnApplyChanges += UpdateBuffVariables;
        GameManager.buffTrigger += ActivateBuff;
        OnBuffFinnished += RemoveBuff;
        turnOffAllBuffs();
    }
    public void Reset()
    {

        for (int j = 0; j < buffPanelList.Count; j++)
        {
            OutBuffAnimation(buffPanelList[j], buffCircleTimer[j]);
        }
        foreach (var thing in buffObjectList)
        {
            if(thing.beenUsed)
            {
                thing.beenUsed = false;
                buffsUsed.Add(thing.sprite);

            }
        }

       
        buffIndexList.Clear();
    }
    private void OnDestroy()
    {
        GameManager.buffTrigger -= ActivateBuff;
        OnBuffFinnished -= RemoveBuff;
        DeveloperMenu.OnApplyChanges -= UpdateBuffVariables;
    }
    
    public void UpdateBuffVariables()
    {
        SpeedBoostValue = GameLoopManager.progress.SpeedBoostValue ;
        SpeedDecreaseValue = GameLoopManager.progress.SpeedDecreaseValue;
        NightModeLength = GameLoopManager.progress.NightModeLength;
        for (int i = 0; i < buffObjectList.Count; i++)
        {

            buffObjectList[i].updateVariables(SpeedBoostValue, SpeedDecreaseValue, NightModeLength);

        }
}


    public void ActivateBuff(int index)
    {
   
                AddBuff(index);
 

    }

    private void AddBuff(int index)
    {   bool isActive = false;
        if(buffIndexList.Count==0)
        {
            buffIndexList.Add(index);
            buffObjectList[buffIndexList[buffIndexList.Count-1]].BuffTimer();
            InBuffAnimation(buffPanelList[0], buffCircleTimer[0]);
        }
        else
        {
            for (int i = 0; i < buffIndexList.Count; i++)
            {
                if (buffIndexList[i] == index)
                {
                    buffObjectList[i].OnAlreadyActive();
                    isActive = true;
                }
            }
            if (!isActive)
            {
                buffIndexList.Add(index);
                buffObjectList[buffIndexList[buffIndexList.Count-1]].BuffTimer();
                //InBuffAnimation(buffPanelList[buffIndexList.Count-1]);
                UpdateBuffLocation();
            }
        }
    
        
       
    }
    public void InBuffAnimation(Image img,Image img2)
    {
        img.DOFade(1.0f, 1.0f);
        img2.DOFade(0.75f, 1.0f);
        img.gameObject.GetComponent<RectTransform>().DOScale(1.5f, 1.0f);
    }
    public void OutBuffAnimation(Image img, Image img2)
    {
        img.DOFade(0.0f, 1.0f);
        img2.DOFade(0.0f, 1.0f);
        img.gameObject.GetComponent<RectTransform>().DOScale(0.0f, 1.0f);
    }
    private void SpeedBoost()
    {
        GameManager.instance.Frogspeed = 1.25f;


    }
    public void UpdateBuffLocation()
    {
        for(int i=0; i<buffIndexList.Count;i++)
        {
            OutBuffAnimation(buffPanelList[i], buffCircleTimer[i]);
            buffPanelList[i].sprite = buffObjectList[buffIndexList[i]].sprite;

            InBuffAnimation(buffPanelList[i], buffCircleTimer[i]);
        }
        
    }
 

    private void RemoveBuff(int uniqueID)
    {
        for(int i=0; i< buffIndexList.Count;i++)
        {
            if (uniqueID == buffIndexList[i])
            {
                buffIndexList.RemoveAt(i);
                for(int j =0; j<buffPanelList.Count; j++)
                {
                    OutBuffAnimation(buffPanelList[j], buffCircleTimer[j]);
                }
                
                OutBuffAnimation(buffPanelList[i], buffCircleTimer[i]);
                UpdateBuffLocation();
            }
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;buffIndexList.Count>i;i++)
        {
           if(i<buffIndexList.Count)
           {
               buffPanelList[i].sprite = buffObjectList[buffIndexList[i]].sprite;
           }
           else
           {
           
               buffPanelList[i].sprite = null;
           }
          
        }
        
            for (int i = 0; i < buffIndexList.Count; i++)
            {
                sliders[i].value = buffObjectList[buffIndexList[i]]._time;

            }
        
       

    }

    public void turnOffAllBuffs()
    {
        for (int i = 0; i < buffIndexList.Count; i++)
        {
            buffObjectList[buffIndexList[i]].BuffTimer(0);
        }
    }
}
