using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class Settings : MonoBehaviour
{
    [SerializeField] private Sprite _disableAudio;
    [SerializeField] private Sprite _enableAudio;
    [SerializeField] private Sprite _disableMusic;
    [SerializeField] private Sprite _enableMusic;
 
    [SerializeField] private Image _musicButton;
    [SerializeField] private Image _soundButton;

    [SerializeField] private Image increaseButton;
    [SerializeField] private Image decreaseButton;
    private 
    bool isMusicDisabled = false;
    bool isAudioDisabled = false;

    public static Action<int> OnTimeSelectorIndex;
  
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private List<string> _timeSelector;
    [SerializeField] public List<int> _highScore;
    private int _timeSelectorIndex=0;

    [SerializeField] private Image _gameTime;

    [SerializeField] private List<Sprite> lGameTime;
    private void Start()
    {
        _timeSelectorIndex = GameLoopManager.progress._timeLimitIndex;
        OnTimeSelectorIndex(_timeSelectorIndex);
        for (int i=0; i<_highScore.Count; i++)
        {
            switch(i)
            {
                case 0:
                    _highScore[i] = GameLoopManager.progress.Highscore30;
                    break;
                case 1:
                    _highScore[i] = GameLoopManager.progress.Highscore60;
                    break;
                case 2:
                    _highScore[i] = GameLoopManager.progress.Highscore90;
                    break;
                case 3:
                    _highScore[i] = GameLoopManager.progress.Highscore120;
                    break;
            }
        }
        _highScoreText.text = _highScore[_timeSelectorIndex].ToString();
        _gameTime.sprite = lGameTime[_timeSelectorIndex];

        isMusicDisabled= GameLoopManager.progress.music;
        isAudioDisabled = GameLoopManager.progress.audio;
        if (isMusicDisabled)
        {
            _musicButton.sprite = _disableMusic;
        }
        else
        {
            _musicButton.sprite = _enableMusic;
        }
        if (isAudioDisabled)
        {
            _soundButton.sprite = _disableAudio;
        }
        else
        {
            _soundButton.sprite = _enableAudio;
        }
    }
    public void OnBackButton()
    {
        GameLoopManager.progress._timeLimitIndex = _timeSelectorIndex;
        ProgressManager.SaveGameProgress("savedProgress", GameLoopManager.progress);
        OnTimeSelectorIndex(_timeSelectorIndex);
    }
    public void OnDisableMusic()
    {
        isMusicDisabled = !isMusicDisabled;
        GameLoopManager.progress.music = isMusicDisabled;
        if (isMusicDisabled)
        {
            _musicButton.sprite = _disableMusic;
        }
        else
        {
            _musicButton.sprite = _enableMusic;
        }
    }
    public void OnDisableAudio()
    {

        isAudioDisabled = !isAudioDisabled;
        GameLoopManager.progress.audio = isAudioDisabled;
        if (isAudioDisabled)
        {
            _soundButton.sprite = _disableAudio;
        }
        else
        {
            _soundButton.sprite = _enableAudio;
        }
    }

    public void OnTimeSelectorIncrease()
    {
        _timeSelectorIndex++;

        

        if (_timeSelectorIndex > _timeSelector.Count - 1)
        {
          
            _timeSelectorIndex = _timeSelector.Count - 1;
        }

        if (_timeSelectorIndex == 3)
        {
            increaseButton.DOFade(0.5f, 0.1f);
        }
        else
        {
            increaseButton.DOFade(1f, 0.1f);
            decreaseButton.DOFade(1f, 0.1f);
        }

        switch (_timeSelectorIndex)
        {
            case 0:
                _highScore[_timeSelectorIndex] = GameLoopManager.progress.Highscore30;
                break;
            case 1:
                _highScore[_timeSelectorIndex] = GameLoopManager.progress.Highscore60;
                break;
            case 2:
                _highScore[_timeSelectorIndex] = GameLoopManager.progress.Highscore90;
                break;
            case 3:
                _highScore[_timeSelectorIndex] = GameLoopManager.progress.Highscore120;
                break;
        }
        _highScoreText.text = _highScore[_timeSelectorIndex].ToString();
        _gameTime.sprite = lGameTime[_timeSelectorIndex];
    }
    public void OnTimeSelectorDecrease()
    {
        _timeSelectorIndex--;

   
        if (_timeSelectorIndex < 0)
        {
            
          
            _timeSelectorIndex = 0;
        }
        if (_timeSelectorIndex == 0)
        {
            decreaseButton.DOFade(0.5f, 0.1f);
        }
        else
        {
            increaseButton.DOFade(1f, 0.1f);
            decreaseButton.DOFade(1f, 0.1f);
        }

        switch (_timeSelectorIndex)
        {
            case 0:
                _highScore[_timeSelectorIndex] = GameLoopManager.progress.Highscore30;
                break;
            case 1:
                _highScore[_timeSelectorIndex] = GameLoopManager.progress.Highscore60;
                break;
            case 2:
                _highScore[_timeSelectorIndex] = GameLoopManager.progress.Highscore90;
                break;
            case 3:
                _highScore[_timeSelectorIndex] = GameLoopManager.progress.Highscore120;
                break;
        }

        _highScoreText.text = _highScore[_timeSelectorIndex].ToString();
        _gameTime.sprite = lGameTime[_timeSelectorIndex];
    }

    public void OnSettingsLoad()
    {
        switch (GameLoopManager.progress._timeLimitIndex)
        {
            case 0:
                _highScore[GameLoopManager.progress._timeLimitIndex] = GameLoopManager.progress.Highscore30;
                break;
            case 1:
                _highScore[GameLoopManager.progress._timeLimitIndex] = GameLoopManager.progress.Highscore60;
                break;
            case 2:
                _highScore[GameLoopManager.progress._timeLimitIndex] = GameLoopManager.progress.Highscore90;
                break;
            case 3:
                _highScore[GameLoopManager.progress._timeLimitIndex] = GameLoopManager.progress.Highscore120;
                break;
        }

        _highScoreText.text = _highScore[GameLoopManager.progress._timeLimitIndex].ToString();
        _gameTime.sprite = lGameTime[GameLoopManager.progress._timeLimitIndex];

    }
 
}
