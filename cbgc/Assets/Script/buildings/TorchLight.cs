using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TorchLight : SetLightAnimator
{
    private int lifeTime;
    public int LifeTime=>lifeTime;

    [SerializeField] private Bonfire bonfire;
    private bool onFire = false;

    private void Start()
    {
        maxTime = 50;
        anims = GetComponentsInChildren<Animator>();
        FireLightSetFloat();
        InvokeRepeating("TimeCount", 1, 1);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BonfireLight"))
        {
            onFire = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BonfireLight"))
        {
            onFire = false;
        }
    }

    private void Update()
    {
        if (onFire && HasLeftTime()) 
        {
            leftTime = maxTime;
        }
    }

    protected override void FireLightSetFloat()
    {
        foreach (Animator anim in anims)
        {
            anim.SetFloat("LeftTime", leftTime);
            lifeTime = leftTime;
        }
    }

    private bool HasLeftTime()
    {
        if (bonfire.LeftTime >= 1) return true;
        else return false;
    }
}
