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
        foreach (var data in waveData)
        {
            yield return new WaitForSeconds(data.waveDelay);
            for (int i = 0; i < data.spawnAmount; i++)
            {
                randomPos = Random.insideUnitCircle.normalized * spawnDistanceFromCampFire;
                var nowEnemy = ObjectPoolManager.instance.GetPool(data.spawnEnemy).Get();
                nowEnemy.transform.position = randomPos;
                nowEnemy.GetComponent<Enemy>().isUpdate = true;
            }
        }
    }
}
