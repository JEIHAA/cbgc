using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    TMP_Text logAmountText, fleshAmountText, nowWave, leftEnemy, leftTimeNextWave, campFireLeftTime;
    public void UpdateLogAmountUI(int amount) { logAmountText.text = $" x {amount}"; }
    public void UpdateFleshAmountUI(int amount) { fleshAmountText.text = $" x {amount}"; }
    public void UpdateLeftEnemyUI(int amount) { leftEnemy.text = $"{amount} enemy is left."; }
    public void UpdateNowWavetUI(int amount) { nowWave.text = $" Now wave is {amount}"; }
    public void UpdateLeftTimeNextWavetUI(int amount) { leftTimeNextWave.text = $"Left {amount} sec"; }
    public void UpdateCampFireLeftTimetUI(int amount) { campFireLeftTime.text = $"Left {amount} sec"; }
    
    private void Start()
    {
        instance = this;
    }
}
