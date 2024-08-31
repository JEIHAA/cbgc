using System;
using System.Collections;
using UnityEngine;

public class Tree : InteractiveObject
{
    [SerializeField] private int leftFirewood = 5;
    
    private Animator animator;
    public Animator Anim => animator;

    private int axingCnt=0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Use()
    {
        --leftFirewood;
        Debug.Log("Chop!");
        ResourceData.LogAmount += 1;
        if (leftFirewood <= 0) 
        {
            Invoke("Delay", 0.4f);
            gameObject.SetActive(false);
        }
    }

    public IEnumerator ReUseTime(float _time)
    {
        yield return new WaitForSeconds(_time);
    }
    

    public override void Interaction(float _time)
    {
        if (_time > 1f) 
        {
            animator.SetTrigger("Hit");
        }
        axingCnt++;
        if (axingCnt >= 1)
        {
            Use();
            axingCnt = 0;
        }
    }

    private void Delay()
    {
        Debug.Log("Delay");
        return;
    }

}