using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] protected GameObject obj;
    [SerializeField] protected int maxNum = 40;
    [SerializeField]
    protected float maxX = 9,
                    minX = -9,
                    maxY = 9,
                    minY = -9;


    protected float randomX;
    protected float randomY;
    protected Vector2 randomPos;
    private void Start()
    {
        RandomGanerate();
    }
    protected void RandomGanerate()
    {
        for (int i = 0; i < maxNum; ++i) 
        {
            randomPos = SetRendomPosValue();
            Instantiate(obj, randomPos, Quaternion.identity,transform);   
        }
    }

    protected Vector3 SetRendomPosValue()
    {
        randomX = Random.Range(maxX, minX);
        randomY = Random.Range(maxY, minY);
        randomPos = new Vector3(randomX, randomY);
        return randomPos;
    }
}
