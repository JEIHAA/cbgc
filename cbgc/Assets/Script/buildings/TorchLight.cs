using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TorchLight : SetLightAnimator
{
    public int LifeTime;

    private void Start()
    {
        maxTime = 50;
        Debug.Log(leftTime);
        anims = GetComponentsInChildren<Animator>();
        BonfireSetFloat();
        InvokeRepeating("TimeCount", 1, 1);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<LightShimmer>() != null) 
        {
            leftTime = maxTime;
        }   
    }

    protected override void BonfireSetFloat()
    {
        foreach (Animator anim in anims)
        {
            anim.SetFloat("LeftTime", leftTime);
            LifeTime = leftTime;
        }
    }

    public void ResizeColl()
    {
        return;
    }
}
