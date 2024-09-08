using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceGeneratorBorder : ResourceGenerator
{
    [SerializeField]
    protected float BorderLength;
    protected override Vector3 SetRendomPosValue()
    {
        return (Random.value > 0.5f ?
                    new Vector3(Random.Range(maxX, maxX - BorderLength) * (Random.value > 0.5f ? 1 : -1), Random.Range(maxY, minY)) :
                    new Vector3(Random.Range(maxX, minX), Random.Range(maxY, maxY - BorderLength) * (Random.value > 0.5f ? 1 : -1)));
    }
}
