using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    [SerializeField] GameObject pauseObj;
    [SerializeField] AudioSource bgm;
    private void Start()
    {
        instance = this;
    }
    public bool IsPause
    {
        get => Time.timeScale <= 0;
        set
        {
            pauseObj.SetActive(value);
            Time.timeScale = value ? 0 : 1;
            if(value) bgm.Pause();
            else bgm.UnPause();
        }
    }
}
