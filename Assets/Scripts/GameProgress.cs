using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class GameProgress
{
    public int _timeLimitIndex;
    public int Highscore30;
    public int Highscore60;
    public int Highscore90;
    public int Highscore120;
    public int _score;
    public bool audio;
    public bool music;


    public float speed;
    public float SpeedDecreaseValue;
   public float SpeedBoostValue;
    public float NightModeLength;



}

public class ProgressManager : MonoBehaviour
{


    public static void SaveGameProgress(string miniGameId, GameProgress progress)
    {
        string progressJson = JsonUtility.ToJson(progress);
        PlayerPrefs.SetString(miniGameId + "Progress", progressJson);
    }

    public static GameProgress LoadGameProgress(string miniGameId)
    {
        string loadedProgressJson = PlayerPrefs.GetString(miniGameId + "Progress");
        return JsonUtility.FromJson<GameProgress>(loadedProgressJson);
    }

    public static void DeleteGameProgress()
    {
        PlayerPrefs.DeleteAll();

    }



}
