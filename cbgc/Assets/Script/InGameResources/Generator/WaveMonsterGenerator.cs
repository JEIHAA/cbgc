using System.Collections;
using UnityEngine;
public class WaveMonsterGenerator : MonoBehaviour
{
    public float distance;
    [System.Serializable]
    public class WaveData
    {
        public GameObject enemy;
        public float waveTime;
        public int amount;
    }
    public WaveData[] waveData;
    void Start()
    {
        StartCoroutine(WaveAppear());
    }
    IEnumerator WaveAppear()
    {
        foreach (var data in waveData)
        {
            yield return new WaitForSeconds(data.waveTime);
            for (int i = 0; i < data.amount; i++)
            {
                var nowEnemy = Instantiate(data.enemy, Random.insideUnitCircle.normalized * distance, Quaternion.identity, transform).GetComponent<Enemy>();
                nowEnemy.moveCenterWhenStart = true;
            }
        }
    }
}
