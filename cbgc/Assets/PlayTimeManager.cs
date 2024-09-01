using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayTimeManager : MonoBehaviour
{

    [SerializeField] private TMP_Text playTime;
    private int time;

    private void Start()
    {
        playTime.text = $"{UIManager.playTime / 60} : {UIManager.playTime % 60}";
    }
}
