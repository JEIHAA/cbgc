using System.Collections;
using UnityEngine;
public class WaveMonsterGenerator : MonoBehaviour
{
    public float spawnDistanceFromCampFire;
    [System.Serializable]
    public class WaveData
    {
        public ObjectPoolManager.Pool spawnEnemy;
        public float waveDelay;
        public int spawnAmount;
    }
    public WaveData[] waveData;
    void Start()
    {
        StartCoroutine(WaveAppear());
    }
    IEnumerator WaveAppear()
    {
        Vector2 randomPos;
        int loopCount = 0, nowWave = 0;
        while (true)
        {
            Debug.Log($"Now wave is {nowWave}");
            yield return new WaitForSeconds(waveData[nowWave].waveDelay);
            for (int i = 0; i < waveData[nowWave].spawnAmount; i++)
            {
                randomPos = Random.insideUnitCircle.normalized * spawnDistanceFromCampFire;
                var nowEnemy = ObjectPoolManager.instance.GetPool(waveData[nowWave].spawnEnemy).Get();
                nowEnemy.transform.position = randomPos;
                nowEnemy.GetComponent<Enemy>().isUpdate = true;
            }
            loopCount += ++nowWave / waveData.Length;
            nowWave %= waveData.Length;
        }
    }
}
