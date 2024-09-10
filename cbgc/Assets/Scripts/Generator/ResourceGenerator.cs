using UnityEngine;
using System.Collections.Generic;
public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] protected ObjectPoolManager.Pool[] pools;
    [SerializeField] protected int BorderLength, maxNum = 40, width = 100, height = 100;
    [SerializeField] protected Vector2 offset;
    private HashSet<Vector2> usedPositions = new HashSet<Vector2>();
    private void Start() => RandomGenerate();
    protected void RandomGenerate()
    {
        foreach (var pool in pools)
            for (int i = 0; i < maxNum; ++i)
                if (GenerateUniquePosition(out Vector2 newPos)) 
                    ObjectPoolManager.instance.GetPool(pool).Get().transform.position = newPos;
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
    protected Vector2 SetRandomPosValue() =>
        offset + (Random.value > 0.5f ?
            new Vector2(
                Random.Range(width / 2 - BorderLength, width / 2) * (Random.value > 0.5f ? 1 : -1),
                Random.Range(height / 2, -height / 2)) :
            new Vector2(
                Random.Range(width / 2, -width / 2),
                Random.Range(height / 2 - BorderLength, height / 2) * (Random.value > 0.5f ? 1 : -1)));
}
