using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayTimeManager : MonoBehaviour
{

    [SerializeField] private TMP_Text playTime, bestTimeText;
    private int time;

    private void Start()
    {
        int nowPlayTime = UIManager.playTime;
        if (!PlayerPrefs.HasKey("BestTime")) PlayerPrefs.SetInt("BestTime", nowPlayTime);

        int bestTime = PlayerPrefs.GetInt("BestTime");
        //update bestTime
        if(nowPlayTime >= bestTime)
        {
            bestTime = nowPlayTime;
            PlayerPrefs.SetInt("BestTime", nowPlayTime);
            playTime.color = Color.green;
        }
        bestTimeText.text = $"{bestTime / 60:D2} : {bestTime % 60:D2}";
        playTime.text = $"{nowPlayTime / 60:D2} : {nowPlayTime % 60:D2}";
        

    }
}
