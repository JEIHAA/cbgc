using UnityEngine;
using System.Collections.Generic;
using System;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField]
    private GenInfo[] generateList;
    [Serializable]
    public class GenInfo
    {
        public ObjectPoolManager.Pool pool;
        public int BorderLength, maxNum = 40, width = 100, height = 100;
        public Vector2 offset;
    }
    GenInfo nowGenObj;
    private HashSet<Vector2> usedPositions = new HashSet<Vector2>();
    private void Start() => RandomGenerate();
    private void RandomGenerate()
    {
        foreach (var obj in generateList)
        {
            nowGenObj = obj;
            for (int i = 0; i < obj.maxNum; ++i)
                if (GenerateUniquePosition(out Vector2 newPos))
                    ObjectPoolManager.instance.GetPool(obj.pool).Get().transform.position = newPos;
        }
    }
    protected bool GenerateUniquePosition(out Vector2 newPos)
    {
        int attempts = 0, maxAttempts = 25;
        do newPos = SetRandomPosValue();
        while (usedPositions.Contains(newPos) && ++attempts < maxAttempts);
        if (attempts >= maxAttempts) return false;
        usedPositions.Add(newPos);
        return true;
    }
    private Vector2 SetRandomPosValue() =>
        nowGenObj.offset + (UnityEngine.Random.value > 0.5f ?
            new Vector2(
                UnityEngine.Random.Range(nowGenObj.width / 2 - nowGenObj.BorderLength, nowGenObj.width / 2) * (UnityEngine.Random.value > 0.5f ? 1 : -1),
                UnityEngine.Random.Range(nowGenObj.height / 2, -nowGenObj.height / 2)) :
            new Vector2(
                UnityEngine.Random.Range(nowGenObj.width / 2, -nowGenObj.width / 2),
                UnityEngine.Random.Range(nowGenObj.height / 2 - nowGenObj.BorderLength, nowGenObj.height / 2) * (UnityEngine.Random.value > 0.5f ? 1 : -1)));
}