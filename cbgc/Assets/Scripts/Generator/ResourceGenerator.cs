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
                {
                    GameObject nowObject = ObjectPoolManager.instance.GetPool(obj.pool).Get();
                    nowObject.transform.position = newPos;
                    if (obj.pool == ObjectPoolManager.Pool.Tree)
                    {
                        Tree nowTree = nowObject.GetComponent<Tree>();
                        nowTree.TreeLevel =
                            1 + (int)Mathf.Lerp(0, 2.99f, //거리 비례해서 1~3의 값 나오도록
                            newPos.magnitude / Mathf.Sqrt(Mathf.Pow(obj.width/2, 2) + Mathf.Pow(obj.width/2, 2)));//중앙으로부터 얼마나 떨어졌는지 0~1 값으로
                        //nowTree.SetTreeLevel();
                    }
                }
                    
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