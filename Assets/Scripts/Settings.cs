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

    [SerializeField] private Image _MusicButtonPause;
    [SerializeField] private Image _SoundButtonPause;

    [SerializeField] private Image _musicButton;
    [SerializeField] private Image _soundButton;

    [SerializeField] private Image increaseButton;
    [SerializeField] private Image decreaseButton;
    private 
    bool isMusic = false;
    bool isAudio = false;

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

        isMusic= GameLoopManager.progress.music;
        isAudio = GameLoopManager.progress.audio;
        if (isMusic)
        { 
            
            _MusicButtonPause.sprite = _enableMusic;
            _musicButton.sprite = _enableMusic;
        }
        else
        {
            _MusicButtonPause.sprite = _disableMusic;
            _musicButton.sprite = _disableMusic;
        }
        if (isAudio)
        {
            _SoundButtonPause.sprite = _enableAudio;
            _soundButton.sprite = _enableAudio;
        }
        else
        {
            _SoundButtonPause.sprite = _disableAudio;
            _soundButton.sprite = _disableAudio;
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
        isMusic = !isMusic;
        GameLoopManager.progress.music = isMusic;
        if (isMusic)
        {
            _MusicButtonPause.sprite = _enableMusic;
            _musicButton.sprite = _enableMusic;
        }
        else
        {
            _MusicButtonPause.sprite = _disableMusic;
            _musicButton.sprite = _disableMusic;

        }
    }
    public void OnDisableAudio()
    {

        isAudio = !isAudio;
        GameLoopManager.progress.audio = isAudio;
        if (isAudio)
        {
            _SoundButtonPause.sprite = _enableAudio;
            _soundButton.sprite = _enableAudio;
        }
        else
        {
            _SoundButtonPause.sprite = _disableAudio;
            _soundButton.sprite = _disableAudio;
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

        isMusic = GameLoopManager.progress.music;
        isAudio = GameLoopManager.progress.audio;
        if (isMusic)
        {
            _MusicButtonPause.sprite = _enableMusic;
            _musicButton.sprite = _enableMusic;
        }
        else
        {
            _MusicButtonPause.sprite = _disableMusic;
            _musicButton.sprite = _disableMusic;
        }
        if (isAudio)
        {
            _SoundButtonPause.sprite = _enableAudio;
            _soundButton.sprite = _enableAudio;
        }
        else
        {
            _SoundButtonPause.sprite = _disableAudio;
            _soundButton.sprite = _disableAudio;
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
 
}
