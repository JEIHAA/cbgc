using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public ObjectPool<GameObject> GetPool(Pool pool)
    {
        return poolsInfo[(int)pool].objectPools;
    }
    private void Awake()
    {
        instance = this;
        //I dont know why using "for loop" makes index out error... :(
        poolsInfo[0].objectPools = new ObjectPool<GameObject>(
            () => Instantiate(poolsInfo[0].prefeb,transform),
            (GameObject obj) => obj.SetActive(true),
            (GameObject obj) => obj.SetActive(false),
            (GameObject obj) => Destroy(obj),
            false,
            poolsInfo[0].initialPoolSize,
            poolsInfo[0].initialPoolSize
            );
        poolsInfo[1].objectPools = new ObjectPool<GameObject>(
            () => Instantiate(poolsInfo[1].prefeb, transform),
            (GameObject obj) => obj.SetActive(true),
            (GameObject obj) => obj.SetActive(false),
            (GameObject obj) => Destroy(obj),
            false,
            poolsInfo[1].initialPoolSize,
            poolsInfo[1].initialPoolSize
            );
        poolsInfo[2].objectPools = new ObjectPool<GameObject>(
            () => Instantiate(poolsInfo[2].prefeb, transform),
            (GameObject obj) => obj.SetActive(true),
            (GameObject obj) => obj.SetActive(false),
            (GameObject obj) => Destroy(obj),
            false,
            poolsInfo[2].initialPoolSize,
            poolsInfo[2].initialPoolSize
            );
    }
}
