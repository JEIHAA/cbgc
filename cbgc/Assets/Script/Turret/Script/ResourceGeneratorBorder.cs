using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceGeneratorBorder : ResourceGenerator
{
    [SerializeField]
    protected float BorderLength;
    private void Start()
    {
        RandomGanerate();
    }
    public new void RandomGanerate()
    {
        for (int i = 0; i < maxNum; ++i)
        {
            randomPos = (Random.value > 0.5f ?  
                new Vector3(Random.Range(maxX, maxX - BorderLength) * (Random.value > 0.5f ? 1 : -1), Random.Range(maxY, minY)) :
                new Vector3(Random.Range(maxX, minX), Random.Range(maxY, maxY - BorderLength) * (Random.value > 0.5f ? 1 : -1)));
            Instantiate(obj, randomPos, Quaternion.identity,transform);
        }
    }
}
