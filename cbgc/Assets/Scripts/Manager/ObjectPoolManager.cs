using System;
using UnityEngine;
using UnityEngine.Pool;
public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    [SerializeField]
    private ObjectInPool[] poolsInfo;
    [Serializable]
    private class ObjectInPool
    {
        public GameObject prefeb;
        public int initialPoolSize;
        public ObjectPool<GameObject> objectPools;
    }
    public enum Pool
    {
        FastMonster,
        EyeMonster,
        Tree,
    }
    public ObjectPool<GameObject> GetPool(Pool pool) => poolsInfo[(int)pool].objectPools;
    private void Awake()
    {
        instance = this;
        for (int i = 0; i < poolsInfo.Length; i++) PoolInit(i);
    }
    void PoolInit(int idx)
    {
        poolsInfo[idx].objectPools = new ObjectPool<GameObject>(
            () => Instantiate(poolsInfo[idx].prefeb, transform),
            (GameObject obj) => obj.SetActive(true),
            (GameObject obj) => obj.SetActive(false),
            (GameObject obj) => Destroy(obj),
            false,
            poolsInfo[idx].initialPoolSize,
            poolsInfo[idx].initialPoolSize
        );
    }
}