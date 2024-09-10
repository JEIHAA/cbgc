using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] protected ObjectPoolManager.Pool[] pools;
    [SerializeField] protected int BorderLength;
    [SerializeField] protected int maxNum = 40;
    [SerializeField]
    protected int width = 100, height = 100;
                    
    private void Start() => RandomGanerate();//Invoke("RandomGanerate",0.125f);
    protected virtual void RandomGanerate()
    {
        foreach (var pool in pools)
        {
            for (int i = 0; i < maxNum; ++i)
            {
                ObjectPoolManager.instance.GetPool(pool).Get().transform.position = SetRendomPosValue();
            }
        }
    }
    protected Vector3 SetRendomPosValue()
    {
        return (Random.value > 0.5f ?
                    new Vector3(Random.Range(width/2, width/2 - BorderLength) * (Random.value > 0.5f ? 1 : -1), Random.Range(height / 2, -height / 2)) :
                    new Vector3(Random.Range(width/2, -width/2), Random.Range(height/2, height/2 - BorderLength) * (Random.value > 0.5f ? 1 : -1)));
    }
}
