using System.Collections;
using UnityEngine;
public class WaveMonsterGenerator : MonoBehaviour
{
    public float spawnDistanceFromCampFire;
    [System.Serializable]
    public class WaveData
    {
        public GameObject spawnEnemy;
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
        foreach (var data in waveData)
        {
            yield return new WaitForSeconds(data.waveDelay);
            if(data.spawnEnemy != null)
                for (int i = 0; i < data.spawnAmount; i++)
                {
                    var nowEnemy = Instantiate(data.spawnEnemy, Random.insideUnitCircle.normalized * spawnDistanceFromCampFire, Quaternion.identity, transform).GetComponent<Enemy>();
                    nowEnemy.isUpdate = true;
                }
        }
    }
}
