using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] protected GameObject obj;
    protected float maxX = 9;
    protected float minX = -9;
    protected float maxY = 9;
    protected float minY = -9;

    protected int maxNum = 40;

    protected float randomX;
    protected float randomY;
    protected Vector2 randomPos;

    protected void RandomGanerate()
    {
        for (int i = 0; i < maxNum; ++i) 
        {
            randomX = Random.Range(maxX, minX);
            randomY = Random.Range(maxY, minY);
            randomPos = new Vector3(randomX, randomY);
            Instantiate(obj, randomPos, Quaternion.identity);   
        }
    }

    
}
