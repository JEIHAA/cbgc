using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    WaitForSeconds oneSec;
    [SerializeField]
    TMP_Text logAmountText, fleshAmountText, nowTime, leftEnemy, leftTimeNextWave, campFireLeftTime, torchLeftPercent;
    public void UpdateLogAmountUI(int amount) { logAmountText.text = $" x {amount}"; }
    public void UpdateFleshAmountUI(int amount) { fleshAmountText.text = $" x {amount}"; }
    public void UpdateLeftEnemyUI(int amount) { leftEnemy.text = $"{amount} enemy is left."; }
    public void UpdateNowTimeUI(int amount) { nowTime.text = $"{amount}s"; }
    public void UpdateLeftTimeNextWavetUI(int amount) { leftTimeNextWave.text = $"{amount}s"; }
    public void UpdateCampFireLeftTimetUI(int amount) { campFireLeftTime.text = $"{amount}s"; }
    public void UpdateTorchLeftTimetUI(int amount) { torchLeftPercent.text = $"{amount}%"; }

    private void Start()
    {
        oneSec = new(1f);
        instance = this;
        StartCoroutine(OneSecLooper());
    }
    IEnumerator OneSecLooper()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        while (true)
        {
            yield return oneSec;
            UpdateNowTimeUI((int)stopwatch.ElapsedMilliseconds / 1000);
        }
    }
}
