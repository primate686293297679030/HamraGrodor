using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "new buff", menuName ="Buff")]
public class Buff : ScriptableObject
{
    float animSpeed;
    Tween anim;
    Sequence sequence;
    public Sprite sprite;
    float time;
    
    public bool isActive;
    public bool beenUsed; // --> used in buff manager to check if the buff has been used thereupon use OnAlreadyActive() instead of UniqueFunction()
    
    float _buffTimerTotal; // Total and remaining time are used for calculations for the buff icon timer
    float _remainingTime;
    //Adjustments to unique id can be made in Editor
    public int uniqueId;
    public float _time=0;
    float timeDecrease=0.01f;


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

    public static Buff instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            GameObject.Destroy(this);
        }
    }



    async public void BuffTimer()
    {
         _time = 0;
    beenUsed = true;
        _buffTimerTotal = 5;
       _remainingTime = 5;
        UniqueFunction();

        
        //BuffManager.instance.sliderTimer(uniqueId, _time);

        while (_remainingTime != 0)
        {
            
             await Awaitable.WaitForSecondsAsync(timeDecrease);
            _remainingTime-=timeDecrease;
            _time = _remainingTime / _buffTimerTotal;
            if (_remainingTime <= 0)
            {
                BuffManager.OnBuffFinnished?.Invoke(uniqueId);
                TurnOffBuffEffect();
                _buffTimerTotal = 0;
                break;
                
                
            }
          
        }

    }
   public void updateVariables(float a,float b,float c)
    {
        SpeedBoostValue = a;
         SpeedDecreaseValue= b;
        NightModeLength= c;

    }
    public void BuffTimer(int i)
    {
        _remainingTime = i;
        TurnOffBuffEffect();
        _remainingTime = 0;

    }
    public void OnAlreadyActive()
    {
        //UniqueFunction();
        switch(uniqueId)
        {
           

            case 0:
                _remainingTime += 5;
                _buffTimerTotal += 5;
                break;
                case 1:
                GameManager.instance._gameTime += 5f;
                break;
                case 2:
                _remainingTime += 5;
                _buffTimerTotal += 5;
                break;
                case 3:
                _remainingTime += 5;
                _buffTimerTotal += 5;
                break;
                case 4:
                _remainingTime += 5;
                _buffTimerTotal += 5;
                break;
                case 5:

                if (GameManager.instance._gameTime <= 5)
                {

                    GameManager.instance.EndGame();
                    GameManager.instance.UpdateTimerDisplay();

                }
                else
                {
                    GameManager.instance._gameTime -= 5f;
                }
                break;
                case 6:
                _remainingTime += 5;
                _buffTimerTotal += 5;
                break;
                case 7:
                _remainingTime += 5;
                _buffTimerTotal += 5;
                break;
              

        }
        _buffTimerTotal += 5;
        _remainingTime += 5;
    }

    public void UniqueFunction()
    {
        /*
        0. Speed Boost
        1. Time Boost
        2. Double Score
        3. Pulsating Touch
        4. Night Mode
        5. Time Decrease
        6  Speed Decrease
        7. Frog Shield
        */
        switch (uniqueId)
        {
           
            case 0:
                GameManager.instance.Frogspeed += SpeedBoostValue;
                GameManager.instance.frogSpawnFrequency -= 0.25f;
             
                break;
            case 1:
                GameManager.instance._gameTime += 5f;
                break;
            case 2:
                GameManager.instance.scoreAmount = 2;
                GameManager.doubleScore = true;
                GameManager.OnDoubleScore?.Invoke();

                break;
            case 3:
                GameManager.pulsatingTouch=true;

                break;
            case 4:
                _buffTimerTotal += NightModeLength;
                _remainingTime += NightModeLength;
                GameManager.instance.nightPanel.DOFade(0.5f, 1.0f);

                break;
                case 5:
                if(GameManager.instance._gameTime <=5)
                {

                   GameManager.instance.EndGame();
                    GameManager.instance.UpdateTimerDisplay();
                    
                }
                else
                {
                    GameManager.instance._gameTime -= 5f;
                }
               
                break;
                case 6:
                GameManager.instance.Frogspeed -= SpeedDecreaseValue;
                GameManager.instance.frogSpawnFrequency += 0.25f;
                break;
                case 7:
                GameManager.frogShield = true;
                GameManager.OnFrogShield?.Invoke();
                break;
        }
        
    }

    public void TurnOffBuffEffect()
    {
        switch (uniqueId)
        {
            case 0:
                GameManager.instance.Frogspeed -= SpeedBoostValue;
                GameManager.instance.frogSpawnFrequency += 0.25f;

                break;
            case 1:

                break;
            case 2:
                GameManager.instance.scoreAmount = 1;
                GameManager.doubleScore = false;
                GameManager.OnDoubleScore?.Invoke();
                break;
            case 3:
                GameManager.pulsatingTouch = false;
                break;
            case 4:
                GameManager.instance.nightPanel.DOFade(0.0f, 1.0f);

                break;
            case 5:
                break;
            case 6:
                GameManager.instance.Frogspeed += SpeedDecreaseValue;
                GameManager.instance.frogSpawnFrequency -= 0.25f;
                break;
            case 7:
                GameManager.frogShield = false;
                GameManager.OnFrogShield?.Invoke();
                break;
        }
    }
}
