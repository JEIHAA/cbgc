using System;
using System.Collections;
using UnityEngine;

public class Tree : InteractiveObject
{
    private const float treeChopTimeConst = 1f;
    public int leftFirewood = 5;

    public void Use()
    {
        --leftFirewood;
        Debug.Log("Chop!");
        if (leftFirewood <= 0) gameObject.SetActive(false);
    }

    public IEnumerator ReUseTime(float _time)
    {
        yield return new WaitForSeconds(_time);
    }
    

    public override void Interaction(float _time)
    {
        if (_time > 1f)
        {
            Debug.Log("Tree Interaction");
            Use();
        }
    }
}