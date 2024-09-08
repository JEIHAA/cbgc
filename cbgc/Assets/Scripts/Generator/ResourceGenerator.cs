using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] protected ObjectPoolManager.Pool[] pools;
    [SerializeField] protected int maxNum = 40;
    [SerializeField]
    protected float maxX = 9,
                    minX = -9,
                    maxY = 9,
                    minY = -9;

    protected float randomX;
    protected float randomY;
    protected Vector2 randomPos;
    private void Start() => RandomGanerate();//Invoke("RandomGanerate",0.125f);
    protected void RandomGanerate()
    {
        foreach (var pool in pools)
        {
            for (int i = 0; i < maxNum; ++i)
            {
                ObjectPoolManager.instance.GetPool(pool).Get().transform.position = SetRendomPosValue();
            }
        }
    }
    protected virtual Vector3 SetRendomPosValue()
    {
        randomX = Random.Range(maxX, minX);
        randomY = Random.Range(maxY, minY);
        randomPos = new Vector3(randomX, randomY);
        return randomPos;
    }
}
