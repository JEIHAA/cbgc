using System;
using System.Collections;
using UnityEngine;
public class WaveMonsterGenerator : MonoBehaviour
{
    public float spawnDistanceFromCampFire;
    [Serializable]
    public class EnemySpawnInfo
    {
        public ObjectPoolManager.Pool spawnEnemy;
        public int spawnAmount;
    }
    [Serializable]
    public class WaveData
    {
        public float waveDelay;
        public EnemySpawnInfo[] enemyInfo;
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
            foreach (var nowEnemy in waveData[nowWave].enemyInfo)
            {   
                for (int i = 0; i < nowEnemy.spawnAmount; i++)
                {
                    randomPos = UnityEngine.Random.insideUnitCircle.normalized * spawnDistanceFromCampFire;
                    var nowEnemySpawned = ObjectPoolManager.instance.GetPool(nowEnemy.spawnEnemy).Get();
                    nowEnemySpawned.transform.position = randomPos;
                    nowEnemySpawned.GetComponent<Enemy>().isUpdate = true;
                }
            }
            loopCount += ++nowWave / waveData.Length;
            nowWave %= waveData.Length;
        }
    }
}
